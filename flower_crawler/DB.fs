module DB

open FSharp.Data.Sql
open System.Collections.Generic
open Model

let [<Literal>] connString = "Host=localhost;Database=test_db;Username=postgres;Password=postgres"

let [<Literal>] dbVendor = Common.DatabaseProviderTypes.POSTGRESQL

type sql = SqlDataProvider<dbVendor,connString, "">

let ctx = sql.GetDataContext()

let Goods = ctx.Public.Goods

let Categorys = ctx.Public.Categorys


let saveCategorys (list: IEnumerable<string>) : unit = 
    Seq.map (fun text -> 
        let cate = Categorys.Create()
        cate.Name <- text
    ) list |> ignore
    ctx.SubmitUpdates()

let saveGoods (list: IEnumerable<Good>) : unit = 
    Seq.map (fun model -> 
        let goods = Goods.Create()
        goods.Title <- model.Title
        goods.Src <- model.Src
        goods.Desc <- model.Description
        goods.Content <- model.Content
        goods.Price <- model.Price
    ) list |> ignore
    ctx.SubmitUpdates()
