"use strict"
var request = require('request');
var rp = require('request-promise-native');
var token = null;
const p = {
    name: 'set-adp-auth',
    policy: (actionParams) => {
        var loadToken = function(baseAdress) {
            var options = {
                "url": baseAdress + "oauth/token",
                "json": {"client_id":"rgxB7Xs04SmIpNkcjA4AnxjjcvPbaJdv","client_secret":"mNhhFYD8y6WZlxPhBVBjPu0UqK-gu-2JFvtyGxi_vYQK5g9rW8TKumqF0HWfPBA8","audience":"http://localhost/api","grant_type":"client_credentials"}
            }
            return rp.post(options)
            .then(t => {
                var d = new Date();
                d.setSeconds(t.expires_in);
                t.expiration = d;
                return t;
            });            
        }

        var setAuthorization = function(req) {
            req.headers["authorization"] = "Bearer " + token.access_token;
        }

        return (req, res, next) => {
          console.log('executing set-adp-auth with params', actionParams);
          if (!token || token.expiration < new Date()) {
            loadToken(actionParams.address)
            .then((t) => {
                token = t;
                setAuthorization(req);
                next();
            })
            .catch(ex => {
                console.log(ex);
                res.status(401).send("Unauthorized")
            })
          }
          else {
              setAuthorization(req);
              next();
          }
          // or write response:  res.json({result: "this is the response"})
        };
      }
}

module.exports = {
    version: '1.2.0',
    init: function (pluginContext) {
        pluginContext.registerPolicy(p)
    },
    schema: {
        $id: 'jesterscreditunion/policies/setadpauth',
        address: {
            type: 'string',
        },
        required: ['address']
    } 

}