module DB

open FSharp.Data.Sql
open System.Collections.Generic
open Model
open System

let [<Literal>] connString = "Host=localhost;Database=test_db;Username=bob;Password=bob"

let [<Literal>] dbVendor = Common.DatabaseProviderTypes.POSTGRESQL

type sql = SqlDataProvider<dbVendor,connString, "", "./libs" >

let runtimeConnectionStr = connString

let ctx = sql.GetDataContext(runtimeConnectionStr)

let Goods = ctx.Public.Goods

let Categorys = ctx.Public.Categorys

let Subjects = ctx.Public.Subjects


let saveCategorys (list: IEnumerable<string>) : unit = 
    Seq.iter (fun (text: string) -> 
        let cate = Categorys.Create()
        cate.Name <- text
    ) list
    ctx.SubmitUpdates()

let saveGoods (list: IEnumerable<Good>) : unit = 
    Seq.iter (fun (model: Good) -> 
        let goods = Goods.Create()
        goods.Title <- model.Title
        goods.Src <- model.Src
        goods.Desc <- model.Description
        goods.Content <- model.Content
        goods.Price <- model.Price
        goods.Type <- model.Type
        goods.CategoryId <- 2
    ) list |> ignore
    ctx.SubmitUpdates()

let saveSubjects (list: IEnumerable<Subject>) : unit = 
    Seq.iter (fun (model: Subject) -> 
        let subj = Subjects.Create()
        subj.Title <- model.Title
        subj.Src <- model.Src
        subj.Content <- model.Content
    ) list |> ignore
    ctx.SubmitUpdates()