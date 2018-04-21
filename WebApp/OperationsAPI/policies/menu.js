const p = {
    name: 'menu',
    policy: (actionParams) => {
        return (req, res, next) => {
            res.send([
                [
                    {Text: "Request Role", URL: "/rolerequest"}
                ]
            ]);
      }
    }
}

module.exports = {
    version: '1.2.0',
    init: function (pluginContext) {
        pluginContext.registerPolicy(p);
    }   
}