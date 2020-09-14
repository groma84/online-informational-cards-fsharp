module ParsersTest

open System
open Expecto
open Swensen.Unquote
open Thoth.Json.Net
open Types
open Parsers

[<Tests>]
let tests =
    testList
        "DeckMetadata"
        [ testCase "should error on invalid id field"
          <| fun () ->
              let actual =
                  """{"id": 3, "title": "X", "description": "abc"}"""
                  |> Decode.fromString DeckMetadata.Decoder

              test
                  <@ Expect.wantError actual "Err expected" = "Error at: `$.id`\nExpecting a guid but instead got: 3" @>

          testCase "description is optional"
          <| fun () ->
              let actual =
                  """{"id": "840701c0-132f-49e8-a268-9d9aecc28736", "title": "X", "description": null}"""
                  |> Decode.fromString DeckMetadata.Decoder

              test
                  <@ Expect.wantOk actual "Ok expected" =
                      { Id = Guid.Parse("840701c0-132f-49e8-a268-9d9aecc28736")
                        Title = "X"
                        Description = None } @>

          testCase "title and description get parsed correctly"
          <| fun () ->
              let actual =
                  """{"id": "840701c0-132f-49e8-a268-9d9aecc28736", "title": "X", "description": "Y"}"""
                  |> Decode.fromString DeckMetadata.Decoder

              test
                  <@ Expect.wantOk actual "Ok expected" =
                      { Id = Guid.Parse("840701c0-132f-49e8-a268-9d9aecc28736")
                        Title = "X"
                        Description = Some "Y" } @> ]


[<Tests>]
let cardTests =
    testList
        "Card"
        [ testCase "errors on invalid data"
          <| fun () ->
              let actual =
                  """{"id": "3b", "text": "X"}"""
                  |> Decode.fromString Card.Decoder

              test
                  <@ Expect.wantError actual "Err expected" =
                      "Error at: `$.id`\nExpecting an int but instead got: \"3b\"" @>

          testCase "values get decoded correcly"
          <| fun () ->
              let actual =
                  """{"id": 3, "text": "X"}"""
                  |> Decode.fromString Card.Decoder

              test <@ Expect.wantOk actual "Ok expected" = { Id = 3; Text = "X" } @> ]
