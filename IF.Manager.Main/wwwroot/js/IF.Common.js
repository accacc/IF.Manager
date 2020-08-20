function getFunction(code, argNames) {
    var fn = window, parts = (code || "").split(".");
    while (fn && parts.length) {
        fn = fn[parts.shift()];
    }
    if (typeof (fn) === "function") {
        return fn;
    }
    argNames.push(code);
    return Function.constructor.apply(null, argNames);
}




function isString(value) {
    return typeof value === 'string' || value instanceof String;
}

function isArray(value) {
    return value && typeof value === 'object' && value.constructor === Array;
}

function isObject(value) {
    return value && typeof value === 'object' && value.constructor === Object;
}

function isNull(value) {
    return value === null;
}

function isUndefined(value) {
    return typeof value === 'undefined';
}

function isBoolean(value) {
    return typeof value === 'boolean';
}

function isDate(value) {
    return value instanceof Date;
}