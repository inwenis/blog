open System
open System.IO
open System.Text.Json

#r "nuget: FSharp.Data"
open FSharp.Data

fsi.AddPrinter<DateTimeOffset>(fun dt -> dt.ToString("O"))

Environment.CurrentDirectory <- __SOURCE_DIRECTORY__ // ensures the script runs from the directory it's located in

// -------------------------------------------------------------------------


type LogEntry = {
    Timestamp: DateTime
    Level: string
    Message: string
    RedundantContent: string
}

let random = Random()
let levels = [| "INFO"; "WARN"; "ERROR"; "DEBUG" |]
let messages = [| "User logged in"; "User logged out"; "File not found"; "Access denied"; "Operation completed"; "Operation failed" |]

let generateLogEntry () =
    {
        Timestamp        = DateTime.Now.AddSeconds(-random.Next(0, 10000))
        Level            = levels.[random.Next(levels.Length)]
        Message          = messages.[random.Next(messages.Length)]
        RedundantContent = String.replicate(random.Next(10, 100)) "x"
    }

List.init 6_000_000 (fun _ -> generateLogEntry()) // 6M lines is around 1GB of data
|> List.map (fun entry -> JsonSerializer.Serialize(entry))
|> fun lines -> File.WriteAllLines("./logs.json", lines)


type JsonProviderLogEntry = JsonProvider<"""
{
    "Timestamp"        : "2024-12-23T20:51:18.2020753+01:00",
    "Level"            : "ERROR",
    "Message"          : "File not found",
    "RedundantContent" : "x"
}""">

let runWithMemoryCheck f =
    GC.Collect()
    let before = GC.GetTotalMemory(true)
    let x = f()
    GC.Collect()
    let after = GC.GetTotalMemory(true)
    let m = ((after - before) |> float) / 1024. / 1024. / 1024. // GB
    x, m

#time
// -------------------------------------------------------------------------

runWithMemoryCheck (fun () ->
    File.ReadAllLines("./logs.json")
    |> Array.map (fun line -> JsonProviderLogEntry.Parse(line))
) |> snd |> printfn "Memory used: %f GB"

runWithMemoryCheck (fun () ->
    use s = File.OpenText("./logs.json")
    [
        while not s.EndOfStream do
            let line = s.ReadLine()
            let parsed = JsonProviderLogEntry.Parse(line)
            yield parsed
    ]
) |> snd |> printfn "Memory used: %f GB"

// only the properties we're interested in
type LogEntryRecord = {
    Timestamp: DateTimeOffset
    Level: string
    }

let try3 () =
    use f = File.OpenText("./logs.json")
    [
        while not f.EndOfStream do
            let line = f.ReadLine()
            let parsed = JsonSerializer.Deserialize<LogEntryRecord>(line)
            yield parsed
    ]

runWithMemoryCheck (fun () -> try3()) |> snd |> printfn "Memory used: %f GB"

// try using JsonNode and JsonDocument
open System.Text.Json.Nodes
let try4 () =
    use f = File.OpenText("./logs.json")
    [
        while not f.EndOfStream do
            let line = f.ReadLine() |> JsonNode.Parse
            let t = line.["Timestamp"].GetValue<DateTimeOffset>()
            let l = line.["Level"].GetValue<string>()
            yield { Timestamp = t; Level = l }
    ]
runWithMemoryCheck (fun () -> try4()) |> snd |> printfn "Memory used: %f GB"

// this seems faster than JsonNode
let try5 () =
    use f = File.OpenText("./logs.json")
    [
        while not f.EndOfStream do
            use line = f.ReadLine() |> JsonDocument.Parse
            let t = line.RootElement.GetProperty("Timestamp").GetDateTimeOffset()
            let l = line.RootElement.GetProperty("Level").GetString()
            yield { Timestamp = t; Level = l }
    ]
runWithMemoryCheck (fun () -> try5()) |> snd |> printfn "Memory used: %f GB"


