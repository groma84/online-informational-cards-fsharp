module Types

open System

type Card = { Id: int; Text: string }

type DeckMetadata =
    { Id: Guid
      Title: string
      Description: string option }

type DeckChildren =
    | Subdecks of Deck list
    | Cards of Card list

and Deck =
    { Metadata: DeckMetadata
      Children: DeckChildren option }
