# Exercises in bash/shell/scripting

Being fluent in shell/scripting allows you to improve your work by 20%. It doesn't take you to another level. You don't suddenly poses the knowledge to implement flawless distributed transactions but some things get done much faster with no frustration.

Here is my collection of shell/scripting exercises for others to practice shell skills.

A side note - I'm still not sure if I should learn more PowerShell, try out a different shell or do everything in F# fsx. PowerShell is just so ugly ;(

Scroll down for answers

## Exercise 1

What were the arguments of `DetectOrientationScript` function in [https://github.com/tesseract-ocr/tesseract](https://github.com/tesseract-ocr/tesseract) when it was first introduced?

## Exercise 2

Get `Hadoop distributed file system log` from [https://github.com/logpai/loghub?tab=readme-ov-file](https://github.com/logpai/loghub?tab=readme-ov-file)

Find the ratio of (failed block serving)/(failed block serving + successful block serving) for each IP

The result should like:
```
...
10.251.43.210  0.452453987730061
10.251.65.203  0.464609355865785
10.251.65.237  0.455237129089526
10.251.66.102  0.452124935995904
...
```

## Exercise 3

This happened to me once - I had to find all http/s links to a specific domains in the export of our company's messages as someone shared proprietary code on websites available publicly.

Exercise - find all distinct http/s links in [https://github.com/tesseract-ocr/tesseract](https://github.com/tesseract-ocr/tesseract)

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

.

## Exercise 1 - answer

Answer:
```C++
bool DetectOrientationScript(int& orient_deg, float& orient_conf, std::string& script, float& script_conf);
```

```PowerShell
[PowerShell]
> git log -S DetectOrientationScript # get sha of oldest commit
> git show bc95798e011a39acf9778b95c8d8c5847774cc47 | sls DetectOrientationScript

[bash]
> git log -S DetectOrientationScript # get sha of oldest commit
> git show bc95798e011a39acf9778b95c8d8c5847774cc47 | grep DetectOrientationScript
```

One-liner:
```PowerShell
[PowerShell]
> git log -S " DetectOrientationScript" -p | sls DetectOrientationScript | select -Last 1

[bash]
> git log -S " DetectOrientationScript" -p | grep DetectOrientationScript | tail -1
```

### Bonus - execution times
```PowerShell
[PowerShell 7.4]
> measure-command { git log -S " DetectOrientationScript" -p | sls DetectOrientationScript | select -Last 1 }
...
TotalSeconds      : 3.47
...

[bash]
> time git log -S " DetectOrientationScript" -p | grep DetectOrientationScript | tail -1
...
real    0m3.471s
...
```

Without `git log -S` doing heavy lifting times look different:

```PowerShell
[PowerShell 7.4]
> @(1..10) | % { Measure-Command { git log -p | sls "^\+.*\sDetectOrientationScript" } } | % { $_.TotalSeconds } | Measure-Object -Average

Count    : 10
Average  : 9.27122774
```
```PowerShell
[PowerShell 5.1]
> @(1..10) | % { Measure-Command { git log -p | sls "^\+.*\sDetectOrientationScript" } } | % { $_.TotalSeconds } | Measure-Object -Average

Count    : 10
Average  : 27.33900077
```
```bash
[bash]
> seq 10 | xargs -I '{}' bash -c "TIMEFORMAT='%3E' ; time git log -p | grep -E '^\+.*\sDetectOrientationScript' > /dev/null" 2> times
> awk '{s+=$1} END {print s}' times
6.7249 # For convince I moved to dot one place to the left
```

### Reflections
Bash is faster then PowerShell. PowerShell 7 is **much** faster then PowerShell 5. It was surprisingly easy to get the average with `Measure-Object` in PowerShell and surprisingly difficult in bash.

## Exercise 2 - answer

```PowerShell
[PowerShell 7.4]
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | % { $_.Matches[0].Value -replace "Served block.*/","ok/" -replace "Got exception while serving.*/","nk/" -replace ":","" } | % { $_ -replace "(ok|nk)/(.*)", "`${2} `${1}"} | sort > sorted
> cat .\sorted | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x } | % { write-output "$_" }
```

This is how I got to the answer:
```PowerShell
> sls "Served block" -Path .\HDFS.log | select -first 10
> sls "Served block|Got exception while serving" -Path .\HDFS.log | select -first 10
> sls "Served block|Got exception while serving" -Path .\HDFS.log | select -first 100
> sls "Served block|Got exception while serving" -Path .\HDFS.log | select -first 1000
> sls "Served block.*|Got exception while serving" -Path .\HDFS.log | select -first 1000
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | select -first 1000
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log -raw | select -first 1000
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log -raw | select matches -first 1000
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log -raw | select Matches -first 1000
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log -raw | select Matches
> $a = sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log -raw
> $a[0]
> get-type $a[0]
> Get-TypeData $a
> $a[0]
> $a[0].Matches[0].Value
> $a = sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log
> $a[0]
> $a[0].Matches[0].Value
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | % { $_.Matches[0].Value -replace "Served block.*/","ok/" }
> "asdf" -replace "a","b"
> "asdf" -replace "a","b" -replace "d","x"
> "asdf" -replace "a.","b" -replace "d","x"
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | % { $_.Matches[0].Value -replace "Served block.*/","ok/" -replace "Got exception while serving.*/","nk" }
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | % { $_.Matches[0].Value -replace "Served block.*/","ok/" -replace "Got exception while serving.*/","nk/" }
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | % { $_.Matches[0].Value -replace "Served block.*/","ok/" -replace "Got exception while serving.*/","nk/" -replace ":","" }
> "aaxxaa" -replace "a.","b"
> "aaxxaa" -replace "a.","b$0"
> "aaxxaa" -replace "a.","b$1"
> "aaxxaa" -replace "a.","b${1}"
> "aaxxaa" -replace "a.","b${0}"
> "aaxxaa" -replace "a.","b`${0}"
> "okaaxxokaa" -replace "(ok|no)aa","_`{$1}_"
> "okaaxxokaa" -replace "(ok|no)aa","_`${1}_"
> "okaaxxokaa" -replace "(ok|no)aa","_`${1}_`${0}"
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | % { $_.Matches[0].Value -replace "Served block.*/","ok/" -replace "Got exception while serving.*/","nk/" -replace ":","" } | % { $_ -replace "(ok|nk)/(.*)", "`${2} `${1}"}
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | % { $_.Matches[0].Value -replace "Served block.*/","ok/" -replace "Got exception while serving.*/","nk/" -replace ":","" } | % { $_ -replace "(ok|nk)/(.*)", "`${2} `${1}"} | sort
> sls "Served block.*|Got exception while serving.*" -Path .\HDFS.log | % { $_.Matches[0].Value -replace "Served block.*/","ok/" -replace "Got exception while serving.*/","nk/" -replace ":","" } | % { $_ -replace "(ok|nk)/(.*)", "`${2} `${1}"} | sort > sorted
> cat .\sorted -First 10
> cat | group
> cat | group -Property {$_}
> cat .\sorted | group -Property {$_}
> cat .\sorted -Head 10 | group -Property {$_}
> cat .\sorted -Head 100 | group -Property {$_}
> cat .\sorted -Head 1000 | group -Property {$_}
> cat .\sorted -Head 10000 | group -Property {$_}
> cat .\sorted -Head 10000 | group -Property {$_} | select name,count
> cat .\sorted | group -Property {$_} | select name,count
> cat .\sorted | group -Property {$_ -replace "nk|ok",""}
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""}
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $_.name, $g.Length / $_.count }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $_.name, $g.Length, $_.count }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $_.name, $g.Length / $_.count }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $_.name, $g.Length, $_.count }
> $__
> $__[0]
> $__[1]
> $__[2]
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $_.name, $g.Length, $_.count }
> $a = cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $_.name, $g.Length, $_.count }
> $a[0]
> $a[1]
> $a[2]
> $a[1].GetType()
> $a[2].GetType()
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $_.name, ($g.Length) / ($_.count) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $_.name, (($g.Length) / ($_.count)) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; ,$_.name, (($g.Length) / ($_.count)) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; @($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; ,@($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; return ,@($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; [Array] ,@($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; [Array]@($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; return ,$_.name, (($g.Length) / ($_.count)) }
> $a = cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; return ,$_.name, (($g.Length) / ($_.count)) }
> $a[0]
> $a = cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; return ,($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; return ,($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; return ,@($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; ,@($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))) }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); $x }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); ,$x }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x }
> $a = cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x }
> $a[0]
> $a[0][0]
> $a = cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x } | % { wirte-output "$_[0]" }
> $a = cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x } | % { write-output "$_[0]" }
> $a = cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x } | % { write-output "$_[0]" }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x } | % { write-output "$_[0]" }
> cat .\sorted -Head 10000 | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x } | % { write-output "$_" }
> cat .\sorted | group -Property {$_ -replace "nk|ok",""} | % { $g = $_.group | ? {$_.contains("nk") }; $x = @($_.name, (($g.Length) / ($_.count))); return ,$x } | % { write-output "$_" }
```

```F#
[F#]
open System.IO
open System.Text.RegularExpressions

let lines = File.ReadAllLines("HDFS.log")

let a =
    lines
    |> Array.filter (fun x -> x.Contains("Served block") || x.Contains("Got exception while serving"))

a
// |> Array.take 10000
|> Array.map (fun x ->
    let m = Regex.Match(x, "(Served block|Got exception while serving).*/(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})")
    m.Groups[2].Value,
    match m.Groups[1].Value with
    | "Served block"                -> true
    | "Got exception while serving" -> false )
|> Array.groupBy fst
|> Array.map (fun (key, group) ->
    let total = group.Length
    let failed = group |> Array.map snd |> Array.filter not |> Array.length
    key, (decimal failed)/(decimal total)
    )
|> Array.sortBy fst
|> Array.map (fun (i,m) -> sprintf "%s  %.15f" i m)
|> fun x -> File.AppendAllLines("fsout", x)

```

## Exercise 3 - answer

```PowerShell
[PowerShell 7.4]
> ls -r -file | % { sls -path $_.FullName -pattern https?:.* -CaseSensitive } | % { $_.Matches[0].Value } | sort | select -Unique

# finds 234 links
```

```bash
[bash]
> find . -type f -not -path './.git/*' | xargs grep -E https?:.* -ho | sort | uniq

# finds 234 links
```
