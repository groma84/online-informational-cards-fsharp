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
      CardPaths: string list }

type DeckStrings =
    { MetadataJson: string
      CardsJson: string list }
