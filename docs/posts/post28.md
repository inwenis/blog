---
draft: false
date: 2025-03-07
categories:
  - misc
---

# escape!

Why is escaping a character called escaping a character?

When a C compiler (or any other compiler) encounters a `"` it thinks to it self "huh, this is the begninning or end of a string!".

Say you need `"` in your string?

You have to tell the compiler "do not process this character like you always do, I want you to ESCAPE the default processing". You do this with `"dear compiler this \" will become a double quotation mark in my string"`.

Likewise if you want a new line you need to tell the compile "do not process this n as a "n", escape the default processing and process it as a new line \n".


