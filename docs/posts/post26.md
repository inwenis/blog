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

In WSL:
```
rm /etc/resolv.conf
bash -c 'echo "nameserver 8.8.8.8" > /etc/resolv.conf'
bash -c 'echo "[network]" > /etc/wsl.conf'
bash -c 'echo "generateResolvConf = false" >> /etc/wsl.conf'
chattr +i /etc/resolv.conf
```
^ use `8.8.8.8` (Google's DNS). Stop recreating `resolv.conf` on startup

In Windows:
```
Set-Content -Path "C:\Users\{user}\.wslconfig" -Value "[wsl2]`nnetworkingMode=mirrored"
```

Works now, also on my company's VPN.

kudos [https://github.com/microsoft/WSL/issues/5420#issuecomment-646479747](https://github.com/microsoft/WSL/issues/5420#issuecomment-646479747)

more info [https://gist.github.com/machuu/7663aa653828d81efbc2aaad6e3b1431](https://gist.github.com/machuu/7663aa653828d81efbc2aaad6e3b1431)
