module Program

open Falco

[<EntryPoint>]
let main args =
    try
        Host.startWebHost
            args
            (Server.configureWebHost Env.developerMode)
            [ get "/" (Response.ofPlainText "Hello World") ]
        0
    with _ -> -1
