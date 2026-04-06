---
draft: false
date: 2026-04-06
categories:
  - misc
---

# Why `match` Feels Better Than `if` Until You Hit PowerShell

`match` is not just nicer syntax. It is better because it belongs to a different programming model.

In F#, `match` is built for typed, expression-oriented code. In PowerShell, you are usually writing shell code: runtime-typed, side-effect-heavy, command-first. That is why `match` feels
elegant in F# and why `if / elseif / else` often remains the clearest tool in PowerShell.

## `match` works because it maps cases to values

A good `match` says: given this case, produce this value.

```fsharp
let color =
    match status with
    | Ok      -> "green"
    | Warning -> "yellow"
    | Error   -> "red"

This is one value-producing construct. No mutable variable. No repeated assignment. No control-flow noise. Just input shape to output value.

That is what people mean by "one expression": the whole block evaluates to a value.

## if / elseif / else in PowerShell is usually a statement, not an expression

In PowerShell, the same logic often looks like this:

if ($status -eq "Ok") {
    $color = "green"
}
elseif ($status -eq "Warning") {
    $color = "yellow"
}
else {
    $color = "red"
}

This works. But it is not the same thing.

This is not "a case-to-value mapping". It is imperative control flow plus assignment. That is why it feels heavier. You are managing a procedure, not describing a result.

## The real break point is side effects

match is strongest when every branch returns a value. It gets weaker when branches mostly do things.

PowerShell code often does things:

- Push-Location
- Write-Output
- Remove-Item
- Start-Process
- git fetch

That changes what "readable" means.

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

This is not elegant in the F# sense. But it is direct. Each branch says exactly what effect happens.

That matters. When code is effect-heavy, the most readable form is often the one that exposes the effect immediately.

## “Obvious with side effects” means case -> action

In a shell script, branches often exist to perform different actions. In that setting, if / elseif / else has a real advantage: it is blunt and honest.

The reader sees:

- if this case, write a message
- if that case, change directory
- otherwise, show a picker and maybe change directory

There is no fake value flow around inherently imperative behavior.

That is why people can reasonably prefer if / elseif / else in PowerShell even if they love match elsewhere.

## The trap: simulating match with helper functions

If you try to make PowerShell feel more like F#, the first move is usually to extract tiny branch helpers.

function Write-NoExeFound { Write-Output "No .exe files found..." }
function Push-ExeDirectory($exe) { Push-Location $exe.DirectoryName }

if     ($exe.Length -eq 0) { Write-NoExeFound }
elseif ($exe.Length -eq 1) { Push-ExeDirectory $exe[0] }

This is not automatically bad. But it is easy to add ceremony faster than clarity.

You add:

- extra function definitions
- extra names to learn
- more vertical space
- more indirection
- more places to hide semantic mistakes

If the original logic was only three obvious cases, the abstraction tax can outweigh the readability gain.

## PowerShell makes over-abstraction riskier than it looks

PowerShell functions write to the pipeline by default. That means a helper that "just prints something" can accidentally become data.

This is a real failure mode:

function Write-NoExeFound { Write-Output "No .exe files found..." }

$goto =
    if ($exe.Length -eq 0) { Write-NoExeFound }
    elseif ($exe.Length -eq 1) { $exe[0].DirectoryName }

if ($null -ne $goto) {
    Push-Location $goto
}

Looks harmless. It is not.

In the zero-results case, the message becomes $goto. Then Push-Location tries to enter a directory literally named:

No .exe files found...

That bug is a good example of the cost of abstraction in PowerShell. The language will happily treat output as data unless you are extremely explicit.

## So is this style worth doing?

Sometimes. Not always.

A small refactor from shared mutable state to branch-owned behavior can be a real improvement. Moving logic closer to the branch that owns it is usually good.

But do not overstate it. In a short PowerShell function, plain if / elseif / else is often already the right answer.

Here is the honest ranking:

- if / elseif / else: usually the most idiomatic choice for effect-heavy PowerShell
- switch: the best native “match-like” compromise
- tiny helper-function mini-language: sometimes readable, often more ceremony than value

## If you want match-like structure in PowerShell, use switch

switch gets you closer to case-based branching without pretending PowerShell is F#.

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

This keeps the case-based shape while staying idiomatic for the language you are actually in.

## The naming rule did not change because AI exists

AI does not make long names automatically better.

The rule is still simple: use the shortest name that is unambiguous in local context.

Good:

- $toRemove
- $localToRemove
- $remoteToRemove

Bad:

- names that restate obvious local context without removing ambiguity

Tooling makes longer names cheaper than they used to be. It does not make overdescribed code good.

## Bottom line

match feels better because it is a case-to-value expression. That is a real strength, not aesthetic hype.

But PowerShell is not built around that model. It is built around commands, pipelines, and side effects.

So the practical rule is:

- when branches return values, match is hard to beat
- when branches perform effects, plain branching is often the clearest form
- use helpers when they isolate meaningful logic
- do not invent a fake F# on top of PowerShell just because the syntax looks cleaner
