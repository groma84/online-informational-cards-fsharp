module ParsersTest

open Expecto
open Types
open Parsers
open Swensen.Unquote
open Fleece.Newtonsoft

[<Tests>]
let tests =
    testList
        "DeckMetadata"
        [ testCase "should error on invalid id field"
          <| fun () ->
              let j =
                  """{"id": 3, "title": "X", "description": "abc"}"""

              let x: DeckMetadata ParseResult = parseJson j

              test <@ false = true @> ]
