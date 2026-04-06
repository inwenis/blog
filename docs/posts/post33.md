---
draft: false
date: 2026-04-06
categories:
  - PowerShell
  - F Sharp
---

# `match` feels better than `if` until PowerShell

## tl;dr

`match` in F# is usually `case -> value`.

In PowerShell it is often `case -> effect`.

That is why plain `if / elseif / else` or `switch` often reads better in PowerShell than trying to force a fake `match` style.

## In F# `match` is a value

```fsharp
let color =
    match status with
    | Ok      -> "green"
    | Warning -> "yellow"
    | Error   -> "red"
```

This whole thing evaluates to one value.

No mutable variable.
No repeated assignment.

That is why `match` feels so clean.

## In PowerShell the same thing often becomes control flow

```PowerShell
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

This works fine.

But it is not really `case -> value`.
It is branching plus assignment.

## The real issue is side effects

PowerShell code often wants to:

- `Write-Output`
- `Push-Location`
- `Remove-Item`
- `Start-Process`
- run `git fetch`

At that point the question is no longer "what value do I return?".

It is "what do I do in this case?".

```PowerShell
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

This is not elegant in the F# sense.

But it is honest.
Each branch shows the effect directly.

## The trap

When trying to make PowerShell feel more like F#, it is easy to add helpers like this:

```PowerShell
function Write-NoExeFound { Write-Output "No .exe files found..." }
function Push-ExeDirectory($exe) { Push-Location $exe.DirectoryName }

if     ($exe.Length -eq 0) { Write-NoExeFound }
elseif ($exe.Length -eq 1) { Push-ExeDirectory $exe[0] }
```

Sometimes this is fine.

Sometimes it is just:

- more names
- more jumping around
- more ceremony

PowerShell also makes this riskier than it first looks because functions write to the pipeline by default.

```PowerShell
function Write-NoExeFound { Write-Output "No .exe files found..." }

$goto =
    if ($exe.Length -eq 0) { Write-NoExeFound }
    elseif ($exe.Length -eq 1) { $exe[0].DirectoryName }

if ($null -ne $goto) {
    Push-Location $goto
}
```

In the zero-results case `$goto` becomes `No .exe files found...`.

Then `Push-Location` tries to go there.

## `switch` is the native compromise

If you want something more match-like in PowerShell, `switch` is usually the right tool.

```PowerShell
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

It keeps the case-based shape without pretending PowerShell is F#.

## small naming side note

AI does not change naming rules.

Use the shortest name that is unambiguous in local context.

Good:

- `$toRemove`
- `$localToRemove`
- `$remoteToRemove`

Bad:

- names that restate obvious context without adding clarity

## bottom line

- when code is `case -> value`, `match` is hard to beat
- when code is `case -> effect`, plain branching is often clearer
- in PowerShell, `switch` is usually a better match-like tool than helper-function gymnastics
