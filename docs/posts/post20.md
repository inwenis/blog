---
draft: false
date: 2025-01-20
categories:
  - PowerShell
---

# Environment variable
## but only in a specific directory

The idea - use the `Prompt` function to check if you're in a specific dir and set/unset an env var:

```PowerShell
function Prompt {
    $currentDir = Get-Location
    if ("C:\git\that-special-dir" -eq $currentDir) {
        $env:THAT_SPECIAL_ENV_VAR = "./extra.cer"
    }
    else {
        Remove-Item Env:\THAT_SPECIAL_ENV_VAR
    }
}
```

Extract the special env setting/unsetting to a function:

```PowerShell
function SetOrUnSet-DirectoryDependent-EnvironmentVariables {
    $currentDir = Get-Location
    if ("C:\git\that-special-dir" -eq $currentDir) {
        $env:THAT_SPECIAL_ENV_VAR = "./extra.cer"
    }
    else {
        Remove-Item Env:\THAT_SPECIAL_ENV_VAR
    }
}

function Prompt {
    SetOrUnSet-DirectoryDependent-EnvironmentVariables
}
```

If your `Prompt` function is already overwritten by ex. `oh-my-posh`:

```PowerShell
function SetOrUnSet-DirectoryDependent-EnvironmentVariables {
    $currentDir = Get-Location
    if ("C:\git\that-special-dir" -eq $currentDir) {
        $env:THAT_SPECIAL_ENV_VAR = "./extra.cer"
    }
    else {
        Remove-Item Env:\THAT_SPECIAL_ENV_VAR
    }
}

$promptFunction = (Get-Command Prompt).ScriptBlock

function Prompt {
    SetOrUnSet-DirectoryDependent-EnvironmentVariables
    $promptFunction.Invoke()
}
```

## Why did I need this?

In a repository with several js scrapers run by `NODE` a few scrape data from misconfigured websites. These websites don't provide the intermediate certificate for https. Your browser automatically fills in the gap for convenience but a simple http client like `axios` will rightfully reject the connection as it can't verify who it is talking to (see more [here](post09.md#lazy-websites))

Solution?
> Use [NODE_EXTRA_CA_CERTS](https://nodejs.org/api/cli.html#node_extra_ca_certsfile)

- You configure your production server with `NODE_EXTRA_CA_CERTS`.
- When testing locally you get tired of remembering to set `NODE_EXTRA_CA_CERTS`.
- You add `NODE_EXTRA_CA_CERTS` to your powershell profile. Now every time you run anything using `NODE` (like vs code) you see
```
Warning: Ignoring extra certs from `./extra.cer`, load failed: error:02000002:system library:OPENSSL_internal:No such file or directory
```
- You get annoyed and you ask yourself how to set an
[environment variable but only in a specific directory](post20.md)

I use this myself here -> [the public part of my powershell-profile](https://github.com/inwenis/powershell-profile/blob/master/Profile.ps1#L194)
