https://superuser.com/questions/620121/what-is-the-difference-between-a-certificate-and-a-key-with-respect-to-ssl

https://stackoverflow.com/questions/51363855/how-to-configure-axios-to-use-ssl-certificate

https://stackoverflow.com/questions/69502074/how-to-send-proxy-and-certs-over-a-axios-or-node-fetch-call-from-nodejs




key != cert

cert
a certificate proves that a public key belongs to a given entity.
The cert includes the public key and info about it. and CA's signature validating the cert.
So the CA signes saying "I prove that this public key belongs to this person/entity"

SSL vs TLS

SSH (Secure SHell protocol) - protocol that allows to execute shell commands over a secure connection
SFTP is ssh FTP.
it is an extension of SSH.
to connect to a sftp you need a private ssh key. The public ssh key (you'r provaites key counterpart) is stored at the server.


client can be anonymous in TLS - usually the case on web
TLS can be mutual - if the client has a cert the servers will/can validate it

putty is free+open source software than can do SSH.
putty has its own format of key files -> .ppk

ppk - putty private key ( ppk can be changed to pem with some software)
A PPK file stores a private key, and the corresponding public key. Both are contained in the same file.
https://tartarus.org/~simon/putty-snapshots/htmldoc/AppendixC.html

x.509?
pem
pfx

to try out - https://www.npmjs.com/package/proxy-agent



https://www.glezen.org/Base64Decoder.html


convention - propose
    - specify format in secret name
    - use plain - not base64 encoded