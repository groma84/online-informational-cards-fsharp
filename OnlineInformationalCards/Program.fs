module Program

open Falco
open FileReader
open DeckReader
open FSharpPlus

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
                [ Routing.get
                    "/"
                      (decks
                       |> Map.toList
                       |> (List.map (snd >> (fun deck -> deck.Metadata)))
                       |> DeckListView.view
                       |> Response.ofHtml) // Liste aller Decks
                  Routing.get "/deck/{deckId:guid}" (Response.ofPlainText "Hello World! deckId") // Redirect auf zufällig ermittelte CardId
                  Routing.get "/deck/{deckId:guid}/card/{cardId}" (Response.ofPlainText "Hello World! deckId cardId") ] // konkrete Karte anzeigen, mit generierten URLs für vorwärts rückwärts zufällig Buttons
        | Error err ->
            Host.startWebHost
                args
                (Server.configureWebHost Env.developerMode)
                [ Routing.get "/{**unused}" (Response.ofHtml <| ErrorView.view err) ]

        0
    with _ -> -1
