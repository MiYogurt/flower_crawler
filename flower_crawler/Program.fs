
open DebugHelper
open Lib
open DB

open Fizzler.Systems.HtmlAgilityPack
open System

[<EntryPoint>]
let main argv =
    //[getCategorys; getGoods] 
    //[getCategorys] 
    //[getGoods] 
    [getSubjects] 
    |> Async.Parallel
    |> Async.RunSynchronously 
    |> ignore
    0 
