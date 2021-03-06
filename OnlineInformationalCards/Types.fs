module Types

open System

type Card = { Id: int; Text: string }

type DeckMetadata =
    { Id: Guid
      Title: string
      Description: string option }

type Deck =
    { Metadata: DeckMetadata
      Cards: Card list }

type DeckFiles =
    { MetadataPath: string
      CardsPaths: string list }

type DeckStrings =
    { MetadataJson: string
      CardsJson: string list }

[<Struct>]
type DeveloperMode = DeveloperMode of bool

[<Struct>]
type DeckDirectory = DeckDirectory of string

[<Struct>]
type Password = Password of string
