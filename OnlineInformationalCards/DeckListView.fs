module DeckListView

open System
open Falco.Markup
open FSharpPlus
open Types

let view (decks: DeckMetadata list) =
    let oneDeck metadata =
        Elem.li [] [
            Elem.h2 [] [ Text.raw metadata.Title ]
            Elem.p [] [
                Text.raw
                <| Option.defaultWith (fun () -> "") metadata.Description
            ]
            Elem.a [ Attr.href
                     <| (sprintf "deck/%s" <| metadata.Id.ToString()) ] [
                Text.raw <| metadata.Id.ToString()
            ]
        ]

    Elem.div [] [
        Elem.h1 [] [
            Text.raw "Alle Kartensätze"
        ]
        Elem.ul [] (List.map oneDeck decks)
    ]
