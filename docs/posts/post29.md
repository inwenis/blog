---
draft: false
date: 2025-03-22
categories:
  - PowerShell
---

# PowerShell quirk 2

## tl;dr

Let's say you have a `File[1].txt` file and you would like to read it.
```PowerShell
Get-Content "File[1].txt"
```
^ this returns nothing

Why?

PowerShell interprets `[]` as special characters. A range in this case. PowerShell is actually looking for a file named `File1.txt`.

What to do?

```PowerShell
Get-Content "File``[1``].txt"
```

## longer reads

- [https://stackoverflow.com/questions/21008180/copy-file-with-square-brackets-in-the-filename-and-use-wildcard](https://stackoverflow.com/questions/21008180/copy-file-with-square-brackets-in-the-filename-and-use-wildcard)
- [https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-powershell-1.0/ff730956(v=technet.10)?redirectedfrom=MSDN](https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-powershell-1.0/ff730956(v=technet.10)?redirectedfrom=MSDN)
