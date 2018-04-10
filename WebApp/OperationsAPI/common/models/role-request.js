'use strict';

module.exports = function(Rolerequest) {
    Rolerequest.create = function(r, cb) {
        console.log(r)
        cb(null, "Ok")
    }

    Rolerequest.remoteMethod("create", {
        accepts: [{arg: "r", type: "RoleRequest", required: true, http: { source: 'body' }}],
        http: {path: "/", verb: "post", status: 200}, returns: {type: "string", root: true}
    })
};
