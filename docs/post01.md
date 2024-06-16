# F# async - be mindful of what you put in async {}

```F#
open System

let r = Random()

let m () =
  let random_num = r.Next()
  async {
    printfn "%i" random_num
  }

m () |> Async.RunSynchronously // prints a random number
m () |> Async.RunSynchronously // prints another random number
let x = m ()
x |> Async.RunSynchronously // prints another random number
x |> Async.RunSynchronously // prints same number as above
```

Why does it matter that lines 14 and 15 print the same number?

Let's consider the following code:

```F#
// We're sending http requests and if they fail we'd like to retry them

#r "System.Net.Http"
open System.Net.Http

let HTTP_CLIENT = new HttpClient()

let send url =
  let httpRequest = new HttpRequestMessage()
  httpRequest.RequestUri <- Uri url

  async {
    let! r =
      HTTP_CLIENT.SendAsync httpRequest
      |> Async.AwaitTask
    return r
  }

send "http://test" |> Async.RunSynchronously
send "http://test" |> Async.RunSynchronously
let y = send "http://test"
y |> Async.RunSynchronously
y |> Async.RunSynchronously

let retry computation =
  async {
    try
      let! r = computation
      return r
    with
    | e ->
      printf "ups, err, let's retry"
      let! r2 = computation
      return r2
  }

send "http://test" |> retry |> Async.RunSynchronously
// retrying will fail always with "The request message was already sent. Cannot send the same request message multiple times."
// This is because just like L14/15 print the same number, here we send the exact same request object and that's not allowed
```

The fix
```F#
let send2 url =
  async {
    let httpRequest = new HttpRequestMessage()
    httpRequest.RequestUri <- Uri url
    let! r =
      HTTP_CLIENT.SendAsync httpRequest
      |> Async.AwaitTask
    return r
  }

send2 "http://test" |> retry |> Async.RunSynchronously
```