---
draft: false
date: 2024-05-18
categories:
  - certificates
  - keys
  - SSL
  - HTTPS
---

# Notes on keys, certs, certificates, HTTPS, SSL, SSH, TLS

key != cert (a key is different from a certificate)

Keys are used to encrypt connections, certs are used to verify that the key owner is who he says he is.

## Certificate aka cert
A certificate proves that a public key belongs to a given entity.
The cert includes:

- public key
- information about the key
- CA's signature validating the cert

CA's signature basically says "I confirm that this public key belongs to this person/entity". The signature is made using CA's private key.

This is wikipedia certificate which I've exported from my chrome browser. This is the certificate in PEM format.

```
-----BEGIN CERTIFICATE-----
MIIISzCCB9GgAwIBAgIQB0GeOVg6THbPHqFDR/pfOjAKBggqhkjOPQQDAzBWMQsw
CQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMTAwLgYDVQQDEydEaWdp
Q2VydCBUTFMgSHlicmlkIEVDQyBTSEEzODQgMjAyMCBDQTEwHhcNMjMxMDE4MDAw
MDAwWhcNMjQxMDE2MjM1OTU5WjB5MQswCQYDVQQGEwJVUzETMBEGA1UECBMKQ2Fs
aWZvcm5pYTEWMBQGA1UEBxMNU2FuIEZyYW5jaXNjbzEjMCEGA1UEChMaV2lraW1l
ZGlhIEZvdW5kYXRpb24sIEluYy4xGDAWBgNVBAMMDyoud2lraXBlZGlhLm9yZzBZ
MBMGByqGSM49AgEGCCqGSM49AwEHA0IABDVh9CEa/2rEO/oGR8YZbr5wOPHcFrG8
OBQS1BQrHAsxgVn1Z/bnKtE8Hvqup+0GXdZvXYlMa8iw4A+Dz/XTitqjggZcMIIG
WDAfBgNVHSMEGDAWgBQKvAgpF4ylOW16Ds4zxy6z7fvDejAdBgNVHQ4EFgQUyqwM
Z6LjhkM/u0PnQdmhhzp43TMwggLtBgNVHREEggLkMIIC4IIPKi53aWtpcGVkaWEu
b3Jngg13aWtpbWVkaWEub3Jngg1tZWRpYXdpa2kub3Jngg13aWtpYm9va3Mub3Jn
ggx3aWtpZGF0YS5vcmeCDHdpa2luZXdzLm9yZ4INd2lraXF1b3RlLm9yZ4IOd2lr
aXNvdXJjZS5vcmeCD3dpa2l2ZXJzaXR5Lm9yZ4IOd2lraXZveWFnZS5vcmeCDndp
a3Rpb25hcnkub3Jnghd3aWtpbWVkaWFmb3VuZGF0aW9uLm9yZ4IGdy53aWtpghJ3
bWZ1c2VyY29udGVudC5vcmeCESoubS53aWtpcGVkaWEub3Jngg8qLndpa2ltZWRp
YS5vcmeCESoubS53aWtpbWVkaWEub3JnghYqLnBsYW5ldC53aWtpbWVkaWEub3Jn
gg8qLm1lZGlhd2lraS5vcmeCESoubS5tZWRpYXdpa2kub3Jngg8qLndpa2lib29r
cy5vcmeCESoubS53aWtpYm9va3Mub3Jngg4qLndpa2lkYXRhLm9yZ4IQKi5tLndp
a2lkYXRhLm9yZ4IOKi53aWtpbmV3cy5vcmeCECoubS53aWtpbmV3cy5vcmeCDyou
d2lraXF1b3RlLm9yZ4IRKi5tLndpa2lxdW90ZS5vcmeCECoud2lraXNvdXJjZS5v
cmeCEioubS53aWtpc291cmNlLm9yZ4IRKi53aWtpdmVyc2l0eS5vcmeCEyoubS53
aWtpdmVyc2l0eS5vcmeCECoud2lraXZveWFnZS5vcmeCEioubS53aWtpdm95YWdl
Lm9yZ4IQKi53aWt0aW9uYXJ5Lm9yZ4ISKi5tLndpa3Rpb25hcnkub3JnghkqLndp
a2ltZWRpYWZvdW5kYXRpb24ub3JnghQqLndtZnVzZXJjb250ZW50Lm9yZ4INd2lr
aXBlZGlhLm9yZ4IRd2lraWZ1bmN0aW9ucy5vcmeCEyoud2lraWZ1bmN0aW9ucy5v
cmcwPgYDVR0gBDcwNTAzBgZngQwBAgIwKTAnBggrBgEFBQcCARYbaHR0cDovL3d3
dy5kaWdpY2VydC5jb20vQ1BTMA4GA1UdDwEB/wQEAwIDiDAdBgNVHSUEFjAUBggr
BgEFBQcDAQYIKwYBBQUHAwIwgZsGA1UdHwSBkzCBkDBGoESgQoZAaHR0cDovL2Ny
bDMuZGlnaWNlcnQuY29tL0RpZ2lDZXJ0VExTSHlicmlkRUNDU0hBMzg0MjAyMENB
MS0xLmNybDBGoESgQoZAaHR0cDovL2NybDQuZGlnaWNlcnQuY29tL0RpZ2lDZXJ0
VExTSHlicmlkRUNDU0hBMzg0MjAyMENBMS0xLmNybDCBhQYIKwYBBQUHAQEEeTB3
MCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC5kaWdpY2VydC5jb20wTwYIKwYBBQUH
MAKGQ2h0dHA6Ly9jYWNlcnRzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydFRMU0h5YnJp
ZEVDQ1NIQTM4NDIwMjBDQTEtMS5jcnQwDAYDVR0TAQH/BAIwADCCAYAGCisGAQQB
1nkCBAIEggFwBIIBbAFqAHcA7s3QZNXbGs7FXLedtM0TojKHRny87N7DUUhZRnEf
tZsAAAGLQ1J6cgAABAMASDBGAiEA5reeeuLSzGPvJQ5hT3Bd8aOVxmIltXMTLhY6
19qDGWUCIQDO0LMbF3s42tyxgFIOt7rVOpsHe9Sy0wFQQj8BWO0LIQB2AEiw42va
pkc0D+VqAvqdMOscUgHLVt0sgdm7v6s52IRzAAABi0NSegEAAAQDAEcwRQIgYJdu
BrioIun6FTeQhDxqK2eyZehguOkxScS3nwsGSakCIQC1FyuCpm+QQBRJFSTAnStR
iP+hgGIhgzyZ837usahB0QB3ANq2v2s/tbYin5vCu1xr6HCRcWy7UYSFNL2kPTBI
1/urAAABi0NSeg8AAAQDAEgwRgIhAOm1GvY8M4V+tUyjV9/PCj8rcWHUOvfY0a/o
nsKg/bitAiEA1Vm1pP8CDp7hGcQzBBTscpCVebzWCe8DK231mtv97QUwCgYIKoZI
zj0EAwMDaAAwZQIwKuOOLjmwGgtjG6SASF4W2e8KtQZANRsYXMXJDGwBCi9fM7Qy
S9dvlFLwrcDg1gxlAjEA5XwJikbpk/qyQerzeUspuZKhqh1KPuj2uBdp8vicuBxu
TJUd1W+d3LmikOUgGzil
-----END CERTIFICATE-----

```

