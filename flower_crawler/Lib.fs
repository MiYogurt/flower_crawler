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
    "https://www.hua.com" + item.Attributes.["href"].Value

let getAttr (name: string) (item: HtmlNode) : string = 
    item.Attributes.[name].Value

let joinSrc (list: IEnumerable<string>) : string = 
    let mutable str = ""
    for s in list do
        str <- str + s + ","
    str

let getSrc = getAttr "src"    

let rn = new Random(DateTime.Now.Millisecond)

let getGoodsModel (item: HtmlNode): Good = 
    let Title = item.QuerySelector("h3.product-title").InnerText
    let Content = item.QuerySelector("#Details").InnerHtml
    let Price = (rn.NextDouble().ToString("f2") |> Double.Parse) * 100.0 + 100.0
    let Src = item.QuerySelectorAll(".swiper-slide img") ||> getSrc |> joinSrc
    let Description = item.QuerySelectorAll("dl dd") |> Seq.item 3 |> fun x -> x.InnerText 
    let Type = item.QuerySelectorAll("dl dd") |> Seq.item 2 |> fun x -> x.InnerText 
    
    {
        Title = Title
        Content = Content
        Price = float Price
        Src = Src
        Description = Description
        Type = Type
    }

let getGoods = 
    async {
        let! doc = getDoc "https://www.hua.com/aiqingxianhua"
        let goodsDom = doc.QuerySelectorAll(".grid-wrapper .grid-item .title a")
        let! docs = goodsDom ||> getGoodLink ||> getDoc |> Async.Parallel
        docs ||> getGoodsModel |> saveGoods |> ignore
    }
