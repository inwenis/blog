# guidelines to write scrapers

1. always set timeouts on requests
1. use retires
1. pay attention to dst handling
1. don't scrape data from/to = today/today - always use more days
1. keep a header on top of the scraper with info about what is being scrpaed
1. log the url you're scraping from or make the request easily reproducible
1. when using headless browsers - use forgiving xpaths/selectors - don't be too strict
1. hedless vs mimicking requests - ?
    - I'm not sure what i prefer here
1. don't rely on indexes of csv - rely on header names
1. validate the data you scrape - is it what you expected to be?
1. be mindful of nulls/zeroes/etc
