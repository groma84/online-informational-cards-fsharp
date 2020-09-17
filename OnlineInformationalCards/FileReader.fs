module FileReader

open AppErrors
open Types
open System.IO
open FSharpPlus

let readFile filePath =
    try
        System.IO.File.ReadAllText filePath |> Ok
    with exn -> FileReaderError exn.Message |> Error

let traverseDirectory filePath =
    System.IO.Directory.EnumerateDirectories filePath
    |> Seq.map (fun deckDirectory ->
        let metadataFile =
            Path.Combine [| deckDirectory
                            "deck.json" |]

        if File.Exists metadataFile then
            let cardPaths =
                Path.Combine [| deckDirectory
                                "cards" |]
                |> Directory.EnumerateFiles
                |> Seq.map Path.GetFullPath
                |> Seq.toList

            Ok
            <| { MetadataPath = metadataFile
                 CardsPaths = cardPaths }
        else
            Error <| MetadataFileMissing deckDirectory)
    |> Seq.toList

let readDeckFiles (filePaths: Result<DeckFiles, ApplicationError> list): Result<DeckStrings, ApplicationError list> list =
    let readOneDeck (deckFiles: DeckFiles): Result<DeckStrings, ApplicationError list> =
        let metadata = readFile deckFiles.MetadataPath
        let cards = List.map readFile deckFiles.CardsPaths
        let (cardOks, cardErrors) = Result.partition cards

        match (metadata, List.isEmpty cardErrors) with
        | Ok md, true ->
            Ok
                { MetadataJson = md
                  CardsJson = cardOks }
        | Ok _, false -> Error cardErrors
        | Error err, _ -> Error <| err :: cardErrors

    let mapInputErrorsToListOfErrorsAndParseDeck =
        Result.mapError (fun err -> [ err ])
        >> (Result.bind readOneDeck)

    List.map mapInputErrorsToListOfErrorsAndParseDeck filePaths
