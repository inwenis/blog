---
draft: false
date: 2024-12-13
categories:
  - F Sharp
  - regex
---

# <3 regex

[https://regex101.com/r/RdCR7j/1](https://regex101.com/r/RdCR7j/1) - set the global flag (g) to get all matches

[https://www.debuggex.com/](https://www.debuggex.com/) - havent't played with this a lot but I might give it a try, looks like a decent learning tool

## regex - use static Regex.Matches() or instantiante Regex()?

By default use static method.

.NET regex engine caches regexes (by default 15).

Are you *using more than 15 regexes* and *use them frequently* and *they're complex* and *you care about a performance*?
> Investigate `Regex()` and `RegexOptions.Compiled` `RegexOptions.CompiledToAssembly`

> Test performance before you optimize

[https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-regex#static-regular-expressions](https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-regex#static-regular-expressions)

[https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions?view=net-9.0](https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions?view=net-9.0)

## What is the whole fus about backtracing?

Microsoft's documentation does a bad job explaning backtracking.

Read about backtracking here - [https://www.regular-expressions.info/catastrophic.html](https://www.regular-expressions.info/catastrophic.html)

To experience backtracing yourself - [https://regex101.com/r/1rWKNN/1](https://regex101.com/r/1rWKNN/1)
- keep on adding "x" to the input and see how the execution time increses
- with 35*"x" it takes 5 seconds for the regex to find out it doesn't match!

## Code


These are the methods you need:

```F#
open System
open System.Text.RegularExpressions


Regex.Matches("input", "pattern")
Regex.Matches("input", "pattern", RegexOptions.IgnoreCase ||| RegexOptions.Singleline)
Regex.Matches("input", "pattern", RegexOptions.IgnoreCase ||| RegexOptions.Singleline, TimeSpan.FromSeconds(10.)) // you can use a timeout to prevent a DoS attack with malicous inputs
Regex.Match()
Regex.IsMatch()
Regex.Replace()
Regex.Split()
Regex.Count()

let r = new Regex("pattern") // instance Regex offers the same methods
r.Matches("input")
```
[Regex class - https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=net-9.0](https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=net-9.0)

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

let matches2 = Regex.Matches("Lorem ipsum dolor sit amet, consectetur adipiscing elit", "(\w)+o")
matches2.[1].Groups.[1].Value |> printfn "%A"
matches2.[1].Groups.[1].Captures |> Seq.iter (fun c -> printfn "%s" c.Value)
// l              // gotcha! the value of the group is the last thing captured by that group
// d              // here the (\w)+ group captures 3 times
// o              //
// l              //
```
```
Match object properties:
Match.Success -> bool   | true      | false        |
Match.Value   -> string | the match | String.Empty |
```
```F#
let match3 = Regex.Match("Lorem ipsum dolor sit amet, consectetur adipiscing elit", "Lorem i[a-z ]+i")
match3.Success |> printfn "%A"
match3.Value   |> printfn "%A"
// true
// "Lorem ipsum dolor si"

let match4 = Regex.Match("Lorem ipsum dolor sit amet, consectetur adipiscing elit", "Lorem i[A-Z ]+i")
match4.Success            |> printfn "%A"
match4.Value              |> printfn "%A"
match4.Groups.Count       |> printfn "%A"
match4.Groups.[0].Success |> printfn "%A"
// false
// ""    // notice this is String.empty not <null>
// 1     // even for a failed match there is always at least one group
// false
```

```F#
let mutable m = Regex.Match("Lorem ipsum dolor sit amet, consectetur adipiscing elit", "\wo")
while m.Success do
    printfn "%s" m.Value
    m <- m.NextMatch()

let lines = [
    "The next day the children were ready to go to the plum thicket in the"
    "peach orchard as soon as they had their breakfast, but while they were"
    "talking about it a new trouble arose. It grew out of a question asked by"
    "Drusilla."
]

lines
|> List.filter (fun line -> Regex.IsMatch(line, "the"))
|> List.map    (fun line -> Regex.Replace(line, "(\w+) the", "the $1"))

let text =
    "don't we all love\n" +
    "dealing with different\r\n" +
    "line endings\n" +
    "it's so much fun"
Regex.Split(text, "\r?\n")
|> Array.iter (printfn "%s")

open System.Net.Http
let book = (new HttpClient()).GetStringAsync("https://www.gutenberg.org/cache/epub/74886/pg74886.txt").Result
Regex.Count(book, "[^\w]\w{3}[^\w]") |> printfn "%d" // count 3 letter words
```

# regex - Quick Reference (Microsoft)
[https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference](https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference)

# Cheat sheet

Character escapes
```
\t     matches a tab \u0009
\r     match a carriage return \u000D
\n     new line \u000A
\unnnn match a unicode character by hexadecimal representation, exactly 4 digits
\.     match a dot (not any character) aka. match literally
\*     match an asterisk (don't interpret * as a regex special quantifier)
```

Character classes
```
[character_group]       /[ae]/ will match "a" in "gray"
[^not_character_group]
[a-z] [A-Z] [a-z0-9A-Z] character ranges
.                       wildcard - any character except \n except when using SingleLine option
\w                      word character - upper/lower case letters and numbers
\W                      non word character
\s                      white-space character
\S                      non whitespace character
\d                      digit
\D                      non digit
```

Anchors
```
^   $ beginning and end of a string (in multiline mode beginning and end of a line)
```

Grouping
```
(subexpression)               (\w)\1 - match a character and the same character again - "aa" in "xaax"
(?<name>subexpression)        named group (?<double>\w)\k<double> - same as above
(?:subexpression)             noncapturing group - Write(?:Line)? - will match both Write and WriteLine in a string
                              (:?Mr\. |Ms\. |Mrs\. )?\w+\s\w+ -> match fist name, last name and optional preceding title
(?imnsx-imnsx: subexpression) turn options on or off for a group
(?=subexp)                    zero-width positive lookahead assertion
(?!subexp)                    negative lookahead
(?<=subexp)
(?<!subexp)                   look behind assertions
                              make sure a subexp is/is not following (but don't match it, ie. don't consume the characters)
```

Quantifiers
```
*     0...n (all these are greedy by default -> match as many as possible)
+     1...n
?     0...1
{n}   exactly n
{n,}  at least n
{n,m} n...m
*?
+?
??
{n,}?
{n,m}? question mark makes the match nongreedy (mach as few as possible)
```

Backreference
```
\number   match the value of a previous subexpression - (\w)\1 - matches the same \w character twice
\k<name>  backreference using group name
```

Alternation Constructs
```
| - any element separated by | - th(e|is|at) and the|this|that both match "the" "this" "that"
    ala|ma|kota - match "ala" or "ma" or "kota"
    ala ma (kota|psa) - match "ala ma kota" or "ala ma psa"
TODO - match yes if expresion else match no
```

Substitution
```
$number use numbered group
${name} use named group
$$      literal $
$&      whole match
$`      text before the match
$'      text after the match
$+      last group
$_      entier input string
```

Inline options
```
(?imnsx-imnsx)               use it like this at the beginning
(?imnsx-imnsx:subexpression) use for a group
i                            case insensetive
m                            multiline - match beginning and end of a line
n                            do not capture unnamed groups
s                            signle line - . matches \n also
```
More options are available using `RegexOptions` enum


## Practice regex

[https://regex101.com/quiz](https://regex101.com/quiz)

[https://regexcrossword.com/](https://regexcrossword.com/)

[https://alf.nu/RegexGolf](https://alf.nu/RegexGolf)

## Tutorial

I recall reading this tutorial years ago and I liked it - [https://www.regular-expressions.info/tutorial.html](https://www.regular-expressions.info/tutorial.html)

## Misc
[https://blog.codinghorror.com/regular-expressions-now-you-have-two-problems/](https://blog.codinghorror.com/regular-expressions-now-you-have-two-problems/)

I love regex.

However I used to say "if you solve a problem with regex now you have 2 problems"

Not knowing how this quote came to be I repeated it for years. I'll smack the next person to repeat this quote without elaborating.

If regex did not exist, it would be necessary to invent it.

Why does `.Matches()` return a custom collection instead of `List<Match>`?
> Historic reasons. `Regex` was made in .Net 1.0 before generic were a thing.

[https://github.com/dotnet/runtime/discussions/74919?utm_source=chatgpt.com](https://github.com/dotnet/runtime/discussions/74919?utm_source=chatgpt.com)

I used `(?<!\[.*?)(?<!\(")https?://\S+` with replace `[$&]($&)` to linkify links in this post

My lovely regex helpers
```F#
let regexExtract  regex                      text = Regex.Match(text, regex).Value
let regexExtractg regex                      text = Regex.Match(text, regex).Groups.[1].Value
let regexExtracts regex                      text = Regex.Matches(text, regex) |> Seq.map (fun x -> x.Value)
let regexReplace  regex (replacement:string) text = Regex.Replace(text, regex, replacement)
let regexRemove   regex                      text = Regex.Replace(text, regex, String.Empty)
```
