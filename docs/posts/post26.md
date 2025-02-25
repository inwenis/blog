---
draft: false
date: 2025-02-25
categories:
  - npm
---

# npm wsl EAI_AGAIN

Running `npm ci` on `WSL` (Windows Subsystem for Linux) failed with:

```
npm ERR! syscall getaddrinfo
npm ERR! errno EAI_AGAIN
npm ERR! request to http://registry.npmjs.org/nodemon failed, reason: getaddrinfo EAI_AGAIN registry.npmjs.org
```

```
dig registry.npmjs.org
```
^ confirmed that my Ubuntu `WSL` can't reach `registry.npmjs.org`

## Solution

```
rm /etc/resolv.conf
bash -c 'echo "nameserver 8.8.8.8" > /etc/resolv.conf'
bash -c 'echo "[network]" > /etc/wsl.conf'
bash -c 'echo "generateResolvConf = false" >> /etc/wsl.conf'
chattr +i /etc/resolv.conf
```
^ use `8.8.8.8` (Google's DNS). Stop recreating `resolv.conf` on startup
