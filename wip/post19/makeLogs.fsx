

type RecordTest = {
    Timestamp: DateTimeOffset
    Level: string
    }

let r = { Timestamp = DateTimeOffset.Now; Level = "ERROR" }

JsonSerializer.Serialize(r) |> JsonSerializer.Deserialize<RecordTest>

let tupleT = (DateTimeOffset.Now, "ERROR")
type TupleAlias = string * float
let tuple:TupleAlias = ("asdf", 1.0)
JsonSerializer.Serialize(tuple) |> JsonSerializer.Deserialize<TupleAlias>
JsonSerializer.Serialize((1,2)) |> JsonSerializer.Deserialize<int*int>

type UnionTest =
    | A of int
    | B of string

let u = A 1

JsonSerializer.Serialize(u) // eeeeeeeeeeeeee !

type RecordTest2 = {
    Timestamp: DateTimeOffset
    Level: string
    TestOp: int option
    }

let r2 = { Timestamp = DateTimeOffset.Now; Level = "ERROR"; TestOp = Some 1 }
JsonSerializer.Serialize(r2) |> JsonSerializer.Deserialize<RecordTest2>
let r3 = { Timestamp = DateTimeOffset.Now; Level = "ERROR"; TestOp = None }
JsonSerializer.Serialize(r3) |> JsonSerializer.Deserialize<RecordTest2>


#r "nuget: FSharp.Json"

open FSharp.Json

type TheUnion =
| OneFieldCase of string
| ManyFieldsCase of string*int

let data = OneFieldCase "The string"

let json = Json.serialize data
// json is """{"OneFieldCase":"The string"}"""

let deserialized = Json.deserialize<TheUnion> json
// deserialized is OneFieldCase("The string")




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