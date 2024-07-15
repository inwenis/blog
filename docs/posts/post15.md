---
draft: false
date: 2024-07-15
categories:
  - PowerShell
  - scripting
  - exercises
---

# PowerShell "Oopsie"

## Task - remove a specific string from each line of multiple CSV files.

This task was added to the [scripting exercise list](post02.md).

First - let's generate some CSV files to work with:
```PowerShell
$numberOfFiles = 10
$numberOfRows = 100

$fileNames = 1..$numberOfFiles | % { "file$_.csv" }
$csvData = 1..$numberOfRows | ForEach-Object {
    [PSCustomObject]@{
        Column1 = "Value $_"
        Column2 = "Value $($_ * 2)"
        Column3 = "Value $($_ * 3)"
    }
}

$fileNames | % { $csvData | Export-Csv -Path $_ }
```

## The "Oopsie"
```PowerShell
ls *.csv | % { cat $_ | % { $_ -replace "42","" } | out-file $_ -Append }
```

This command will never finish. Run it for a moment (and then kill it), see the result, and try to figure out what happens. Explanation below.

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

.

.

.

.

.

.

.

.

## The explanation
`Get-Content` (aka. `cat`) keeps the file open and reads the content that our command is appending, thus creating an infinite loop.


## The fix

There are many ways to fix this this "oopsie"

Perhaps the simplest one is to not write to and read from the exact same file. A sensible rule is __when processing files always write to a different file__:

```PowerShell
ls *.csv | % { cat $_ | % { $_ -replace "42","" } | out-file -path "fixed$($_.Name)" }
```

Knowing the reason for our command hanging we can make sure the whole file is read before we overwrite it:
```PowerShell
ls *.csv | % { (cat $_ ) | % { $_ -replace "42","" } | out-file $_ }
ls *.csv | % { (cat $_ ) -replace "42","" | out-file $_ } # we can also use -replace as an array operator
```

I'm amazed by github's co-pilot answer for "powershell one liner to remove a specific text from multiple CSV files":

```PowerShell
Get-ChildItem -Filter "*.csv" | ForEach-Object { (Get-Content $_.FullName) -replace "string_to_replace", "replacement_string" | Set-Content $_.FullName }
```
