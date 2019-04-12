
open DebugHelper
open Lib

open Fizzler.Systems.HtmlAgilityPack

[<EntryPoint>]
let main argv =
    //[getCategorys; getGoods] 
    [getCategorys] 
    //[getGoods] 
    |> Async.Parallel
    |> Async.RunSynchronously 
    |> ignore
    0 
