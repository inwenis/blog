# my regex cheat sheet

## regex - use static Regex. or new Regex()

https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-regex#static-regular-expressions

by default use static method

.NET regex engine caches regexes (by default 15)

Are you using more than 15 regexes and use them heavily and they're complex and you care about a performance gain?
-> Condiser instantiating `Regex()` and using `RegexOptions.Compiled`

https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions?view=net-9.0

```F#
open System
open System.Text.RegularExpressions


new Regex("pattern")
new Regex("pattern", RegexOptions.IgnoreCase ||| RegexOptions.Singleline)
new Regex("pattern", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10.)) // you can use a timeout to prevent a DoS attack with malicous inputs

```

Regex() instance and static have the same methods:

This is what you need:

```
.Matches()
.Match()
.IsMatch()
.Replace()
.Split()
.Count()
```

All described here:

https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=net-9.0

Sample:

```F#
let matches = Regex.Matches("Lorem ipsum dolor sit amet, consectetur adipiscing elit", "(\w)o")
matches |> Seq.iter (fun x -> printfn "%s" x.Value)
matches |> Seq.iter (fun x -> printfn "%A" x.Groups)
matches.[0].Groups.[1].Value |> printfn "%s"

// Lo             // these are the whole matches
// do             //
// lo             //
// co             //
// seq [Lo; L]    // group 0 is the whole match, group 1 is the (\w)
// seq [do; d]    //
// seq [lo; l]    //
// seq [co; c]    //
// L              // this is the letter captured by (\w)
```

Why do .Matches() return a custom collection instead of `List<Match>` - historic reasons. `Regex` was made in .Net 1.0 before generic were a thing

https://github.com/dotnet/runtime/discussions/74919?utm_source=chatgpt.com


```
example for all other
```

regex tester

groupes captures
