# Exercises in bash/shell/scripting

Being fluent in shell/scripting allows you to improve your work by 20%. It doesn't take you to another level. You don't suddenly poses the knowledge to implement flawless distributed transactions but some things get done much faster with no frustration.

Here is my collection of shell/scripting exercises for others to practice shell skills.

A side note - I'm still not sure if I should learn more PowerShell, try out a different shell or do everything in F# fsx. PowerShell is just so ugly ;(

## Exercise 1

What were the arguments of `DetectOrientationScript` function in https://github.com/tesseract-ocr/tesseract when it was first introduced?

Try to do it without `git log -S` <- TODO - hide these

Solution with git:
```
git log -S DetectOrientationScript
```

Why do these returns differ?
```
git log -S DetectOrientationScript -p | sls DetectOrientationScript
git log -p | sls "^\+.*DetectOrientationScript"
```

Measure time of different scripts/commands
```PowerShell
measure-command
```

What is faster?
  - `git log -p | sls "^\+.*DetectOrientationScript"`
  - or doing the same but reading git log from file?


## Exercise 2

Get `Hadoop distributed file system log` from https://github.com/logpai/loghub?tab=readme-ov-file
Do sth like:
    - count log entries that mention "10.251.195.70"
    `sls 10.251.195.70 -path .\HDFS.log | measure`
    - TODO - figure out some extraction and sorting
    so that the solution becomes sth like
    `cat .\HDFS.log | sls -Pattern "(?:.*10.251.195.70.*?)(\d+)" -AllMatches | % { $_.matches.groups[1].value } | % {[int]$_} | sort | select -first 10`
    check for duplicates
    or maybe find all exceptions or sth like that



https://explainshell.com/explain?cmd=git+xargs+du+-c

https://github.com/jlevy/the-art-of-command-line?tab=readme-ov-file#windows-only

## Exercise 3

Real-life example - I once had to find all http/s link to a specific domain in the slack export as someone used a website and publicly shared prioriatery code.

Exerice - find all distinct http links in https://github.com/tesseract-ocr/tesseract repo

`ls -r -file | % { sls -path $_.FullName -Pattern http } | % { $_.line } | sort | select -Unique | Measure-Object`
