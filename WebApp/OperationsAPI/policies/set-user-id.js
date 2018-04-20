const p = {
    name: 'set-user-id',
    policy: (actionParams) => {
        return (req, res, next) => {
          //console.log('executing set-user-id with params', actionParams);
          console.log(req.body);
          next() // calling next policy
          // or write response:  res.json({result: "this is the response"})
        };
      }
}
module.exports = {
    version: '1.2.0',
    init: function (pluginContext) {
        pluginContext.registerPolicy(p)
    }   

}