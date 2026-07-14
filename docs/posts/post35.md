---
draft: false
date: 2026-06-22
categories:
  - coding agents
---

# coding agents - phrases I needed to learn

Claude keeps on using phrases I don't know.
I needed to look them up so to understand what he's is saying.

- telescoping - mathematics - shortening a series by canceling phrases
- vendoring a script - copy a 3rd paty script into your repo rather then reference it as a build time dependency
- coalesce - bring together/join
- provenance - the source/origin of something
- word - I agree (slang) originating from hiphop
- invariant - a condition that must hold/be true at any moment
    - to be more precise - in OOP an invariant of an object must be true from the `end of the constructor` to the `beggining of the destructor`
        - makes sense cuz before the object is fully created or while it's being torn down the `invariant` is allowd not to hold
        - the case where i stumbled onthis is:
            - the ui i build had a few state that could not happen at the same time
                - the invariant here was that the switches "live" and "history" can't be on at the same time
                    - it helped to create one class that manages the state of the UI and ensure the invariant holds
        - another exmple - invariant can be -> the progress has to always bee between 0 and 100 % (can't be negative or more than 100%)
        - an invariant can also be false during execution of a method - when you transition from one state to another
- `hand rolled` code - insted of using a dependency code is written by hand
