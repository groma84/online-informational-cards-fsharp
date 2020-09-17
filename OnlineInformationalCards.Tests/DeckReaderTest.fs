module DeckReaderTest

open Expecto
open Expect
open Swensen.Unquote
open FSharpPlus
open Types
open DeckReader

[<Tests>]
let deckReaderTest =
    testList
        "readDecks"
        [ testCase "x"
          <| fun () ->
              let input =
                  Ok
                      { DeckStrings.MetadataJson = """{
    "id": "de2ffa7c-98b5-4d5f-a945-74f016a9b972",
    "title": "testdeck1",
    "description": "a test deck"
}"""
                        CardsJson =
                            [ """{
    "id": 1,
    "text": "A fancy card text.\nNew line!"
}"""
                              """{
    "id": 2,
    "text": "Another fancy card text."
}""" ]                }

              let actual = readDecks [ input ]
              test <@ List.length actual = 1 @>

              let actualValue = wantOk (List.head actual) "Expected Ok"

              test
                  <@ actualValue =
                      { Deck.Metadata =
                            { Id = System.Guid.Parse("de2ffa7c-98b5-4d5f-a945-74f016a9b972")
                              Title = "testdeck1"
                              Description = Some "a test deck" }
                        Cards =
                            [ { Card.Id = 1
                                Text = "A fancy card text.\nNew line!" }
                              { Card.Id = 2
                                Text = "Another fancy card text." } ] } @> ]
