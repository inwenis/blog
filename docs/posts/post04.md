---
draft: false
date: 2023-05-09
categories:
  - General coding
---

# Short post on premature optimization and error handling

*Premature optimization is the root of all evil*
~Donald Knuth

*Don't pretend to handle more than you handle* ~me just now

My team took over some scrapers. After a few months an issue is reported stating that accounting is missing data from one of the scrapers. No errors were logged and or seen by our team.

My colleague investigates the issue. Findings:

- most recent data is indeed missing in our data base (it is already available in the API)
- the data is often delayed (compared to when it's available in the remote API)
- the data is not time critical but a delay of hours or days is vexing (remember folks - talk to your users or customers)
- the scraper is using parallelism to send all requests at once (probably to get the data faster)
- the API doesn't like our intense scraping and bans us from accessing the API, sometimes for hours
- we never saw any Errors as the error handling looks like this:

```
try {
    data = hit the REST API using multiple parallel requests
    persist(data)
} catch {
    log.info("No data found")
}
```

## Take away

- talk to your users - in this case to find our that data doesn't need to arrive seconds after being published
- don't optimize prematurely
- don't catch all exception pretending you handle them

## More venting

I have seen my share of premature optimizations. AFAIR I always managed to have a conversation about the (un)necessity of an optimization and agree to prefer readability/simplicity over premature optimization.

If you see premature optimization my advise is "talk to the perp". People do what they consider necessary and right. They might optimize code hoping to save the company money or time.

If you have the experience to know that saving 2kB of RAM in an invoicing app run once a month is not worth using that obscure data structure - talk to those who don't yet know it. Their intentions are good.

I'm pretty sure I'm also guilty of premature optimization, just can't recall any instance as my brain is probably protecting my ego by erasing any memories of such mistakes from my past.

## An example

One example of premature optimization stuck with me. I recall reviewing code as below


```C#
foreach(var gasConnectionPoint in gasPoints)
{
    if (gasConnectionPoint.properties.Any())
    {
        foreach (var x in gasConnectionPoint.properties.properties)
        {
            // do sth with x
        }
    }
}
```

The review went something like this:

> **me**: drop the if, foreach handles empty collections just fine
>
> **author**: but this is better
>
> **me**: why?
>
> **author**: if the collection is empty we don't even use the iterator
>
> **me**: how often is this code run?
>
> **author**: currently only for a single entry in gasPoints, but there can be more
>
> **me**: how many more and when?
>
> **author**: users might create an entry for every gas pipeline connection in Europe
>
> **me**: ok, how many is that?

We agreed to drop the if after realizing that:

> We have ~30 countries in Europe, even if they all connect with each other there will be at most ~400 gas connections to handle here. We don't know that the if is faster then the iterator. 400 is extremely optimistic. We have 1 entry now, and realistically we will have 10 gasPoints in 5 years.

The conversation wasn't as smooth as I pretend here but we managed.

## Links

[https://wiki.c2.com/?PrematureOptimization](https://wiki.c2.com/?PrematureOptimization)

[https://wiki.c2.com/?ProfileBeforeOptimizing](https://wiki.c2.com/?ProfileBeforeOptimizing)

[https://youtube.com/CodeAesthetics/PrematureOptimization](https://www.youtube.com/watch?v=tKbV6BpH-C8)
