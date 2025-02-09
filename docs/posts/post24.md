---
draft: false
date: 2025-02-09
categories:
  - F Sharp
---


# Some neat fsx F\#

My company had a hackathon focused on data scraping/processing.

Each team had to scrape 3 endpoints. I came up with something similar to this:
```F#
open System
open System.Net.Http
open System.Text

let c = new HttpClient()
c.Timeout <- TimeSpan.FromSeconds(5.0)

let lockObject = new obj()
let printSync text =
    let now = DateTimeOffset.Now.ToString("O")
    lock lockObject (fun _ -> printfn "[%s] %s" now text)

let s = new HttpClient()
s.Timeout <- TimeSpan.FromSeconds(5.0)
s.DefaultRequestHeaders.Add("X-Sender", "this is me, Mario!")
let sendToDestination stream response = async {
    let template = """{
    "CreatedAt": "xxXCreatedAtXxx",
    "Stream": "xxXStreamXxx",
    "Data": [
        xxXDataXxx
    ]
}"""
    let payload = template.Replace("xxXCreatedAtXxx", DateTimeOffset.Now.ToString("O"))
                          .Replace("xxXStreamXxx", stream)
                          .Replace("xxXDataXxx", response)
    let! response = s.PostAsync("http://localhost:8080", new StringContent(payload, Encoding.UTF8, "application/json") ) |> Async.AwaitTask
    sprintf "%s done sending response code %A" stream response.StatusCode |> printSync
}

let scraper (url:string) stream = async {
    while true do
        try
            let! response = c.GetStringAsync(url) |> Async.AwaitTask
            do! sendToDestination stream response
            sprintf "scraped %40s sendTo %s" url stream |> printSync
        with
        | _ -> sprintf "failed to scrape/or send %40s" url |> printSync

        do! Async.Sleep 1000
}

let urls = [
    "https://jsonplaceholder.typicode.com/posts", "123"
    "https://jsonplaceholder.typicode.com/posts", "124"
    "https://jsonplaceholder.typicode.com/posts", "125"
]

urls
|> List.map (fun (url, stream) -> scraper url stream)
|> Async.Parallel
|> Async.Ignore
|> Async.Start

// Async.CancelDefaultToken()

```

Things to keep in mind:

  - always have a try/catch all exceptions in async/tasks/threads
    - you don't want your thread to die without you knowing
  - always set a timeout when scraping (default timeout in .NET is 100s which is excessive for this script)

A minimalistic http server to listen to our scrapers:

```F#
open System.Net
open System.Text

// https://sergeytihon.com/2013/05/18/three-easy-ways-to-create-simple-web-server-with-f/
// run with `fsi --load:ws.fsx`
// visit http://localhost:8080

let host = "http://localhost:8080/"

let listener (handler:(HttpListenerRequest->HttpListenerResponse->Async<unit>)) =
    let hl = new HttpListener()
    hl.Prefixes.Add host
    hl.Start()
    let task = Async.FromBeginEnd(hl.BeginGetContext, hl.EndGetContext)
    async {
        while true do
            let! context = task
            Async.Start(handler context.Request context.Response)
    } |> Async.Start

listener (fun req response ->
    async {
        response.ContentType <- "text/html"
        let bytes = UTF8Encoding.UTF8.GetBytes("thanks!")
        response.OutputStream.Write(bytes, 0, bytes.Length)
        response.OutputStream.Close()
    })
```
