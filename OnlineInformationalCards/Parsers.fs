module Parsers

open Fleece.Newtonsoft
open Fleece.Newtonsoft.Operators
open Types
open AppErrors

type DeckMetadata with
    static member OfJson json =
        match json with
        | JObject o ->
            let description = o .@ "description"
            let title = o .@ "title"
            let id = o .@ "id"
            match description, title, id with
            | Decode.Success description, Decode.Success title, Decode.Success id ->
                Decode.Success
                    { DeckMetadata.Description = description
                      Title = title
                      Id = id }
            | x ->
                Error
                <| ParserError(sprintf "Error parsing DeckMetadata: %A" x)
        | x ->
            Error
            <| ParserError(sprintf "Error parsing DeckMetadata - object expected: %A" x)

type Card with
    static member OfJson json =
        match json with
        | JObject o ->
            let id = o .@ "id"
            let text = o .@ "text"
            match id, text with
            | Decode.Success id, Decode.Success text -> Decode.Success { Card.Id = id; Text = text }
            | x ->
                Error
                <| ParserError(sprintf "Error parsing Card: %A" x)
        | x ->
            Error
            <| ParserError(sprintf "Error parsing Card - object expected: %A" x)
