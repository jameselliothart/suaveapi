﻿// Learn more about F# at http://fsharp.org

open System
open Suave.Web
open Suave.Successful
open suaveapi.Rest
open suaveapi.Db

[<EntryPoint>]
let main argv =
    let personWebPart = rest "people" {
        GetAll = Db.getPeople
    }
    startWebServer defaultConfig personWebPart
    0 // return an integer exit code
