


axios - promise-based HTTP client for node.js
    - in node it uses http module
        -> no cookies in node by default
    - in browsers it uses XMLHttpRequest
        -> cookies work by default
https://github.com/axios/axios/issues/5742
    node issue saying that it doesn't have cookies by default


for every request if agents are not specified they're creted by axios
    https://axios-http.com/docs/req_config (not sure if it's created each time or re-used, doesn't matter much) (our http client is bit mislaeding, it uses same agent for http and https, it should maybe be called customAgent)


https://www.npmjs.com/package/http-cookie-agent
Allows cookies with every Node.js HTTP clients (e.g. Node.js global fetch, undici, axios, node-fetch).
use CookieJar from tough-cookie to store cookie
the module itself wraps around http agent and reads/writes cookies to CookieJar

```js
import axios from 'axios';
import { CookieJar } from 'tough-cookie';
import { HttpCookieAgent, HttpsCookieAgent } from 'http-cookie-agent/http';

const jar = new CookieJar();

const client = axios.create({
  httpAgent: new HttpCookieAgent({ cookies: { jar } }),
  httpsAgent: new HttpsCookieAgent({ cookies: { jar } }),
});

await client.get('https://example.com');
```

axios-cookiejar-support
https://github.com/3846masa/axios-cookiejar-support/tree/main
Add tough-cookie support to axios.
depends on http-cookie-agent
it probably just wraps around axios to replace the http agents with http-cookie-agent with cookie jars
yep - https://github.com/3846masa/axios-cookiejar-support/blob/main/src/index.ts

```js
import axios from 'axios';
import { wrapper } from 'axios-cookiejar-support';
import { CookieJar } from 'tough-cookie';

const jar = new CookieJar();
const client = wrapper(axios.create({ jar }));

await client.get('https://example.com');
```

if you need to use a custom agent you would need to use the http-cookie-agent and configure it with your stuff
https://github.com/3846masa/axios-cookiejar-support/issues/431
https://github.com/3846masa/axios-cookiejar-support/issues/426
    -> if you're using http-cookie-agent, just don't use axios-cookiejar-support

https://github.com/3846masa/axios-cookiejar-support/blob/main/MIGRATION.md






https://datatracker.ietf.org/doc/html/rfc6265
doc describing cookies
server respons with Set-Cookie header
    cookies have a specific format defined in this doc
https://datatracker.ietf.org/doc/html/rfc6265#section-7.1
    very concise description on 3rd party cookies



axios - module using http (in node) to make http request
http-cookie-agent - module wraps around http, uses tough cookie to make a cookie enabled http agent
tough-cookie - cookie parsing/storage/retrieval (but nothing with http)

https://npmtrends.com/cookie-vs-cookiejar-vs-cookies-vs-tough-cookie
    interesting cookie for !servers! are more popular than tough-cookie for client since ~2023

stuff we don't use
cookie - cookie module for !servers!
cookiejar - just a different cookie jar for clients
https://www.npmjs.com/package/cookies - another lib for server cookies

fetch is kinda like axios - it's another http client lib.
you can export requsts to fetch from google chrome, but you can easily modify them for axios (having the fetch request)


when reaserching i came across I think 2 chat-gpt articles, and they're crap.
you read them thinking it will be sth but it's trash
https://www.dhiwise.com/post/managing-secure-cookies-via-axios-interceptors
    -> like this article from 2024 that tell you to implement everything your self xD

https://medium.com/@stheodorejohn/managing-cookies-with-axios-simplifying-cookie-based-authentication-911e53c23c8a
    -> or this doesn't mention that cookies don't work in node without extra packages
    (at least this one mentions i's chat-gpt helped)
