#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"

#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.IO

// Properties
let buildDir = "./build/"


// Targets
Target.create "Clean" (fun _ -> Shell.cleanDir buildDir)

Target.create "Default" (fun _ -> Trace.trace "Hello World from FAKE")

// Dependencies
open Fake.Core.TargetOperators

"Clean" ==> "Default"

// start build
Target.runOrDefault "Default"
