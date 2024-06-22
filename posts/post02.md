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
