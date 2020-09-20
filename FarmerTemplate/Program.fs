open Farmer
open Farmer.Builders

let publishDir = "../publish/"

let myWebApp = webApp {
    name "OnlineInformationalCards"
    zip_deploy publishDir
}

let deployment = arm {
    location Location.GermanyWestCentral
    add_resource myWebApp
}

deployment
|> Deploy.execute "onlineInformationalCardsResourceGroup" Deploy.NoParameters
|> ignore