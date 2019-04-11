module DebugHelper

open System
open System.Collections.Generic


let print (list: IEnumerable<string>) =
    for item in list do
         Console.WriteLine(item.ToString())