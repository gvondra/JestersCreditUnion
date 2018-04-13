'use strict';

module.exports = function(Authtoken) {
    Authtoken.post = function(clientId, clientSecret, audience) {
        return Authtoken.getToken(clientId, clientSecret, audience)        
    }

};
