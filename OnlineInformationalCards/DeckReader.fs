module DeckReader

open FSharpPlus
open Types
open Parsers
open AppErrors
open Thoth.Json.Net

let readDecks allDeckStrings: List<Result<Deck, ApplicationError list>> =
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
