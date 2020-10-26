/**
 * This files generates C# classes from the TON Client SDK definitions location in JSON file (api.json).
 * The JSON file is taken from https://raw.githubusercontent.com/tonlabs/TON-SDK/1.0.0-rc/tools/api.json.
 */

import {TextWriter} from '@yellicode/core';
import {Generator} from '@yellicode/templating';
import {CSharpWriter, EnumDefinition, EnumMemberDefinition, ParameterDefinition} from '@yellicode/csharp';
import {TonApiSpec} from './api';

function ucFirst(value: string): string {
    return value.substring(0, 1).toUpperCase() + value.substring(1);
}

function toCamelCase(value: string): string {
    return ucFirst(value)
        .replace(/[\\-_\\+](\w)/g,
            match => match.substring(1).toUpperCase());
}

function toCSharpModuleName(name: string): string {
    return `${ucFirst(name)}Module`;
}

function toCSharpModuleInterfaceName(name: string): string {
    return `I${toCSharpModuleName(name)}`;
}

function toCSharpPrivateModulePropertyName(name: string): string {
    return `_${name}Module`;
}

function toCSharpMethodName(functionName: string): string {
    return `${toCamelCase(functionName)}Async`;
}

function toCSharpParameterName(paramName: string): string {
    // reserved word check
    return 'params' === paramName ||
    'return' === paramName ||
    'event' === paramName
        ? `@${paramName}` : paramName;
}

function toCSharpMethodReturnType(result: TonApiSpec.Result) {
    const returnType = getReturnType(result);
    return ('void' !== returnType)
        ? `Task<${returnType}>`
        : 'Task';
}

function toCSharpPropertyName(propertyName: string, typeName: string): string {
    let resultPropertyName = toCamelCase(propertyName);
    return (resultPropertyName !== typeName)
        // member names cannot be the same as their enclosed type
        ? resultPropertyName : resultPropertyName + 'Property';
}

function toCSharpValueType(value: TonApiSpec.HasValue, optional: boolean = false): string {
    switch (value.type) {
        case 'String':
            return 'string';
        case 'Number':
            return 'int' + (optional ? '?' : '');
        case 'Boolean':
            return 'bool' + (optional ? '?' : '');
        case 'Ref':
            // FIXME: no concrete types are defined for these API types in api.json; returning raw JSON.
            if ('API' === value.ref_name ||
                'Value' === value.ref_name ||
                'TransactionFees' === value.ref_name) {
                return 'Newtonsoft.Json.Linq.JRaw';
            }
            // FIXME: SigningBoxHandle is a number
            if ('SigningBoxHandle' === value.ref_name) {
                return 'decimal';
            }
            return value.ref_name;
        case 'Optional':
            return toCSharpValueType(value.optional_inner, true);
        case 'Array':
            return `${toCSharpValueType(value.array_item)}[]`;
        case 'BigInt':
            return 'BigInteger';
        default:
            throw new Error(`Unsupported field type: ${value.type}`);
    }
}

function getModuleClassLocation(module: TonApiSpec.Module): string {
    let moduleName = toCSharpModuleName(module.name);
    return `../src/Modules/${moduleName}.cs`;
}

function writeUsingDirectives(writer: CSharpWriter): CSharpWriter {
    return writer.writeUsingDirectives(
        'Newtonsoft.Json',
        'System',
        'System.Numerics',
        'System.Threading.Tasks',
        'TonSdk.Modules'
    );
}

function writeModuleFileNotes(module: TonApiSpec.Module, version: string, writer: CSharpWriter): CSharpWriter {
    return writer.writeDelimitedCommentParagraph([
        `TON API version ${version}, ${module.name} module.`,
        'THIS FILE WAS GENERATED AUTOMATICALLY.'
    ]);
}

function getXmlDocSummary(summary: string, description: string = null): string[] | undefined {
    return description
        ? description.split('\n')
        : summary ? summary.split('\n')
            : null;
}

function getParameterType(param: TonApiSpec.Param): string {
    switch (param.type) {
        case 'Ref':
            return param.ref_name;
        case 'Generic':
            // Not supposed to happen, just in case
            return `${param.generic_name}<${param.generic_args.map(a => a.ref_name).join(',')}>`;
        default:
            throw new Error(`Unsupported param type: ${param.type}`);
    }
}

function getReturnType(result: TonApiSpec.Result): string {
    if ('Generic' !== result.type) {
        throw new Error(`Unsupported result type: ${result.type}`);
    }
    if ('ClientResult' !== result.generic_name) {
        throw new Error(`Unexpected return type: ${result.generic_name}`);
    }
    if (1 !== result.generic_args.length) {
        throw new Error(`Unexpected return type: ${result.generic_name}<${result.generic_args.map(a => a.ref_name).join(',')}>`);
    }
    if ('None' === result.generic_args[0].type) {
        return 'void';
    }
    if ('Ref' !== result.generic_args[0].type) {
        throw new Error(`Unexpected generic_args type: ${result.generic_args[0].type}`);
    }
    return result.generic_args[0].ref_name;
}

