# event handlers

remember - they need to be fire and forget or short, don't block the code execution because your app might get stuck.

the issue i faced
- subscribe to response received event in puppeteer sharp
- read response in event handler, read it synchronously
- execution gets blocked - i guess the response will be available for reading later
