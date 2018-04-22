const p = {
    name: 'menu',
    policy: (actionParams) => {
        return (req, res, next) => {
            var menuCount = 0;
            console.log(req.user["https://jesterscreditunion-dvlp/role-SU"]);
            var isSuperUser = (req.user["https://jesterscreditunion-dvlp/role-SU"] && req.user["https://jesterscreditunion-dvlp/role-SU"] === "SU");
            var section1 = null;
            var section2 = null;
            
            if (isSuperUser || (req.user["https://jesterscreditunion-dvlp/role-UA"] && req.user["https://jesterscreditunion-dvlp/role-UA"] === "UA")) {
                if (!section2) {
                    section2 = []
                }
                section2.push({Text: "Users", URL: "/usersearch"})
                menuCount += 1;
                section2.push({Text: "Groups", URL: "/groups"})
                menuCount += 1;
            }

            // if the user has no roles the include the role request
            if (menuCount === 0) {
                if (!section1) {
                    section1 = {}
                }
                section1.push({Text: "Request Role", URL: "/rolerequest"})
            }

            var menu = [];
            if (section1) {
                menu.push(section1);
            }
            if (section2) {
                menu.push(section2);
            }
            res.send(menu);
      }
    }
}

module.exports = {
    version: '1.2.0',
    init: function (pluginContext) {
        pluginContext.registerPolicy(p);
    }   
}