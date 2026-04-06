---
draft: false
date: 2026-04-06
categories:
  - misc
---

# Summary

## Core point

- match is strongest in expression-oriented, typed languages.
- PowerShell is shell-first, side-effect-heavy, runtime-typed.
- Result: F#-style match is a natural fit in F#, only an approximation in PowerShell.

## Why match feels better

- It maps cases to values.
- All branches do the same kind of work: produce a result.
- It removes temp state and imperative noise.

```fsharp
let color =
    match status with
    | Ok      -> "green"
    | Warning -> "yellow"
    | Error   -> "red"
```

- This is a single value-producing construct.
- No mutable `$color`, no branch-by-branch assignment.

## What “one expression” means

- An expression evaluates to a value.
- A statement tells the program to do something.
- In F#, match is an expression: the whole block returns one value.
- In PowerShell, if / elseif / else is usually used imperatively.

```powershell
if ($status -eq "Ok") {
    $color = "green"
}
elseif ($status -eq "Warning") {
    $color = "yellow"
}
else {
    $color = "red"
}
```

- Works fine.
- But this is control flow plus assignment, not one value-producing unit.

## Why side effects change the equation

- match is best when branches return values.
- It gets less compelling when branches mostly do effects:
    - Push-Location
    - Write-Output
    - Remove-Item
    - Start-Process
    - git fetch

```powershell
if ($exe.Length -eq 0) {
    Write-Output "No .exe files found..."
}
elseif ($exe.Length -eq 1) {
    Push-Location $exe[0].DirectoryName
}
else {
    $selected = ...
    if ($selected) {
        Push-Location $selected.DirectoryName
    }
}
```

- In PowerShell this is often clearer because each branch directly says what action happens.
- No translation step.
- No fake “value flow” around inherently effectful behavior.

## “Obvious with side effects”

- Means the branch reads as direct action.
- Reader sees: case -> effect.
- This is often better than wrapping effects in helper abstractions just to imitate match.

Bad abstraction pattern:

```powershell
function Write-NoExeFound { Write-Output "No .exe files found..." }
function Push-ExeDirectory($exe) { Push-Location $exe.DirectoryName }

if     ($exe.Length -eq 0) { Write-NoExeFound }
elseif ($exe.Length -eq 1) { Push-ExeDirectory $exe[0] }
```

- This can be okay.
- But it may add names and indirection without adding much clarity.

## “Ceremony faster than clarity”

- Means scaffolding grows faster than understanding.
- Typical extra ceremony:
  - extra helper functions
  - extra names to learn
  - more vertical space
  - more indirection
  - more places to hide bugs

Original simple logic:

- none found
- one found
- many found

If the refactor adds 3 helper functions to express those 3 obvious cases, it may be more architecture than value.

## Real bug pattern caused by over-abstraction

- In PowerShell, function output goes to the pipeline by default.
- If a helper writes a message, that output can accidentally become data.

Buggy shape:

```powershell
function Write-NoExeFound { Write-Output "No .exe files found..." }

$goto =
    if ($exe.Length -eq 0) { Write-NoExeFound }
    elseif ($exe.Length -eq 1) { $exe[0].DirectoryName }

if ($null -ne $goto) {
    Push-Location $goto
}
```

- Here the message becomes `$goto`.
- Then `Push-Location` tries to enter a directory literally named "No .exe files found...".
- That is exactly the kind of bug abstraction can create in PowerShell.

## Honest assessment of the Set-LocationExe refactor

- Not garbage.
- Not gold-standard.
- Small readability tweak with modest value.
- Better only if the reader likes case-style branching.
- Slightly less idiomatic than plain if / elseif / else for average PowerShell readers.
- Worth doing as a learning exercise.
- Not worth spending a lot of time polishing.

## Best lesson from that refactor

- Good lesson: move branch-specific behavior into the branch.
- Good lesson: avoid shared mutable state like $goto if the branch can own the effect directly.
- Good lesson: tests catch semantic drift from “readability” refactors.
- Weak lesson: do not force another language’s patterns into PowerShell just because they look elegant elsewhere.

## Closest native PowerShell compromise

- switch is the best native “match-like” tool in PowerShell.

```powershell
switch ($exe.Length) {
    0 {
        Write-Output "No .exe files found in the current directory or its subdirectories."
    }
    1 {
        Push-Location $exe[0].DirectoryName
    }
    default {
        $selected =
            $exe |
            Select-Object Mode, LastWriteTime, Length, DirectoryName, Name |
            Sort-Object LastWriteTime -Descending |
            Out-ConsoleGridView -Title "Where are we going?" -OutputMode Single

        if ($selected) {
            Push-Location $selected.DirectoryName
        }
    }
}
```

- More match-like.
- Still idiomatic PowerShell.
- No fake mini-language built from helper wrappers.

## Naming takeaway

- AI does not change the core rule.
- Best name = shortest name that is unambiguous in local context.
- Long descriptive names are cheaper than before because tooling helps.
- But “make every name maximally descriptive” is still bad.

Good:

- $toRemove
- $localToRemove
- $remoteToRemove

Bad:

- long names that restate obvious local context without removing ambiguity

## Bottom line

- match is great because it is a case-to-value expression.
- PowerShell is not built around that model.
- For effect-heavy scripting, plain branching is often the most honest and readable form.
- Use helpers only when they isolate meaningful logic, not when they only simulate F# aesthetics.
