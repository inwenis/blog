import http from 'http';
import fs from 'node:fs';

const hostname = '127.0.0.1';
const port = 3000;

const server = http.createServer((req, res) => {
    console.log(req.url);
    console.log(req.headers);
    if (req.url === '/') {
        res.writeHead(200, {
            "Content-Type": `text/html`,
        });
        const x = fs.readFileSync('test.html');
        res.write(x);
        res.end();
    }
    if (req.url === '/with-cookie') {
        res.writeHead(200, {
            "Set-Cookie": `my_cookie=oreo`,
            "Content-Type": `text/html`,
        });
        res.end();
    }
});

server.listen(port, hostname, () => {
    console.log(`Server running at http://${hostname}:${port}/`);
});