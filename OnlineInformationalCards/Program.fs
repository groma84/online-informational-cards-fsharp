module Program

open Falco
open FileReader
open DeckReader

[<EntryPoint>]
let main args =
    try

        let decksR =
            Env.deckDirectory
            |> traverseDirectory
            |> readDeckFiles
            |> parseDecks
            |> decksToDictionary

        match decksR with
        | Ok decks ->
            Host.startWebHost
                args
                (Server.configureWebHost Env.developerMode)
                [ get "/" (Response.ofPlainText "Hello World!") // Liste aller Decks
                  get "/{deckId:guid}" (Response.ofPlainText "Hello World! deckId") // Redirect auf zufällig ermittelte CardId
                  get "/{deckId:guid}/{cardId}" (Response.ofPlainText "Hello World! deckId cardId") ] // konkrete Karte anzeigen, mit generierten URLs für vorwärts rückwärts zufällig Buttons
        | Error err ->
            Host.startWebHost
                args
                (Server.configureWebHost Env.developerMode)
                [ get "/{**unused}" (Response.ofHtml <| ErrorView.view err) ]

        0
    with _ -> -1