We can double click the cert file to view it (on Windows) or use many other different tools to view its content.

https://stackoverflow.com/questions/9758238/how-to-view-the-contents-of-a-pem-certificate

```PowerShell
> $cert = New-Object Security.Cryptography.X509Certificates.X509Certificate2([string]"C:\Users\inwen\Downloads\_.wikipedia.org.crt")
> $cert | select *

EnhancedKeyUsageList : {Server Authentication (1.3.6.1.5.5.7.3.1), Client Authentication (1.3.6.1.5.5.7.3.2)}
DnsNameList          : {*.wikipedia.org, wikimedia.org, mediawiki.org, wikibooks.org...}
SendAsTrustedIssuer  : False
Archived             : False
Extensions           : {System.Security.Cryptography.Oid, System.Security.Cryptography.Oid, System.Security.Cryptography.Oid, System.Security.Cryptography.Oid...}
FriendlyName         :
IssuerName           : System.Security.Cryptography.X509Certificates.X500DistinguishedName
NotAfter             : 17/10/2024 01:59:59
NotBefore            : 18/10/2023 02:00:00
HasPrivateKey        : False
PrivateKey           :
PublicKey            : System.Security.Cryptography.X509Certificates.PublicKey
RawData              : {48, 130, 8, 75...}
SerialNumber         : 07419E39583A4C76CF1EA14347FA5F3A
SubjectName          : System.Security.Cryptography.X509Certificates.X500DistinguishedName
SignatureAlgorithm   : System.Security.Cryptography.Oid
Thumbprint           : 483F0C71F34AE0EA30D99BD60463DCDAA8F49DFB
Version              : 3
Handle               : 2140299849504
Issuer               : CN=DigiCert TLS Hybrid ECC SHA384 2020 CA1, O=DigiCert Inc, C=US
Subject              : CN=*.wikipedia.org, O="Wikimedia Foundation, Inc.", L=San Francisco, S=California, C=US
```

