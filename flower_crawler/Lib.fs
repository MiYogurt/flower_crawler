module Lib

open DebugHelper
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
        print aList
    }

type Good = 
    {
       Title: string
       Src: string
       Description: string
    }

let getGood (item: HtmlNode) : Good = 
    {
        Title = item.QuerySelector(".product-title").InnerText
        Src = item.QuerySelector(".img-box img").Attributes.["src"].Value
        Description = item.QuerySelector(".feature").InnerText
    }

let getGodos = 
    async {
        let! doc = getDoc "https://www.hua.com/flower/?r=0&pg=1"
        let goodsDom = doc.QuerySelectorAll(".grid-wrapper .grid-item")
        let goods = goodsDom |> Seq.map getGood

        goods |> Seq.map (fun (g: Good) -> g.Title + " --- >"+g.Src) |> print

        ()
    }
