An exercise in shell/scripting/regex

At my company consultants used to exchange code on pastebin.
We obviously don't want our code publicly available on pastebin.
We have exported our slack workspace and the task is to find all suspicious links.

I can't share the actual slack export so for this exercise we use a sample slack export.

1/ find all links from all messages in this slack export
2/ find all domains used in those links (to see if there are anyother suspicoius domains other than pastebin.)



TODO for filip:
- since this is a sample slack export it actually doesn't have any pastebin links - TODO - create some dummy messages in the export with pastebin links
- check how many links are in there so people know when they're done
- create some silly pastebins so people know they've found the right ones
