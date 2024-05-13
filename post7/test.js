const a = require('axios');
const http = require('node:http');

(async () => {
    const myCustomAgent = new http.Agent({ keepAlive: true }); // configure your agent as you need

    // use your custom agent for a specific request
    const x = await a.get('https://example.com/', { httpAgent: myCustomAgent });
    console.log(x);

    // set you agent as default for all requests
    a.default.httpAgent = myCustomAgent;
})();
