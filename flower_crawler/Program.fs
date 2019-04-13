
open DebugHelper
open Lib
open DB

open Fizzler.Systems.HtmlAgilityPack
open System

[<EntryPoint>]
let main argv =
    //[getCategorys; getGoods] 
    //[getCategorys] 
    [getGoods] 
    |> Async.Parallel
    |> Async.RunSynchronously 
    |> ignore
    //saveCategorys [| "text"; "222" |]
    0 
