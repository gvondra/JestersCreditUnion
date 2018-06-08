"use strict"
var request = require('request');
var rp = require('request-promise-native');

const p = {
    name: 'set-user-id',
    policy: (actionParams) => {
        var getUser = function(url, bearer) {
            var options = {
                "url": url + "api/user",
                "headers": {
                    "authorization": bearer
                  }
            }
            return rp.get(options)    
        }

        return (req, res, next) => {
          //console.log('executing set-user-id with params', actionParams);
          getUser(actionParams.address, req.headers["authorization"])
            .then(data => {
                console.log(data);
                res.send(data);
            })
            .catch(ex => res.status(500).send({ error: 'something blew up', ex: ex }));
          //next() // calling next policy
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
        $id: 'jesterscreditunion/policies/setuserid',
        address: {
            type: 'string',
        },
        required: ['address']
    } 

}