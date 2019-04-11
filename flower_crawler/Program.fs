
open DebugHelper
open Lib

open Fizzler.Systems.HtmlAgilityPack



[<EntryPoint>]
let main argv =
    [getCategorys; getGodos] 
    |> Async.Parallel
    |> Async.RunSynchronously 
    |> ignore
    0 
