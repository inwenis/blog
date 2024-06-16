post 4

Lasse at work had this issue wwhere someone wrote a scraper and used parallelism to make it faster. The data tho didn't seem to be time critical so the parallelism seems unnecessary. It also caused the scraper to hit the API many time, some requests would fail. The other error in this scraper was the error handling of basically swallowing all errors with a msg "no data found"


take away - don't preoptimize
    - don't handle more erorrs than you can
