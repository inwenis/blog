open System
open System.IO
open System.Text.Json

#r "nuget: FSharp.Data"
open FSharp.Data

fsi.AddPrinter<DateTimeOffset>(fun dt -> dt.ToString("O"))

Environment.CurrentDirectory <- __SOURCE_DIRECTORY__ // ensures the script runs from the directory it's located in

// -------------------------------------------------------------------------