function writeStructDefinition(module: TonApiSpec.Module, type: TonApiSpec.Type, writer: CSharpWriter, parent: TonApiSpec.Type = null): boolean {
    if (type.struct_fields.length && !type.struct_fields[0].name) {
        return false;
    }

    writer.writeClassBlock({
        accessModifier: 'public',
        name: type.name,
        xmlDocSummary: getXmlDocSummary(type.summary, type.description),
        inherits: parent ? [parent.name] : null
    }, () => {
        for (let i = 0, n = type.struct_fields.length; i < n; ++i) {
            const field = type.struct_fields[i];
            writer.writeXmlDocSummary(getXmlDocSummary(field.summary, field.description));
            writer.writeLine(`[JsonProperty("${field.name}")]`);
            writer.writeAutoProperty({
                accessModifier: 'public',
                typeName: toCSharpValueType(field),
                name: toCSharpPropertyName(field.name, type.name)
            });
            if (i < n - 1) {
                writer.writeLine();
            }
        }
    });

    return true;
}

function writeEnumDefinition(module: TonApiSpec.Module, type: TonApiSpec.Type, writer: CSharpWriter): boolean {
    writer.writeEnumeration({
        accessModifier: 'public',
        xmlDocSummary: getXmlDocSummary(type.summary, type.description),
        name: type.name,
        members: type.enum_consts.map(e => ({
            name: e.name,
            xmlDocSummary: getXmlDocSummary(e.summary, e.description)
        } as EnumMemberDefinition))
    } as EnumDefinition);
    return true;
}

function writeAbstractTypeDefinition(module: TonApiSpec.Module, type: TonApiSpec.Type, writer: CSharpWriter, parent: TonApiSpec.Type = null): boolean {
    writer.writeClassBlock({
        accessModifier: 'public',
        xmlDocSummary: getXmlDocSummary(type.summary, type.description),
        name: type.name,
        isAbstract: !parent
    }, () => {
        for (let i = 0, n = type.enum_types.length; i < n; ++i) {
            writeTypeDefinition(module, type.enum_types[i], writer, type);
            if (i < n - 1) {
                writer.writeLine();
            }
        }
    });
    return true;
}

function writeTypeDefinition(module: TonApiSpec.Module, type: TonApiSpec.Type, writer: CSharpWriter, parent: TonApiSpec.Type = null): boolean {
    switch (type.type) {
        case 'Struct':
            return writeStructDefinition(module, type, writer, parent);

        case 'EnumOfTypes':
            return writeAbstractTypeDefinition(module, type, writer, parent);

        case 'EnumOfConsts':
            return writeEnumDefinition(module, type, writer);

        case 'None':
            return false;

        default:
            throw new Error(`Unsupported type: ${type.type}`);
    }
}

function writeTypeDefinitions(module: TonApiSpec.Module, writer: CSharpWriter): CSharpWriter {
    for (let i = 0, n = module.types.length; i < n; ++i) {
        const type = module.types[i];
        if (writeTypeDefinition(module, type, writer)) {
            writer.writeLine();
        }
    }
    return writer;
}

function getFunctionParameters(fn: TonApiSpec.Function): ParameterDefinition[] {
    return fn.params
        .filter(p => p.type != 'Generic' && p.name != '_context')
        .map(p => ({
            name: toCSharpParameterName(p.name),
            typeName: getParameterType(p),
            xmlDocSummary: getXmlDocSummary(p.summary, p.description)
        } as ParameterDefinition));
}

function writeInterfaceMethod(module: TonApiSpec.Module, fn: TonApiSpec.Function, writer: CSharpWriter): CSharpWriter {
    return writer.writeMethodDeclaration({
        name: toCSharpMethodName(fn.name),
        parameters: getFunctionParameters(fn),
        returnTypeName: toCSharpMethodReturnType(fn.result),
        xmlDocSummary: getXmlDocSummary(fn.summary, fn.description)
    });
}

