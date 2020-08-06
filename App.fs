// Learn more about F# at http://fsharp.org

open System
open Suave.Web
open Suave.Successful

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig (OK "Hello, Suave!")
    0 // return an integer exit code
