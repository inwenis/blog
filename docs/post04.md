# Short post on premature optimization and error handling

2023-05-09

*Premature optimization is the root of all evil*
~some wise guy long ago

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
