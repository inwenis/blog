---
draft: false
date: 2024-06-24
categories:
  - Power Shell
---

# PowerShell quirk

## tl;dr

In PowerShell if you want to return an array instead of one element of the array at the time do this:
```PowerShell
> @(1..2) | % { $a = "a" * $_; @($a,$_) } # wrong! will pipe/return 1 element at a time
> @(1..2) | % { $a = "a" * $_; ,@($a,$_) } # correct! will pipe/return pairs
```
__Beware! Result of both snippets will be displayed in the exact same way even though they have different types!__
See below:
```PowerShell
> @(1,2,3,4)
1
2
3
4
> @((1,2),(3,4))
1
2
3
4
```

To check types of you result
```PowerShell
> $x = @(1..2) | % { $a = "a" * $_; @($a,$_) } ; $x.GetType().Name ; $x[0].GetType().Name ; $x
> $x = @(1..2) | % { $a = "a" * $_; ,@($a,$_) } ; $x.GetType().Name ; $x[0].GetType().Name ; $x
# Alternatively
> @(1..2) | % { $a = "a" * $_; @($a,$_) } | Get-Member -name GetType
> @(1..2) | % { $a = "a" * $_; ,@($a,$_) } | Get-Member -name GetType
# Get-Member only shows output for every distinct type
```

## Longer read
Occasionally I have a fist fight with PS to return an Array instead of one element at a time. PS is a tough oponent. I think I get it now though.


The comma `,` in PS is a binary and unary operator. You can use it with a single or 2 arguments.
```PowerShell
> ,1 # as an unary operator the comma creates an array with 1 member
1
> 1,2 # as an binary operator the comma creates an array with 2 members
1
2
```

Beware that both an array[] and array[][] will be displayed the same way. `$y` is an array[][], it is printed the same way to the output as `$x`
```PowerShell
> $x = @(1,2,3,4) ; $x.GetType().Name ; $x[0].GetType().Name ; $x
Object[]
Int32
1
2
3
4
> $y = @((1,2),(3,4)) ; $y.GetType().Name ; $y[0].GetType().Name ; $y
Object[]
Object[]
1
2
3
4
```



If you're trying to return an array of pairs:
```PowerShell
> @(1..2) | % { $a = "a" * $_; @($a,$_) } # wrong
a
1
aa
2
> @(1..2) | % { $a = "a" * $_; ,@($a,$_) } # correct!
a
1
aa
2
# Even though the result looks like a flat array this this time it's an array of arrays
> @(1..2) | % { $a = "a" * $_; @($a,$_) } | Get-Member -name GetType # we get strings and ints

   TypeName: System.String

Name    MemberType Definition
----    ---------- ----------
GetType Method     type GetType()

   TypeName: System.Int32

Name    MemberType Definition
----    ---------- ----------
GetType Method     type GetType()

> @(1..2) | % { $a = "a" * $_; ,@($a,$_) } | Get-Member -name GetType # we get arrays

   TypeName: System.Object[]

Name    MemberType Definition
----    ---------- ----------
GetType Method     type GetType()

```

More on printing your arrays of pairs:
```PowerShell
> @(1..4) | % { $a = "a" * $_; ,@($a,$_) } | write-output # write-output will "unwind" your array
a
1
aa
2
> @(1..4) | % { $a = "a" * $_; ,@($a,$_) } | write-host
a 1
aa 2
> @(1..4) | % { $a = "a" * $_; ,@($a,$_) } | % { write-output "$_" }
a 1
aa 2
> @(1..4) | % { $a = "a" * $_; ,@($a,$_) } | write-output -NoEnumerate # returns an array of arrays but it's printed as if it's a flat array
a
1
aa
2
```

This [https://stackoverflow.com/a/29985418/2377787](https://stackoverflow.com/a/29985418/2377787) explains how `@()` works in PS.

```PowerShell
> $a='A','B','C'
> $b=@($a;)
> $a
A
B
C
> $b
A
B
C
> [Object]::ReferenceEquals($a, $b)
False
```
Above `$a;` is understood as `$a` is a collection, collections should be enumerated and each item is passed to the pipeline. `@($a;)` sees 3 elements but not the original array and creates an array from the 3 elements. In PS `@($collection)` creates a copy of `$collection`. `@(,$collection)` - creates an array with a single element `$collection`.

