---
draft: false
date: 2025-02-08
categories:
  - PowerShell
---

# PowerShell Gotcha! - dynamic scoping

PowerShell uses dynamic scoping. Yet the [about_Scopes](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_scopes?view=powershell-7.5) page doesn't mention the word "dynamic".

Wird (wird - so weird that you need to misspell weird to get your point across).

---
```PowerShell
function Do-InnerFunction  { Write-Host $t }
function Do-OutterFunction {
    $t = "hello"
    Do-InnerFunction
}

Do-OutterFunction
```
```
hello
```

Weird!

---

```PowerShell
Set-StrictMode -Version Latest
function Do-InnerFunction  { Write-Host $t }
function Do-OutterFunction {
    $t = "hello"
    Do-InnerFunction
}

Do-OutterFunction
Set-StrictMode -Off # remember to turn strict mode off for further testing
```

```
hello
```

Weird! (but makes sense since in PowerShell's world this is perfectly legal hence "strict" changes nothing here)

---

```PowerShell
function Do-InnerFunction  { Write-Host $t }
function Do-OutterFunction {
    $private:t = "hello"
    Do-InnerFunction
}

Do-OutterFunction
```

```
```

Output is empty. No errors but at least `$t` behaves more like a variable we know from C#/F#.

---

```PowerShell
Set-StrictMode -Version Latest
function Do-InnerFunction  { Write-Host $t }
function Do-OutterFunction {
    $private:t = "hello"
    Do-InnerFunction
}

Do-OutterFunction
```

```
InvalidOperation: C:\Users\...\Temp\44f5ff41-4105-482b-a134-b505049d2c61\test3.ps1:2
Line |
   2 |      Write-Host $t
     |                 ~~
     | The variable '$t' cannot be retrieved because it has not been set.
```

Finally!

---

```PowerShell
function Do-InnerFunction {
    Write-Host $t
    $t = "world"
    Write-Host $t
}

function Do-OutterFunction {
    $t = "hello"
    Do-InnerFunction
    Write-Host $t
}

Do-OutterFunction
```

```
hello
world
hello
```

Ah! So variables are copied to the next "scope".

---

```PowerShell
function Do-InnerFunction {
    Write-Host $t
    $global:t = "world"
    Write-Host $t
}

function Do-OutterFunction {
    $t = "hello"
    Do-InnerFunction
    Write-Host $t
}

Do-OutterFunction
Write-Host $t
```

```
hello
hello
hello
world
```

Now we have created a `global` `$t` variable.

This [https://ig2600.blogspot.com/2010/01/powershell-is-dynamically-scoped-and.html](https://ig2600.blogspot.com/2010/01/powershell-is-dynamically-scoped-and.html) explains it nicely.
