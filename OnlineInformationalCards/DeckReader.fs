module DeckReader

open FSharpPlus
open Types
open Parsers
open AppErrors
open Thoth.Json.Net

let readDecks path: List<Result<Deck, ApplicationError list>> =
    let paths = FileReader.traverseDirectory path

    let parseOneDeck (deckFiles: DeckFiles): Result<Deck, ApplicationError list> =
        let parse (decoder: Decoder<'a>) (file: string): Result<'a, ApplicationError> =
            file
            |> FileReader.readFile
            >>= ((Decode.fromString decoder)
                 >> Result.mapError ParserError)

        let parseOneCard (cardFile: string): Result<Card, ApplicationError> = parse Card.Decoder cardFile

        let metadata =
            parse DeckMetadata.Decoder deckFiles.MetadataPath

        let cards =
            List.map parseOneCard deckFiles.CardPaths

        let (cardOks, cardErrors) = Result.partition cards

        match (metadata, List.isEmpty cardErrors) with
        | Ok md, true -> Ok { Metadata = md; Cards = cardOks }
        | Ok _, false -> Error cardErrors
        | Error err, _ -> Error <| err :: cardErrors

    let mapInputErrorsToCorrectErrorTypeAndParseDeck =
        (Result.mapError (fun err -> [ err ]))
        >> (Result.bind parseOneDeck)

    List.map mapInputErrorsToCorrectErrorTypeAndParseDeck paths
