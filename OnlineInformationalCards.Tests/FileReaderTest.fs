module FileReaderTest

open Expecto
open Expect
open FileReader
open AppErrors
open Swensen.Unquote

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
