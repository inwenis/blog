---
draft: false
date: 2026-06-03
categories:
  - coding agents
---

# coding agents - my thoughts

## tl;dr

Coding agents are amazing — no doubt. (em dash not accidental)

They give you impostor syndrome nailing a bug after a single prompt.

But:

Senior devs never wrote down how they molest/mutate/abuse code to examine it.
So LLMs had no way to learn it and they don't do it.

Coding agents also:

- preserve too much (afraid to alter existing behaviour)
- turn accidental behavior into supported behavior (no test for this crazy accidentally supported case? better add one!)
- assume too much (they "dry run" code)
- verify too little (aka. hallucinate)
- their premise is "for any requirement, code an `enterprise-ready` solution"
- they don't follow the flow of code or debug it - they just `grep` strings

What to do?
Start with the usual:

> There is a bug. We expect xyz for a but we get abc. Fix it.

complement it with:

> prove to me with an end to end test that your fix works.

What I only now realized I started doing with experience:

- do not just read the code
- abuse it
- rip it apart
- run strange little experiments
- create throwaway tools & scripts
- break the weird accidental feature if you know no one really asked/uses it (this one I was aware of)

You can prompt the agent to abuse code, rip it apart just to run separate pieces, grow understanding, and validate claims.

## fast mode

Lately I have been using Claude with fast mode enabled.

Works great. Costs quite a lot.

Not necessarily because the model is smarter. Maybe it is not. But because the feedback loop is shorter.

If the agent is slow, I multitask.
If I multitask, I lose context.
If I lose context, I stop steering the agent properly.

Then the agent goes somewhere weird and I have to reconstruct what happened.

That is exhausting.

## hallucinated conclusions

One useful trick is to make the agent list the claims it made.

For example:

- what did you assume?
- what did you verify?
- what is only a guess?
- what file proves this?
- what command can I run to check it?

This helps a lot.

It does not remove hallucinations, but it makes them easier to catch.

## code is not the truth

One thing I see often is that agents treat existing code like Moses treated his stone commandments.

If the code implies that this ridiculous combination of features and configs is possible, it must be intentional and thou shalt not break it!

This is understandable. Human developers do this too, especially in unfamiliar systems.

It takes time for a human to realize:

> I have seen this oddity before, but I have never seen any proof that it is used intentionally.

But agents do it very strongly.

If some accidental behavior exists, the agent may preserve it. It may even build more code around it.

They might harden the ridiculous functionality with tests.

We would not want to break functionality when refactoring, do we now?

The agent sees that and thinks:

> this must be supported

But sometimes the correct answer is:

> no, this is nonsense, break it

I often have to explicitly say this.

You can change this behavior.
Nobody depends on this.
Nobody should depend on this.
This weird dysfunctionality is not a feature.

Without that permission, the agent may keep carrying the weirdness forward.

## defensive coding

Agents also overengineer.

They handle too many cases.

They add defensive code for theoretical problems.

They might add abstractions too early.

The less I review the code they write, the more I can feel the bloat accumulating somewhere.

They add fallback paths.

> What if that tool is not available on the user's PC? Let me code a hand-rolled simplified version. LOL.

Often during the review phase I tell it to KISS.

Or:

> do not code defensively against imaginary issues

This is not because defensive coding is always bad.

It is because code has a cost.

Every extra branch is something to read.
Every extra case is something to test.
Every extra abstraction is another thing to understand.

Every extra supported case is something the agent will resist breaking next time it touches the code.

## shallow understanding

When I work with agents, I also notice something about myself.

I do not build understanding the same way I used to.

Before, I had to follow the flow myself.
I had to open the files.
I had to trace the calls.
I had to suffer a little.

That suffering was useful.

Now the agent can grab code very quickly.

This is good, but also dangerous.

The agent may jump around the project and collect fragments. Then it builds an explanation from those fragments.

But some of that code may be unreachable.
Some of it may be old.
Some of it may be a weird edge path.
Some of it may not matter.

So the agent can create an understanding that looks broad, but is actually shallow.

It knows many pieces.
It does not necessarily know the flow.

## debugging by imagination

A common agent failure mode is debugging by imagination.

It reads code.
It explains what should happen.
It proposes a fix.

But it does not always check what actually happens.

Experienced developers do ugly things.

They add temporary logs.
They print variables.
They comment things out.
They write tiny scripts.
They create throwaway harnesses.
They call private functions directly.
They fake inputs.
They break stuff on purpose.
They run the program in stupid ways just to see what happens.

Create and destroy worlds.

Create mutants to your liking.

Here the goal justifies almost any means.

Mold the system, the code, the component to your needs.

Run it.
Test it.
Learn what is actually true.

## throwaway tools

Agents often need help creating throwaway tools.

Not production tools.
Not nice abstractions.
Not reusable libraries.

Just ugly little things used to learn something.

A script that calls one function.
A log line in the middle of a branch.
A temporary endpoint.
A fake input file.
A tiny harness around one parser.
A command that proves whether some path is reachable.

This is how you turn guessing into knowing.

But agents do not naturally do enough of it.

They tend to dry-run the code mentally.

Sometimes that is enough.

Often it is not.

## novelty

There is another thing I suspect.

Coding agents are trained on code where people do things in established ways.

So when you do something unusual, you may be fighting the model a little.

It tries to pull the code back toward the common path.

Usually that is good.

Common patterns are common for a reason.

But if you are deliberately doing something different, something new, or something specific to your system, the agent may keep “normalizing” it.

It may remove the interesting part.
It may replace your idea with a standard pattern.
It may treat novelty as a mistake.

This can make some kinds of programming harder.

Not impossible.

Just harder than expected.
