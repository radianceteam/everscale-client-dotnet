'use strict';

// Build module function index.
// This is needed to identify changes in api.json.
// Suggested is to use it after each API version update,
// by running `npm run index`
// and then comparing the changes using git diff.

import {TonApiSpec} from "./api";
import * as fs from 'fs';

let json: string = fs.readFileSync('api.json').toString();
const api: TonApiSpec.Api = JSON.parse(json);

function indexModuleType(module: TonApiSpec.Module, type: TonApiSpec.Type, stream: fs.WriteStream) {
    stream.write(`${module.name}.${type.name}\n`);
}

function indexModuleFunction(module: TonApiSpec.Module, f: TonApiSpec.Function, stream: fs.WriteStream) {
    stream.write(`${module.name}.${f.name}\n`);
}

function indexModuleTypes(module: TonApiSpec.Module, stream: fs.WriteStream) {
    for (const t of module.types) {
        indexModuleType(module, t, stream);
    }
}

function indexModuleFunctions(module: TonApiSpec.Module, stream: fs.WriteStream) {
    for (const f of module.functions) {
        indexModuleFunction(module, f, stream);
    }
}

function indexApi(api: TonApiSpec.Api, stream: fs.WriteStream) {
    stream.write(`${api.version}\n`);
    stream.write(`TYPES\n`);
    for (const module of api.modules) {
        indexModuleTypes(module, stream);
    }
    stream.write(`FUNCTIONS\n`);
    for (const module of api.modules) {
        indexModuleFunctions(module, stream);
    }
}

const stream = fs.createWriteStream('api.index.txt');
indexApi(api, stream);
stream.end();
