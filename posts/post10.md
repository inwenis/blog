# How the way I work/code/investigate/debug changed with time & experience

I use this metaphor when describing how I work these days.
[vid here]

# TL;DR;

1. Quick feedback is king
    - unit tests
    - quick test in another way
    - reproduce issue locally
2. One thing at a time
    - experimenting
    - refactoring preparing for feature addition
    - feature coding
    - clean up after feature coding
3. Divide problems into smaller problems
4. Solve all sub problems separately
5. Solve all pieces independently of your project, create a project on the side
6. Do not experiment in your large/heavy project*


\* conditions apply - don't try to figure out how `List.foldBack()` works while debugging a big project. Do it on the side.


---

## Example

You're working with code that talks to a remote API, you want to test different API calls to the remote API.

__don't__ - change API parameters in code and run the project each time you test sth. It takes too long.

__do__ - write a piece of code to send a HTTP request, fiddle with this code

__do__ - intercept request with `fiddler`/`postman`/other interceptor and reissue requests with different parameters

---
## Example
Something fails in CI pipeline.

__don't__ make a change, commit, wait for remote CI to trigger, see result

__do__ reproduce issue locally

---

1. Quick feedback
   - __do__ - write a test for it
   - __do__ - isolate your issue/suspect/the piece of code you're working with
      - it is helpful if you can run just a module/sub-system/piece of your project/system
   - if you relay on external data and it takes time to retrieve it (even a 5 seconds delay can be annoying) - dump data to a file and read it from the file instead of hitting an external API or a DB every time you run your code
---

2. Separate refactoring from feature addition
   - fiddle first, find the walls/obstacles
   - `git reset --hard`
   - refactor preparing for new feature (can become a separate PR)
   - code feature
   - if during coding you find something that needs refactoring/renaming/cleaning up - any kind of "wtf is this I need to fix this!" try a) or b)
      - a) make a note to fix it later
      - b) fix immediately
        ```
        > git stash
        > git checkout master
        > git checkout -b fix-typo
        fix stuff
        merge or create a PR
        git checkout feature
        > git merge fix-typo or git rebase fix-typo
        continue work
        ```
   - always have a paper notepad on your desk
      - note things you would like to come back to or investigate
      - it gives me great satisfaction to go through a list of "side quests" I have noted and strike though all of them, knowing I have dealt with everyone
      - when investigating sth I also note question I would like to be able to answer after I'm done investigating
         - example: while working with axios and cookies i found conflicting information whether axios supports cookies. After the investigation I knew that axios supports cookies by default in a browser but not in node js

5. Spin up a new solution/project on the side to test things
   - partial execution helps - like in python/jupiter or F# fsx
   - occasional I hear junior asking "does this work in this way" - you can test it your self easily if you do it on the side


# Big bang vs baby steps
The old me often ended up doing the __big bang__. Rewriting large pieces of code at once. Starting things from scratch. Working for hours or days with a code base that can't even compile.

Downsides
   - for a long time the project doesn't even compile, I lose motivation, I feel like I'm walking in the dark, I don't see errors for a long time
   - requires a lot of context keeping in my mind since I've ripped the project apart
   - if I abandon work for a few days sometimes I forget everything and progress is lost

The new me prefers __baby steps__

Fiddle with the code knowing I'll `git reset --hard`. Try renaming some stuff - helps me understand the code base better. Try out different things and abandon them. Get an idea/feeling of what needs to be done. Plan a few smaller refactorings. After them I am usually closer to the solution and am able to code it without a big bang.
