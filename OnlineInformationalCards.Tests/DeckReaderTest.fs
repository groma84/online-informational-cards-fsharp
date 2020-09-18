module DeckReaderTest

open Expecto
open Expect
open Swensen.Unquote
open FSharpPlus
open Types
open DeckReader
open System


[<Tests>]
let deckReaderTest =
    testList
        "deckReaderTest"

        [ testList
            "decksToDictionary"
              [ testCase "two valid decks get put into Map"
                <| fun () ->
                    let deck1Guid =
                        Guid.Parse("42f5ec90-6b63-4aa5-bee2-1b6e61cec121")

                    let deck2Guid =
                        Guid.Parse("d44d7869-f12a-4b76-bb85-44a8b7b38b06")

                    let deck1 =
                        { Deck.Metadata =
                              { DeckMetadata.Id = deck1Guid
                                Title = "deck1"
                                Description = None }
                          Cards =
                              [ { Id = 1; Text = "card1" }
                                { Id = 2; Text = "card2" } ] }

                    let deck2 =
                        { Deck.Metadata =
                              { DeckMetadata.Id = deck2Guid
                                Title = "deck2"
                                Description = None }
                          Cards =
                              [ { Id = 3; Text = "card3" }
                                { Id = 4; Text = "card4" } ] }

                    let actualR = decksToDictionary [ Ok deck1; Ok deck2 ]
                    let actual = wantOk actualR "Expected Ok"

                    test <@ Map.count actual = 2 @>
                    test <@ Map.containsKey deck1Guid actual @>
                    test <@ Map.containsKey deck2Guid actual @>
                    test <@ (Map.find deck1Guid actual).Metadata.Title = "deck1" @>
                    test <@ (Map.find deck2Guid actual).Metadata.Title = "deck2" @> ]


          testList
              "readDecks"
              [ testCase "parse a valid deck into Record"
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
}""" ]                      }

                    let actual = parseDecks [ input ]
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
                                      Text = "Another fancy card text." } ] } @> ] ]
