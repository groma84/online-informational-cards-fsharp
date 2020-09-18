module ErrorView

open Falco.Markup.Elem
open Falco.Markup.Text
open Falco.Markup.Attr
open AppErrors

let view (errors: list<ApplicationError>) =
    let oneError error =
        li [] [
            raw (ApplicationError.ToString error)
        ]

    div [] [
        h1 [ style "color: red;" ] [
            raw "Fehler"
        ]
        h2 [] [
            raw "Leider ist etwas schiefgelaufen beim Laden der Kartendecks:"
        ]
        ul [] (List.map oneError errors)
    ]
