import http from 'http';
import fs from 'node:fs';

const hostname = '127.0.0.1';
const port = 3000;

const server = http.createServer((req, res) => {
  res.writeHead(200, {
      "Set-Cookie": `my_cookie=oreo`,
      "Content-Type": `text/html`,
      'Access-Control-Allow-Origin': 'null'
  });
  const x = fs.readFileSync('test.html');
  res.write(x);
  res.end();
});

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});