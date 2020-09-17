module DeckReader

open FSharpPlus
open Types
open Parsers
open AppErrors
open Thoth.Json.Net

let readDecks allDeckStrings: Result<Deck, ApplicationError list> list =
    let parseOneDeck (deckStrings: DeckStrings): Result<Deck, ApplicationError list> =
        let parse (decoder: Decoder<'a>) (json: string): Result<'a, ApplicationError> =
            json
            |> ((Decode.fromString decoder)
                >> Result.mapError ParserError)

        let parseOneCard (cardFile: string): Result<Card, ApplicationError> = parse Card.Decoder cardFile

        let metadata =
            parse DeckMetadata.Decoder deckStrings.MetadataJson

        let cards =
            List.map parseOneCard deckStrings.CardsJson

        let (cardOks, cardErrors) = Result.partition cards

        match (metadata, List.isEmpty cardErrors) with
        | Ok md, true -> Ok { Metadata = md; Cards = cardOks }
        | Ok _, false -> Error cardErrors
        | Error err, _ -> Error <| err :: cardErrors

    let mapInputErrorsToListOfErrorsAndParseDeck =
        (Result.mapError (fun err -> [ err ]))
        >> (Result.bind parseOneDeck)

    List.map mapInputErrorsToListOfErrorsAndParseDeck allDeckStrings

let decksToDictionary (decks: Result<Deck, ApplicationError list> list)
                      : Result<Map<System.Guid, Deck>, ApplicationError list> =
    let (decksOk, decksError) = Result.partition decks

    if (List.isEmpty decksError) then
        let deckToKeyValuePair deck = (deck.Metadata.Id, deck)

        decksOk
        |> List.map deckToKeyValuePair
        |> Map.ofList
        |> Ok

    else
        (List.collect id decksError) |> Error
