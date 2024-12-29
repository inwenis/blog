

// More on FSharp.Data

let j = FSharp.Data.JsonValue.Parse("{}")
j.Properties()


let d =
    JsonValue.Record [|
        "event",      JsonValue.String "asdf"
        "properties", JsonValue.Record [|
            "token",       JsonValue.String "tokenId"
            "distinct_id", JsonValue.String "123123"
        |]
    |]

d.ToString().Replace("\r\n", "").Replace(" ", "")


// https://stackoverflow.com/questions/48113281/how-to-create-nested-json-objects-in-fsharp
// maybe update answer here