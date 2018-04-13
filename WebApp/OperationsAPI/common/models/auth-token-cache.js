'use strict';

module.exports = function(Authtokencache) {
    function getAuthToken(app, clientId) {
        return app.models.AuthToken.post(clientId, "vKcYdN6REm7Trv9KaUjJAuBmlyDuLPZBhtg9qbtAQy41otFa7fLQqPbLsxE-bNMX", "http://localhost:3000/OperationsAPI")        
    }

    function cacheToken(c, t) {
        var d = new Date();
        d.setSeconds(t.expires_in - 1);
        c.access_token = t.access_token;
        c.token_type = t.token_type;
        c.expiration = d; 
        return Authtokencache.replaceOrCreate(c);
    }

    Authtokencache.getToken = function(clientId, cb) {
        var c = null;
        Authtokencache.find({"where": {"client_id": clientId}})
        .then(res => {            
            if (!res || res.length === 0) {
                c = new Authtokencache();
            }
            else {
                c = res[0];
            }
            if (!res || res.length === 0 || c.expiration <= Date.now()) {
                c.client_id = clientId;
                getAuthToken(Authtokencache.app, clientId)
                .then(t => {
                    cacheToken(c, t).then(cb(null, c))
                })
            }
            else {
                cb(null, c)
            }
        })
        .catch(ex => console.log(ex))
    }

    /*Authtokencache.remoteMethod("getToken", {
        accepts: [{arg: "clientId", type: "string", required: true, http: { source: 'query' }}],
        http: {path: "/", verb: "get", status: 200}, returns: {type: "object", root: true}
    })*/
};
