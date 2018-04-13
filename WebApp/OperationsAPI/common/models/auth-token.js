'use strict';

module.exports = function(Authtoken) {
    Authtoken.post = function(cb) {
        Authtoken.getToken("cJczpoxl0Qxdg47AODCdvsLyp4J7PwPE", "vKcYdN6REm7Trv9KaUjJAuBmlyDuLPZBhtg9qbtAQy41otFa7fLQqPbLsxE-bNMX", "http://localhost:3000/OperationsAPI")
        .then(res => cb(null, res))
        .catch(err => {
            console.log(err)
            cb(err)
        })        
    }

    Authtoken.remoteMethod("post", {
        http: {path: "/", verb: "post", status: 200}, returns: {type: "object", root: true}
    })

    Authtoken.disableRemoteMethodByName('getToken');
};
