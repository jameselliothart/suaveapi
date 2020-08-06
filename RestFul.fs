namespace suaveapi.Rest

open Suave
open Suave.Successful
open Suave.Operators
open Suave.Filters
open System.Text
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

[<AutoOpen>]
module RestFul =

    let JSON v =
        let jsonSerializerSettings = JsonSerializerSettings()
        jsonSerializerSettings.ContractResolver <- CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(v, jsonSerializerSettings)
        |> OK
        >=> Writers.setMimeType "application/json; charset=utf-8"

    let fromJson<'a> (json) =
        JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a

    let getResourceFromReq<'a> (req : HttpRequest) =
        let getString (rawForm : byte[]) =
            Encoding.UTF8.GetString(rawForm)
        req.rawForm |> getString |> fromJson<'a>

    type RestResource<'a> = {
        GetAll : unit -> 'a seq
        Create : 'a -> 'a
    }

    let rest resourceName resource =
        let resourcePath = "/" + resourceName
        let getAll = warbler (fun _ -> resource.GetAll () |> JSON)
        path resourcePath >=> choose [
            GET >=> getAll
            POST >=> request (getResourceFromReq >> resource.Create >> JSON)
        ]