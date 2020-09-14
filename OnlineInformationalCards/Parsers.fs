module Parsers

open Thoth.Json.Net
open Types

type DeckMetadata with
    static member Decoder: Decoder<DeckMetadata> =
        Decode.map3 (fun id title description ->
            { Id = id
              Title = title
              Description = description }: DeckMetadata) (Decode.field "id" Decode.guid)
            (Decode.field "title" Decode.string) (Decode.field "description" (Decode.option Decode.string))

type Card with
    static member Decoder: Decoder<Card> =
        Decode.map2 (fun id text -> { Id = id; Text = text }: Card) (Decode.field "id" Decode.int)
            (Decode.field "text" Decode.string)