## SSL & TLS & SSH & SFTP

SSH (Secure SHell protocol) - protocol that allows to execute shell commands over a secure connection.

SFTP is an extension of SSH. SFTP != FTP over SSH.
To connect to a SFTP server you need a private ssh key. The public ssh key (your private key's counterpart) is stored at the server.

TLS & SSL - think of SSL as the older/first protocol for secure communication. SSL was outphased by TLS. TLS is THE protocol used by HTTPS for secure connections.

Clients can be anonymous in TLS - usually the case on web - the server provides a cert to your browser but you don't need a cert of your own.
TLS can be mutual - if the client has a cert the servers will/can validate it.

## PuTTy
PuTTy is free+open source software than can do SSH.
PuTTy has its own format of key files -> .ppk

ppk - putty private key (ppk can be changed to pem with some software)
A PPK file stores a private key, and the corresponding public key. Both are contained in the same file.
https://tartarus.org/~simon/putty-snapshots/htmldoc/AppendixC.html

# PEM
Privacy-Enhanced Mail (PEM) is THE file format for exhanging keys, certificates.

- `.cer` & `.crt` - PEM file with a certificate
- `.key` - PEM with with a private or public key

The file extensions doesn't really matter. Just open the file and see the headers to be sure what it is.

To view a pem certificate on Windows - rename it to `.crt` and double click.

You can open a .pem file as plain text as see its content:
```
// pem ignores stuff between the headers so you can put comments here

-----BEGIN RSA PRIVATE KEY-----
izfrNTmQLnfsLzi2Wb9xPz2Qj9fQYGgeug3N2MkDuVHwpPcgkhHkJgCQuuvT+qZI
MbS2U6wTS24SZk5RunJIUkitRKeWWMS28SLGfkDs1bBYlSPa5smAd3/q1OePi4ae
dU6YgWuDxzBAKEKVSUu6pA2HOdyQ9N4F1dI+F8w9J990zE93EgyNqZFBBa2L70h4
M7DrB0gJBWMdUMoxGnun5glLiCMo2JrHZ9RkMiallS1sHMhELx2UAlP8I1+0Mav8
iMlHGyUW8EJy0paVf09MPpceEcVwDBeX0+G4UQlO551GTFtOSRjcD8U+GkCzka9W
/SFQrSGe3Gh3SDaOw/4JEMAjWPDLiCglwh0rLIO4VwU6AxzTCuCw3d1ZxQsU6VFQ
PqHA8haOUATZIrp3886PBThVqALBk9p1Nqn51bXLh13Zy9DZIVx4Z5Ioz/EGuzgR
d68VW5wybLjYE2r6Q9nHpitSZ4ZderwjIZRes67HdxYFw8unm4Wo6kuGnb5jSSag
vwBxKzAf3Omn+J6IthTJKuDd13rKZGMcRpQQ6VstwihYt1TahQ/qfJUWPjPcU5ML
9LkgVwA8Ndi1wp1/sEPe+UlL16L6vO9jUHcueWN7+zSUOE/cDSJyMd9x/ZL8QASA
ETd5dujVIqlINL2vJKr1o4T+i0RsnpfFiqFmBKlFqww/SKzJeChdyEtpa/dJMrt2
8S86b6zEmkser+SDYgGketS2DZ4hB+vh2ujSXmS8Gkwrn+BfHMzkbtio8lWbGw0l
eM1tfdFZ6wMTLkxRhBkBK4JiMiUMvpERyPib6a2L6iXTfH+3RUDS6A==
-----END RSA PRIVATE KEY-----
-----BEGIN CERTIFICATE-----
MIICMzCCAZygAwIBAgIJALiPnVsvq8dsMA0GCSqGSIb3DQEBBQUAMFMxCzAJBgNV
BAYTAlVTMQwwCgYDVQQIEwNmb28xDDAKBgNVBAcTA2ZvbzEMMAoGA1UEChMDZm9v
MQwwCgYDVQQLEwNmb28xDDAKBgNVBAMTA2ZvbzAeFw0xMzAzMTkxNTQwMTlaFw0x
ODAzMTgxNTQwMTlaMFMxCzAJBgNVBAYTAlVTMQwwCgYDVQQIEwNmb28xDDAKBgNV
BAcTA2ZvbzEMMAoGA1UEChMDZm9vMQwwCgYDVQQLEwNmb28xDDAKBgNVBAMTA2Zv
bzCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEAzdGfxi9CNbMf1UUcvDQh7MYB
OveIHyc0E0KIbhjK5FkCBU4CiZrbfHagaW7ZEcN0tt3EvpbOMxxc/ZQU2WN/s/wP
xph0pSfsfFsTKM4RhTWD2v4fgk+xZiKd1p0+L4hTtpwnEw0uXRVd0ki6muwV5y/P
+5FHUeldq+pgTcgzuK8CAwEAAaMPMA0wCwYDVR0PBAQDAgLkMA0GCSqGSIb3DQEB
BQUAA4GBAJiDAAtY0mQQeuxWdzLRzXmjvdSuL9GoyT3BF/jSnpxz5/58dba8pWen
v3pj4P3w5DoOso0rzkZy2jEsEitlVM2mLSbQpMM+MUVQCQoiG6W9xuCFuxSrwPIS
pAqEAuV4DNoxQKKWmhVv+J0ptMWD25Pnpxeq5sXzghfJnslJlQND
-----END CERTIFICATE-----
```

Contents between header and footer (`-----BEGIN CERTIFICATE-----` + `-----END CERTIFICATE-----`) is base64 encoded. The content can be DER binary data.

# DER
Distinguished Encoding Rules - is a way of encoding data structures. A certificate is a data structure containing various entires like `validity date`, `issuer`, etc. For certificates to work you need to store this information and transfer it. DER encodes this information is a binary format. This is then after base64 encoded and then it goes into a PEM file.

# X.509
X.509 is the standard defining public key certificates for TLS/SSL (HTTPS)


# PFX/P12/PKCS12
PFX seems to be Microsoft's complicated file format for storing cryptographic data.

P12/PKCS12 is the successor to PFX. Sometimes the terms PFX/P12/PKCS12 are used interchangeably.


---
base64 offline decoder:
https://www.glezen.org/Base64Decoder.html

Nice description of certs vs key:
https://superuser.com/questions/620121/what-is-the-difference-between-a-certificate-and-a-key-with-respect-to-ssl

Generate yourself a certificate:
https://getacert.com/index.html

Important info on `rejectUnauthorized: false` and certificates in `axios`/`node`:
https://stackoverflow.com/questions/51363855/how-to-configure-axios-to-use-ssl-certificate

convention - propose
    - specify format in secret name
    - use plain - not base64 encoded

# Lazy websites

Website's certificates are usually signed by intermediate CA, which in turn are signed by a trusted root CA. The idea is that the server you connect to send you its certificate with all the intermediate certificates. Your app/machine should have the root CA certificate stored so it can validate the chain of certificates it received from the server (by just validating the root cert sent with its own root CA).

Some servers are misconfigured and do not send the intermediate certificates. You do not notice because browsers fill in the gaps for a better browsing experience. However when you try to scrape the same website with ex. node your connection will be rejected.

## don't's (for node)
Several answers on SO suggest:

- `NODE_TLS_REJECT_UNAUTHORIZED=0` or 
- `const httpsAgent = new https.Agent({ rejectUnauthorized: false });`

Both are terrible ideas - they make your app accept unauthorized connections. They are the equivalent of this conversation:

"I can't verify this certificate, we can not be sure who we are connecting to" - says Node with care in its voice

"Doesn't matter, YOLO, carry on" - you reply shrugging your shoulders

Read more [here](https://stackoverflow.com/a/53585725/2377787)

## does (for node)
Use [NODE_EXTRA_CA_CERTS](https://nodejs.org/api/cli.html#node_extra_ca_certsfile). Alternatively use a library to programmatically give node the missing certificate [link](https://stackoverflow.com/a/39972054/2377787)

Good read - https://stackoverflow.com/questions/31673587/error-unable-to-verify-the-first-certificate-in-nodejs

# root CA stores
## Node

It seems everyone has their own root CA store these days. Nodes has a hardcoded list of root CA see:

- https://github.com/nodejs/node/blob/main/src/node_root_certs.h
- https://github.com/nodejs/node/issues/4175

## Windows
You can view Windows certificates with PowerShell:
```PowerShell
Get-ChildItem -Recurse Cert:
```

## Chrome
https://chromium.googlesource.com/chromium/src/+/main/net/data/ssl/chrome_root_store/root_store.md

If you would like to become chrome's trusted CA - https://www.chromium.org/Home/chromium-security/root-ca-policy/

https://blog.chromium.org/2022/09/announcing-launch-of-chrome-root-program.html
