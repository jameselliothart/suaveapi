namespace suaveapi.Rest

open System.Text.Json
open Suave
open Suave.Successful
open Suave.Operators
open Suave.Filters

[<AutoOpen>]
module RestFul =

    let JSON v =
        let jsonSerializerSettings = JsonSerializerOptions()
        jsonSerializerSettings.PropertyNamingPolicy <- JsonNamingPolicy.CamelCase
        jsonSerializerSettings.WriteIndented <- true

        JsonSerializer.Serialize(v, jsonSerializerSettings)
        |> OK
        >=> Writers.setMimeType "application/json; charset=utf-8"

    type RestResource<'a> = {
        GetAll : unit -> 'a seq
    }

    let rest resourceName resource =
        let resourcePath = "/" + resourceName
        let getAll = warbler (fun _ -> resource.GetAll () |> JSON)
        path resourcePath >=> GET >=> getAll