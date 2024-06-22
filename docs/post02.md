# Exercises in bash/shell/scripting

Being fluent in shell/scripting allows you to improve your work by 20%. It doesn't take you to another level. You don't suddenly poses the knowledge to implement flawless distributed transactions but some things get done much faster with no frustration.

Here is my collection of shell/scripting exercises for others to practice shell skills.

A side note - I'm still not sure if I should learn more PowerShell, try out a different shell or do everything in F# fsx. PowerShell is just so ugly ;(

## Exercise 1

What were the arguments of `DetectOrientationScript` function in [https://github.com/tesseract-ocr/tesseract](https://github.com/tesseract-ocr/tesseract) when it was first introduced?

Scroll down for answer

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
Bash is faster then PowerShell. PowerShell 7 is **much** faster then PowerShell 5. It was surprisingly easy to get the average with `Measure-Object` in PowerShell. It was surprisingly difficult in bash to get the average.
