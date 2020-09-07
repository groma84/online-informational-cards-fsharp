module Parsers

open Fleece.Newtonsoft
open Fleece.Newtonsoft.Operators
open Types
open AppErrors
open FSharpPlus

type DeckMetadata with

    static member Create id title description =
        { DeckMetadata.Id = id
          Title = title
          Description = description }

    static member OfJson json =
        match json with
        | JObject o ->
            DeckMetadata.Create
            <!> (o .@ "id")
            <*> (o .@ "title")
            <*> (o .@ "description")
        | x -> Decode.Fail.objExpected x

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


type Person = { Name: string; Age: int }

type Person with
    static member Create name age dob gender children = { Person.Name = name; Age = age }

    static member OfJson json =
        match json with
        | JObject o -> Person.Create <!> (o .@ "name") <*> (o .@ "age")
        | x -> Decode.Fail.objExpected x
