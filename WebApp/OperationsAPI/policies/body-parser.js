/*
I don't think the body parse can be used in conjuction with with the proxy
as they both need to read the body stream.
*/
var bodyParser = require('body-parser');
const p = {
    name: 'body-parser',
    policy: (actionParams) => {
        return bodyParser.json();
      }
}
const r = function(gatewayExpressApp) {
    
    //gatewayExpressApp.post('/api/*', bodyParser.json());
};

module.exports = {
    version: '1.2.0',
    init: function (pluginContext) {
        pluginContext.registerPolicy(p);
    }   
}