function writeMethodImplementation(module: TonApiSpec.Module, fn: TonApiSpec.Function, writer: CSharpWriter): CSharpWriter {
    const returnType = toCSharpMethodReturnType(fn.result);
    return writer
        .writeMethodBlock({
            accessModifier: 'public',
            name: toCSharpMethodName(fn.name),
            parameters: getFunctionParameters(fn),
            returnTypeName: `async ${returnType}`
        }, () => {
            const hasParam = 2 === fn.params.length; // param #1 is always _context
            if ('Task' === returnType) {
                if (hasParam) {
                    writer.writeLine(`await _client.CallFunctionAsync("${module.name}.${fn.name}", ${toCSharpParameterName(fn.params[1].name)}).ConfigureAwait(false);`)
                } else {
                    writer.writeLine(`await _client.CallFunctionAsync("${module.name}.${fn.name}").ConfigureAwait(false);`)
                }
            } else {
                if (hasParam) {
                    writer.writeLine(`return await _client.CallFunctionAsync<${getReturnType(fn.result)}>("${module.name}.${fn.name}", ${toCSharpParameterName(fn.params[1].name)}).ConfigureAwait(false);`)
                } else {
                    writer.writeLine(`return await _client.CallFunctionAsync<${getReturnType(fn.result)}>("${module.name}.${fn.name}").ConfigureAwait(false);`)
                }
            }
        });
}

function writeModuleInterface(module: TonApiSpec.Module, writer: CSharpWriter): CSharpWriter {
    return writer.writeInterfaceBlock({
        accessModifier: 'public',
        name: toCSharpModuleInterfaceName(module.name),
        xmlDocSummary: getXmlDocSummary(module.summary, module.description)
    }, () => {
        for (let i = 0, n = module.functions.length; i < n; ++i) {
            writeInterfaceMethod(module, module.functions[i], writer);
            if (i < n - 1) {
                writer.writeLine();
            }
        }
    }).writeLine();
}

function writeModuleImplementation(module: TonApiSpec.Module, writer: CSharpWriter): CSharpWriter {
    let moduleName = toCSharpModuleName(module.name);
    return writer.writeClassBlock({
        accessModifier: 'internal',
        name: moduleName,
        inherits: [toCSharpModuleInterfaceName(module.name)]
    }, () => {
        writer
            .writeLine('private readonly TonClient _client;')
            .writeLine()
            .writeMethodBlock({
                accessModifier: 'internal',
                isConstructor: true,
                name: moduleName,
                parameters: [
                    {
                        name: 'client',
                        typeName: 'TonClient'
                    }
                ]
            }, () => {
                writer.writeLine('_client = client ?? throw new ArgumentNullException(nameof(client));');
            })
            .writeLine();

        for (let i = 0, n = module.functions.length; i < n; ++i) {
            writeMethodImplementation(module, module.functions[i], writer);
            if (i < n - 1) {
                writer.writeLine();
            }
        }
    });
}

function writeModule(module: TonApiSpec.Module, writer: CSharpWriter): CSharpWriter {
    return writer.writeNamespaceBlock({name: 'TonSdk.Modules'}, () => {
        writeTypeDefinitions(module, writer);
        writeModuleInterface(module, writer);
        writeModuleImplementation(module, writer);
    });
}

function writeClientExtension(module: TonApiSpec.Module, writer: CSharpWriter): CSharpWriter {
    return writer.writeNamespaceBlock({name: 'TonSdk'}, () => {

        const modulePropertyName = ucFirst(module.name);
        const modulePrivatePropertyName = toCSharpPrivateModulePropertyName(module.name);
        const moduleInterfaceName = toCSharpModuleInterfaceName(module.name);
        const moduleImplTypeName = toCSharpModuleName(module.name);

        writer.writeInterfaceBlock({
            accessModifier: 'public',
            isPartial: true,
            name: 'ITonClient'
        }, () => {
            writer.writeAutoProperty({
                typeName: moduleInterfaceName,
                name: modulePropertyName,
                noSetter: true
            });
        });

        writer.writeLine();

        writer.writeClassBlock({
            accessModifier: 'public',
            isPartial: true,
            name: 'TonClient'
        }, () => {

            writer
                .decreaseIndent()
                .writeLineIndented(`private ${moduleImplTypeName} ${modulePrivatePropertyName};`)
                .writeLine()
                .increaseIndent();

            writer.writePropertyBlock({
                accessModifier: 'public',
                typeName: moduleInterfaceName,
                name: modulePropertyName,
                noSetter: true
            }, () => {
                writer.writeLine(`return ${modulePrivatePropertyName} ?? (${modulePrivatePropertyName} = new ${moduleImplTypeName}(this));`);
            }, null);
        });
    });
}

function generateModuleClass(module: TonApiSpec.Module, version: string, output: TextWriter) {
    const writer = new CSharpWriter(output);
    writer.indentString = '    ';
    writeUsingDirectives(writer).writeLine();
    writeModuleFileNotes(module, version, writer).writeLine();
    writeModule(module, writer).writeLine();
    writeClientExtension(module, writer).writeLine();
}

Generator.getModel().then((model: any) => {
    const root = model as TonApiSpec.RootObject;
    root.modules.forEach((module) => {
        Generator.generate({
            outputFile: getModuleClassLocation(module)
        }, (writer: TextWriter) => generateModuleClass(module, root.version, writer));
    });
});
