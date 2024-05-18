https://superuser.com/questions/620121/what-is-the-difference-between-a-certificate-and-a-key-with-respect-to-ssl

https://stackoverflow.com/questions/51363855/how-to-configure-axios-to-use-ssl-certificate

https://stackoverflow.com/questions/69502074/how-to-send-proxy-and-certs-over-a-axios-or-node-fetch-call-from-nodejs




key != cert

SSL vs TLS

ssh - secure shell protocol - do shell commands over a secure connection

client can be anonymoun in TLS - usually the case on web
TLS can be mutual - if the client has a cert the servers will/can validate it

putty is free+openn source software than can do SSH.
putty has it's own format of key files - .ppk

ppk - putty private key ( you can change the fomrat to pem wiht some software)
A PPK file stores a private key, and the corresponding public key. Both are contained in the same file.
https://tartarus.org/~simon/putty-snapshots/htmldoc/AppendixC.html


x.509?
pem
pfx

to try out - https://www.npmjs.com/package/proxy-agent