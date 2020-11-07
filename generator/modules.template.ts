/**
 * This files generates C# classes from the TON Client SDK definitions location in JSON file (api.json).
 * The JSON file is taken from https://raw.githubusercontent.com/tonlabs/TON-SDK/1.1.0-rc/tools/api.json.
 */

import {TextWriter} from '@yellicode/core';
import {Generator} from '@yellicode/templating';
import {CSharpWriter, EnumDefinition, EnumMemberDefinition, ParameterDefinition} from '@yellicode/csharp';
import {TonApiSpec} from './api';

const numericTypes: { [moduleName: string]: { [typeName: string]: TonApiSpec.Numeric } } = {};
const referenceTypes: { [moduleName: string]: { [typeName: string]: TonApiSpec.Type } } = {};
const typesByModuleAndName: { [moduleName: string]: { [typeName: string]: TonApiSpec.Type } } = {};

function ucFirst(value: string): string {
    return value.substring(0, 1).toUpperCase() + value.substring(1);
}

function toCamelCase(value: string): string {
    return ucFirst(value)
        .replace(/[\\-_\\+\s](\w)/g,
            match => match.substring(1).toUpperCase());
}

function toCSharpEventCallbackTypeName(moduleName: string): string {
    return toCamelCase(moduleName) + 'Event';
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

function toCSharpNumericType(value: TonApiSpec.Numeric): string {
    switch (value.number_type) {
        case 'UInt':
            switch (value.number_size) {
                case 8:
                    return 'byte';
                case 16:
                    return 'ushort';
                case 32:
                    return 'uint';
                case 64:
                    return 'ulong';
                default:
                    return 'uint';
            }
        case 'Int':
            switch (value.number_size) {
                case 8:
                    return 'sbyte';
                case 16:
                    return 'short';
                case 32:
                    return 'int';
                case 64:
                    return 'long';
                default:
                    return 'int';
            }
        case 'Float':
            switch (value.number_size) {
                case 32:
                    return 'float';
                case 64:
                    return 'double';
                default:
                    return 'float';
            }
    }
    return 'int';
}

function isNumericType(moduleName: string, typeName: string): boolean {
    return !!(numericTypes[moduleName] && numericTypes[moduleName][typeName]);
}

function getNumericType(moduleName: string, typeName: string): string {
    const typeSpec = numericTypes[moduleName] && numericTypes[moduleName][typeName];
    if (!typeSpec) {
        throw new Error(`Type ${moduleName}.${typeName} is not numeric`);
    }
    return toCSharpNumericType(typeSpec);
}

function parseRef(ref: string): string[] {
    const refSpec = ref.split('.'); // moduleName.typeName
    if (refSpec.length != 2) {
        throw new Error(`Unsupported type ref: ${ref}; Expected moduleName.typeName`);
    }
    return refSpec;
}

function typeExists(moduleName: string, typeName: string): boolean {
    return !!(typesByModuleAndName[moduleName] && typesByModuleAndName[moduleName][typeName]);
}

function getTypeByName(moduleName: string, typeName: string): TonApiSpec.Type {
    if (!typesByModuleAndName[moduleName] || !typesByModuleAndName[moduleName][typeName]) {
        throw new Error(`Type ${moduleName}.${typeName} not found`);
    }
    return typesByModuleAndName[moduleName][typeName];
}

function getReferenceType(moduleName: string, typeName: string): TonApiSpec.Type {
    if (!referenceTypes[moduleName] || !referenceTypes[moduleName][typeName]) {
        throw new Error(`Type ${moduleName}.${typeName} reference not found`);
    }
    return referenceTypes[moduleName][typeName];
}

function isUnknownType(refSpec: string): boolean {
    // FIXME: no concrete types are defined for these API types in api.json; returning raw JSON.
    return ('API' === refSpec || 'Value' === refSpec);
}

function isContextParam(param: TonApiSpec.Param): boolean {
    return param.type === 'Generic' &&
        param.generic_args &&
        param.generic_args.length &&
        param.generic_args[0].type === 'Ref' &&
        param.generic_args[0].ref_name === 'ClientContext';
}

function isCallbackParam(param: TonApiSpec.Param): boolean {
    return param.type === 'Generic' &&
        param.generic_args &&
        param.generic_args.length &&
        param.generic_args[0].type === 'Ref' &&
        param.generic_args[0].ref_name === 'Request';
}

function toCSharpValueType(value: TonApiSpec.HasValue, optional: boolean = false): string {
    switch (value.type) {
        case 'String':
            return 'string';
        case 'Number':
            const numberType = toCSharpNumericType(value);
            return optional ? `${numberType}?` : numberType;
        case 'Boolean':
            return 'bool' + (optional ? '?' : '');
        case 'Ref':
            if (isUnknownType(value.ref_name)) {
                return 'Newtonsoft.Json.Linq.JToken';
            }
            const [moduleName, typeName] = parseRef(value.ref_name);
            if (isNumericType(moduleName, typeName)) {
                return getNumericType(moduleName, typeName);
            }
            return typeName;
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

function getCSharpModuleClassLocation(module: TonApiSpec.Module): string {
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
            const [, typeName] = parseRef(param.ref_name);
            return typeName;
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
    const [, typeName] = parseRef(result.generic_args[0].ref_name);
    return typeName;
}

function writeTypeMembers(type: TonApiSpec.Type, writer: CSharpWriter) {
    for (let i = 0, n = type.struct_fields.length; i < n; ++i) {
        const field = type.struct_fields[i];
        writer.writeXmlDocSummary(getXmlDocSummary(field.summary, field.description));
        writer.writeLine(`[JsonProperty("${field.name}", NullValueHandling = NullValueHandling.Ignore)]`);

        let ref_name = 'Ref' === field.type
            ? field.ref_name
            : 'Optional' === field.type && field.optional_inner && field.optional_inner.type == 'Ref'
                ? field.optional_inner.ref_name
                : undefined;

        if (ref_name && !isUnknownType(ref_name)) {
            const [moduleName, typeName] = parseRef(ref_name);
            const type = getTypeByName(moduleName, typeName);
            if (type.type === 'EnumOfTypes') {
                writer.writeLine(`[JsonConverter(typeof(PolymorphicConcreteTypeConverter))]`);
            }
        }

        writer.writeAutoProperty({
            accessModifier: 'public',
            typeName: toCSharpValueType(field),
            name: toCSharpPropertyName(field.name, type.name)
        });
        if (i < n - 1) {
            writer.writeLine();
        }
    }
}

function writeStructDefinition(module: TonApiSpec.Module, type: TonApiSpec.Type, writer: CSharpWriter, parent: TonApiSpec.Type = null): boolean {
    writer.writeClassBlock({
        accessModifier: 'public',
        name: type.name,
        xmlDocSummary: getXmlDocSummary(type.summary, type.description),
        inherits: parent ? [parent.name] : null
    }, () => writeTypeMembers(type, writer));
    return true;
}

function writeReferenceTypeDefinition(module: TonApiSpec.Module, type: TonApiSpec.Type, writer: CSharpWriter, parent: TonApiSpec.Type = null): boolean {
    writer.writeClassBlock({
        accessModifier: 'public',
        name: type.name,
        xmlDocSummary: getXmlDocSummary(type.summary, type.description),
        inherits: parent ? [parent.name] : null
    }, () => {
        // We represent reference classes as a pure copies of the referenced class.
        // Can't use inheritance here because the reference class may already have parent (passed as the last function argument).
        const referenceType = getReferenceType(module.name, type.name);
        writeTypeMembers(referenceType, writer);
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

        case 'Ref':
            return writeReferenceTypeDefinition(module, type, writer, parent);

        case 'Number':
        case 'None':
            return false;

        default:
            throw new Error(`Unsupported type: ${type.type} (${module.name}.${type.name})`);
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

function functionHasParam(fn: TonApiSpec.Function): boolean {
    return fn.params.filter(p => !isContextParam(p)).length > 1;
}

function functionHasCallback(fn: TonApiSpec.Function): boolean {
    return fn.params.filter(p => isCallbackParam(p)).length > 0;
}

function getFunctionCallbackType(module: TonApiSpec.Module, fn: TonApiSpec.Function): string {
    if (!functionHasCallback(fn)) {
        return null;
    }
    const concreteType = toCSharpEventCallbackTypeName(module.name);
    return typeExists(module.name, concreteType)
        ? concreteType : 'Newtonsoft.Json.Linq.JToken';
}

function toCSharpParameterDefinition(module: TonApiSpec.Module, fn: TonApiSpec.Function, p: TonApiSpec.Param): ParameterDefinition {
    const callbackType = isCallbackParam(p) &&
        getFunctionCallbackType(module, fn);
    const result = {
        name: toCSharpParameterName(p.name),
        typeName: callbackType
            ? `Action<${callbackType}, int>`
            : getParameterType(p),
        xmlDocSummary: getXmlDocSummary(p.summary, p.description)
    } as ParameterDefinition;
    if (callbackType) {
        result.defaultValue = 'null';
    }
    return result;
}

function getFunctionParameters(module: TonApiSpec.Module, fn: TonApiSpec.Function): ParameterDefinition[] {
    return fn.params
        .filter(p => !isContextParam(p))
        .map(p => toCSharpParameterDefinition(module, fn, p));
}

function writeInterfaceMethod(module: TonApiSpec.Module, fn: TonApiSpec.Function, writer: CSharpWriter): CSharpWriter {
    return writer.writeMethodDeclaration({
        name: toCSharpMethodName(fn.name),
        parameters: getFunctionParameters(module, fn),
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
            parameters: getFunctionParameters(module, fn),
            returnTypeName: `async ${returnType}`
        }, () => {

            const callArguments = [`"${module.name}.${fn.name}"`]
                .concat(getFunctionParameters(module, fn)
                    .map(p => p.name));

            let returnSpec = '';
            const genericArguments = [];
            if ('Task' !== returnType) {
                genericArguments.push(getReturnType(fn.result))
                returnSpec = 'return ';
            }

            const callbackType = getFunctionCallbackType(module, fn);
            if (callbackType) {
                genericArguments.push(callbackType);
            }

            if (genericArguments.length) {
                writer.writeLine(`${returnSpec}await _client.CallFunctionAsync<${genericArguments.join(', ')}>(${callArguments.join(', ')}).ConfigureAwait(false);`)
            } else {
                writer.writeLine(`${returnSpec}await _client.CallFunctionAsync(${callArguments.join(', ')}).ConfigureAwait(false);`)
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

function indexType(module: TonApiSpec.Module, type: TonApiSpec.Type) {
    if ('Number' === type.type) {
        console.debug(`Found numeric type: ${module.name}.${type.name}`);
        if (!numericTypes[module.name]) {
            numericTypes[module.name] = {};
        }
        numericTypes[module.name][type.name] = type;
    } else if ('Ref' === type.type) {
        console.debug(`Found reference type: ${module.name}.${type.name} -> ${type.ref_name}`);
        if (!referenceTypes[module.name]) {
            referenceTypes[module.name] = {};
        }
        const [moduleName, typeName] = parseRef(type.ref_name);
        referenceTypes[module.name][type.name] = getTypeByName(moduleName, typeName);
    } else if ('EnumOfTypes' === type.type) {
        type.enum_types.forEach(t => {
            indexType(module, t);
        });
    }
}

function indexModuleTypes(modules: TonApiSpec.Module[]) {

    modules.forEach(module => {
        if (!typesByModuleAndName[module.name]) {
            typesByModuleAndName[module.name] = {};
        }
        module.types.forEach(type => {
            typesByModuleAndName[module.name][type.name] = type;
        });
    });

    modules.forEach(module => {
        module.types.forEach(type => indexType(module, type))
    });
}

Generator.getModel().then((model: any) => {
    const root = model as TonApiSpec.RootObject;
    indexModuleTypes(root.modules);
    root.modules.forEach((module) => {
        Generator.generate({
            outputFile: getCSharpModuleClassLocation(module)
        }, (writer: TextWriter) => generateModuleClass(module, root.version, writer));
    });
});
