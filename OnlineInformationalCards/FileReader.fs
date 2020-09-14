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
                 CardPaths = cardPaths }
        else
            Error <| MetadataFileMissing deckDirectory)
    |> Seq.toList