let try6 () =
    use f = File.OpenText("./logs.json")
    [
        while not f.EndOfStream do
            let line = f.ReadLine() |> FSharp.Data.JsonValue.Parse
            let t = line.GetProperty("Timestamp").AsDateTimeOffset()
            let l = line.GetProperty("Level").AsString()
            yield { Timestamp = t; Level = l }
    ]
runWithMemoryCheck (fun () -> try6()) |> snd |> printfn "Memory used: %f GB"

// JsonSerializer -> static class

let jsonString = ""
// JsonSerializer.Deserialize<'Type>(jsonString)
let options = new JsonSerializerOptions()
options.WriteIndented <- true
JsonSerializer.Deserialize<LogEntryRecord>(jsonString, options)
// JsonSerializer.DeserializeAsync(stream, ...) <- only stream here cuz parsing string has no async operation

// JsonSerializer.Serialize

// JsonSerializer.SerializeToUtf8Bytes(value, options) <- why does this one make sense?
// cuz strings in .Net are UTF-16, so if you don't need to convert to string, you can use this method
// and save memory and maybe time
// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to#serialize-to-utf-8
// 5-10 % faster

// Serialization behavior
// By default, all public properties are serialized. You can specify properties to ignore. You can also include private members.
// By default, JSON is minified. You can pretty-print the JSON.
// By default, casing of JSON names matches the .NET names. You can customize JSON name casing.
// By default, fields are ignored. You can include fields.


// Deserialization behavior
// The following behaviors apply when deserializing JSON:

// By default, property name matching is case-sensitive. You can specify case-insensitivity.
// Non-public constructors are ignored by the serializer.
// Deserialization to immutable objects or properties that don't have public set accessors is supported but not enabled by default. See Immutable types and records.

// TODO - understand this - Utf8JsonReader

// nice - https://stu.dev/a-look-at-jsondocument/


// JsonNode and JsonDocument
// JsonNode -> mutable
open System.Text.Json.Nodes
let x = JsonNode.Parse("""{"x":[1,2,3],"Timestamp": "2024-12-23T20:51:18.2020753+01:00", "Level": "ERROR", "Message": "File not found", "RedundantContent": "xxxxxxxxxx"}""")
x.ToJsonString()
x.["Timestamp"].GetValue<DateTimeOffset>()
x.["Timestamp"].GetPath()

// how to do the equivalent of this int temperatureInt = (int)forecastNode!["Temperature"]!; in f#?

x["Timestamp"].GetType() |> string // json value
x.GetType() |> string // json obj
x["x"].GetValue<int[]>() // err
x["x"].AsArray() |> Seq.map (fun a -> a.GetValue<int>()) // ok
x["x"].[0].GetValue<int>() // ok

// create json
let myObj = new JsonObject()
myObj["asdf"] <- DateTimeOffset.Now
myObj.ToJsonString()
myObj["asdf"] <- new JsonArray(1,2)
myObj.Remove("asdf")

let y = JsonNode.Parse("""{"x":{"y":[1,2,3]},"Timestamp": "2024-12-23T20:51:18.2020753+01:00", "Level": "ERROR", "Message": "File not found", "RedundantContent": "xxxxxxxxxx"}""")
y.["x"] // this is a node
y.["x"].AsObject() // this is a json object
y.["x"].AsObject() |> Seq.map (fun x -> printfn "%A" x)
y.["x"].AsObject().ToJsonString() // you can serialize subsection of the json

JsonNode.DeepEquals(x, y) // comparing json

// JsonDocument -> immutable
// just read the docs here - https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-dom
// json document - faster, not mutable


// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/?ref=stu.dev
// nice
// https://blog.ploeh.dk/2023/12/18/serializing-restaurant-tables-in-f/

// The System.Text.Json namespace contains all the entry points and the main types.
// The System.Text.Json.Serialization namespace contains attributes and APIs for advanced scenarios and customization specific to serialization and deserialization.
// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-dom#json-dom-choices


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

// I could also test this
let j = FSharp.Data.JsonValue.Parse("{}")
j.Properties()
