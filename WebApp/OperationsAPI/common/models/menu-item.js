'use strict';

module.exports = function(Menuitem) {
    Menuitem.get = function(cb) {
        cb(null, [
            [
                {Text: "Home", URL: ""},
                {Text: "Join", URL: "/join"}
            ]
        ])
    }

    Menuitem.remoteMethod("get", {http: {path: "/", verb: "get", status: 200}, returns: {type: "array", root: true}})
};
