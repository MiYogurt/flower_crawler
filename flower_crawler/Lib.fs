module Lib

open System
open DebugHelper
open Model
open DB
open Base
open HtmlAgilityPack
open Fizzler.Systems.HtmlAgilityPack
open System.Net.Http
open System.Collections.Generic

let getText : (IEnumerable<HtmlNode> -> IEnumerable<string>) = Seq.map (fun (node: HtmlNode) -> node.InnerText)

let getDoc (url: string) =
    async {
        let client = new HttpClient()
        let! response = client.GetAsync(url) |> Async.AwaitTask
        let htmlDoc = new HtmlDocument()
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        htmlDoc.LoadHtml content
        return htmlDoc.DocumentNode
    }


let getCategorys =
    async {
        let! doc = getDoc "https://www.hua.com/"
        let aList = doc.QuerySelectorAll(".cate-list a") |> getText
        aList |> saveCategorys
    }



let getGoodLink (item: HtmlNode) : string = 
    "https://www.hua.com/aiqingxianhua" + item.Attributes.["herf"].Value

let getAttr (name: string) (item: HtmlNode) : string = 
    item.Attributes.[name].Value

let joinSrc (list: IEnumerable<string>) : string = 
    let mutable str = ""
    for s in list do
        str <- str + "," + s
    str

let getSrc = getAttr "src"    

let getGoodsModel (item: HtmlNode): Good = 
    {
        Title = item.QuerySelector(".product-title").InnerText
        Content = item.QuerySelector("#Details").InnerHtml
        Price = item.QuerySelector(".price-sell .price-num").InnerText |> System.Double.Parse
        Src = item.QuerySelectorAll(".swiperController img") ||> getSrc |> joinSrc
        Description = item.QuerySelector(".title ~ .attribute").ChildNodes.[4].QuerySelector("dd").InnerText
    }

let getGoods = 
    async {
        let! doc = getDoc "https://www.hua.com/aiqingxianhua"
        let goodsDom = doc.QuerySelectorAll ".grid-wrapper .grid-item a"
        goodsDom 
            ||> getGoodLink 
            ||> getDoc
            |> Async.Parallel
            |> Async.RunSynchronously
            ||> getGoodsModel
            |> saveGoods
            |> ignore

        ()
    }
