# Exercises in bash/shell/scripting

I think that being fluent in the shell/scripting of you choice allows you to improve your work by 20%. It doesn't take you to another level. You don't suddenly poses the knowlege to implement flawless distributed transactions but you get some things done much faster. Less frustration. I often have/had this thought of "if only I knew bash/PowerShell better I would be able to find this information in minutes".

So I thouth I'll collect exercies for others to practice their shell skills.

As a side note - I'm still not sure if I should learn more PowerShell or try out a different Shell. Or perhaps do everything in F# since you can have .fsx files. PowerShell is just so ugly.

## Exercise 1

What were the arguments of `DetectOrientationScript` function in https://github.com/tesseract-ocr/tesseract when it was first introduced?

Try to do it without `git log -S` <- TODO - hide these

solution with git:
git log -S DetectOrientationScript

Why do these returns differ?
git log -S DetectOrientationScript -p | sls DetectOrientationScript
git log -p | sls "^\+.*DetectOrientationScript"

Measure time of different scripts/commands
-> powershell measure-command

dump git log from `git log -p | sls "^\+.*DetectOrientationScript"` to file so it's faster, check if it's faster


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
