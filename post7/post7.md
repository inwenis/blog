# axios, cookies & more

## axios

axios - promise-based HTTP client for node.js
- when used in node.js axios uses http module (https://nodejs.org/api/http.html)
- in node axios __does not__ support cookies by itself (https://github.com/axios/axios/issues/5742)
  - there are npm packages that add cookies support to axios
- when used in browsers it uses XMLHttpRequest
- when used in browsers cookies work by default

---

_Why would you use `axios` over plain `http` module from node?_

> Axios makes http requests much easier. Try using plain `http` and you'll convince your self.

---

_Are there other packages like `axios`?_

> Yes - for example `node-fetch` https://github.com/node-fetch/node-fetch

---

When making a request axios creates a default `http` and `https` agent - https://axios-http.com/docs/req_config (not sure if new agents are created with each requests). You can specify custom agents for a specific request or set custom agents as default agents to use with an `axios` instance.

```js
const a = require('axios');
const http = require('node:http');

(async () => {
    // configure your agent as you need
    const myCustomAgent = new http.Agent({ keepAlive: true });

    // use your custom agent for a specific request
    const x = await a.get('https://example.com/', { httpAgent: myCustomAgent });
    console.log(x);

    // set you agent as default for all requests
    a.default.httpAgent = myCustomAgent;
})();
```

---
_What are agents responsible for?_

> `http`/`s` agents handle creating/closing sockets, TCP, etc. They talk to the OS, manage connection to hosts.
---

## cookies

Without extra packages you need to manually read response headers, look for `Set-Cookie` headers. Store the cookies somewhere. Add cookie headers to subsequent request manually.

### `http-cookie-agent`
https://www.npmjs.com/package/http-cookie-agent

Manages cookies for node.js HTTP clients (e.g. Node.js global fetch, undici, axios, node-fetch).
`http-cookie-agent` implements a `http`/`s` agent that inspects request headers and does cookie related magic for you. It uses the class `CookieJar` from package `tough-cookie` to parse&store cookies.

```js
import axios from 'axios';
import { CookieJar } from 'tough-cookie';
import { HttpCookieAgent, HttpsCookieAgent } from 'http-cookie-agent/http';

const jar = new CookieJar();

const a = axios.create({
  httpAgent: new HttpCookieAgent({ cookies: { jar } }),
  httpsAgent: new HttpsCookieAgent({ cookies: { jar } }),
});
// now we have an axios instance supporting cookies
await a.get('https://example.com');
```

### `axios-cookiejar-support`

https://www.npmjs.com/package/axios-cookiejar-support

Depends on `http-cookie-agent` and `tough-cookie`.
Does the same as `http-cookie-agent` but you don't have to create `http`/`s` agents yourself. This is a small package that just intercepts axios requests and makes sure custom `http`/`s` agents are used [source](https://github.com/3846masa/axios-cookiejar-support/blob/main/src/index.ts).

Saves you a bit of typing but you can't use your own custom agents. If you need to configure your `http`/`s` agents (ex. with a certificate) - use `http-cookie-agent` (see [github issue](https://github.com/3846masa/axios-cookiejar-support/issues/431) and [github issue](https://github.com/3846masa/axios-cookiejar-support/issues/426))


```js
import axios from 'axios';
import { wrapper } from 'axios-cookiejar-support';
import { CookieJar } from 'tough-cookie';

const jar = new CookieJar();
const client = wrapper(axios.create({ jar }));

await client.get('https://example.com');
```

### tough-cookie

https://www.npmjs.com/package/tough-cookie

npm package - cookie parsing/storage/retrieval (but `tough-cookie` itself does nothing with http request).

### A bit about cookies
https://datatracker.ietf.org/doc/html/rfc6265 - RFC describing cookies.

https://datatracker.ietf.org/doc/html/rfc6265#page-28 - concise paragraph on Third-party cookies.

Servers responds with a `Set-Cookie` header. Client can set the requested cookie. Cookies have a specific format described in this document.

## Random stuff
https://npmtrends.com/cookie-vs-cookiejar-vs-cookies-vs-tough-cookie
interesting - `cookie` for servers are more popular than `tough-cookie` for clients since ~2023.

Is this due to more serve side apps being written in node?

## Packages we don't use
- `cookie` - npm package - cookies for servers
- `cookies` - npm package - cookies for servers (different then `cookie`)
- `cookiejar` - npm package - a different cookie jar for clients

## fetch
fetch is kinda like axios - it's another http client.

You can export requests to fetch from google chrome, but you can easily modify them for axios (having the fetch request)

## chat-gpt crap
When researching I came across some chat-gpt generated content.
You read it thinking it will be something but it's trash.

https://www.dhiwise.com/post/managing-secure-cookies-via-axios-interceptors
-> this article from 2024 that tell you to implement cookies your self, doesn't even mention the word "package", "module"

https://medium.com/@stheodorejohn/managing-cookies-with-axios-simplifying-cookie-based-authentication-911e53c23c8a
-> doesn't mention that cookies don't work in axios run in node without extra packages
    (at least this one mentions that chat-gpt helped, thought I bet it's fully written by chat-gpt)

---

inco note - our http client is misleading, it uses same agent for http and https, it should maybe be called customAgent


https://superuser.com/questions/620121/what-is-the-difference-between-a-certificate-and-a-key-with-respect-to-ssl

