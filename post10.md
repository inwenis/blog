# How my way co working/coding/investigating/debugging changed with time & experience?

-> quick feedback is king
picture here

split problem
solve all pieaces separately
solbe all pieces indepentd of you project, create a project on the side - example - self updating exe.

don't - don't code everything in your large/heavy project


---

case: you're working with a large or small system/program, it talks to a remote API, you want to test different API calls to the remote API

don't - change API parameters in code and run whole system each time you test sth
do - write a piece of code to send a HTTP request, fiddle with this code
do - intercept request with fiddler/simillar and reissue requests with different parameters

---

case: sth fails in CI pipeline

don't: make a change, commit, wait for remote CI to trigger and see result

do: reproduce issue locally

---

(quick feedback)

do: write a test for it

do: isolate your issue/suspect/the piece of code you're working with

it is helpful if you can run just a module/sub-system/piece of your project/system

---

separate refactoring from feature adding


-> fiddle first, find the "walls" obstacles

-> git reset --hard

-> refactor preparing for new feature (can become a separate PR)

-> code feature

-> if during codeing you find something that needs refactoring/ranaming/cleaning up - any kind of wtf is this

```
git stash
git checkout master
git checkout -b fix-typo
fix stuff
merge or create a PR
git checkout feature
git merge fix-typo or git rebase fix-typo
continue work
```

more dos:

- spin up a new solution/project on the side to test things, use 

- have a notepad on the side all time with you
    - note things you would like to come back to / investigate
    - it gives me satisfaction when after a task i have notes and all are stricken thru because I managed to investigate all "side quests" while/after the main feature
    - when investigating sth i also note down question I would like to be able to answer after I'm done investigating
        - example: while working with axios and cookies i found conflicting information if axios supports cookies. After the investigation I knew that axios supports cookies by default in a browser but not in node js

- partial execution helps - like in python/jupiter or F# fsx

- if you use/rely on external data and it takes time to load/get it (even at 2s I consider doing this) - dump data to a file and read it from the file instead of hitting an external API or a DB every time you run your code to test something

sometimes I debat 2 appraches when working on sth:

"rewrite/big bang" vs baby steps
- downside of "rewrite/big bang"
    - for a long time the project doesn't even compile, I lose motivation, I feel like I'm walking in the dark, I don't see errors for a long time
    - requires a lot of context keeping in my mind since I've ripped the project apart
    - if I abandon work for a few days sometimes I forget everything and progress is lost

then I try to fiddle with the code,

rename and abandon, refactor and abandon, try out different ways.
Then I often get an idea/feeling of what needs to be done. I plan a few smaller refactorings. After them I am usually closer to the solution and am able to code it without a big bang.


- testing is good and nice, it's quick feedback.


- narrow down the issue/suspect

    - you'll not even know what to ask for/ what to google without narrowing down the issue
        - why doesn't it work?
        - why do these results differ?
        - why does this number differ?
        - why is this list mutated?
        - why is my sample list mutates?
        - find where list gets mutated
        - it's a sort
        - google "sort doesn't preserve order"
        - find out about stable sort

