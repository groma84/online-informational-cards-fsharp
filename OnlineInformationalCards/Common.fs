[<AutoOpen>]
module Common

module Env =
    open System
    open System.IO
    open Falco.StringUtils
    open Types

    let root = Directory.GetCurrentDirectory()

    let deckDirectory =
        Path.Combine(root, "decks") |> DeckDirectory

    let tryGetEnv (name: string) =
        match Environment.GetEnvironmentVariable name with
        | null
        | "" -> None
        | value -> Some value

    let developerMode =
        match tryGetEnv "ASPNETCORE_ENVIRONMENT" with
        | None -> true
        | Some env -> strEquals env "development"
        |> DeveloperMode

    let password =
        match tryGetEnv "PASSWORD" with
        | None -> None
        | Some env -> env |> Password |> Some
