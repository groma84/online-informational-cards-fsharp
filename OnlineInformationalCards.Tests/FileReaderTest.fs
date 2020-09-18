module FileReaderTest

open Expecto
open Expect
open FileReader
open AppErrors
open Swensen.Unquote
open FSharpPlus
open Types

[<Tests>]
let tests =
    testList
        "readFile"
        [ testCase "should error on non-existing file"
          <| fun () ->
              let nonExistingPath = "meh"
              let actual = readFile nonExistingPath

              let (actual': ApplicationError) =
                  wantError actual "expected Error but got Ok"

              match actual' with
              | FileReaderError msg ->
                  isTrue (msg.Contains("Could not find file")) (sprintf "wrong error message: %s" msg)
              | _ -> isTrue false "unexpected case"

          testCase "should read existing file"
          <| fun () ->
              let path = "data/file1.txt"
              let actual = readFile path

              let (actual': string) =
                  wantOk actual "expected Ok but got Error"

              test <@ actual' = "content" @> ]

[<Tests>]
let traverseDirectoryTests =
    let deckDirectory = DeckDirectory "data"

    testList
        "traverseDirectory"
        [ testCase "one deck is parsed"
          <| fun () ->
              let actual = traverseDirectory deckDirectory
              test <@ List.length actual = 1 @>

          testCase "deck has metadata file path"
          <| fun () ->
              let actual =
                  traverseDirectory deckDirectory |> List.head

              test <@ String.isSubString "deck.json" (wantOk actual "Expected Ok").MetadataPath @>

          testCase "deck has two card filepaths"
          <| fun () ->
              let actual =
                  traverseDirectory deckDirectory |> List.head

              test
                  <@ (wantOk actual "Expected Ok").CardsPaths
                     |> List.length = 2 @>

          testCase "filepaths contain correct filename"
          <| fun () ->
              let actual =
                  traverseDirectory deckDirectory |> List.head

              test
                  <@ match (wantOk actual "Expected Ok").CardsPaths with
                     | f :: s :: _ ->
                         String.isSubString "01.json" f
                         && String.isSubString "02.json" s
                     | _ -> true = false @> ]

[<Tests>]
let readDeckFilesTests =
    testList
        "readDeckFiles"
        [ testCase "read deck"
          <| fun () ->
              let metadataPath = "data/testdeck1/deck.json"
              let cardPath1 = "data/testdeck1/cards/01.json"
              let cardPath2 = "data/testdeck1/cards/02.json"

              let input =
                  Ok
                      { MetadataPath = metadataPath
                        CardsPaths = [ cardPath1; cardPath2 ] }

              let actual = readDeckFiles [ input ]
              test <@ List.length actual = 1 @>

              let actualValue = wantOk (List.head actual) "Expected Ok"

              test
                  <@ actualValue =
                      { MetadataJson = System.IO.File.ReadAllText metadataPath
                        CardsJson =
                            [ System.IO.File.ReadAllText cardPath1
                              System.IO.File.ReadAllText cardPath2 ] } @> ]
