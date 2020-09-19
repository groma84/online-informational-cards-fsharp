#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Target //"

#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.DotNet

// Properties
let buildDir = "./build/"
let deployDir = "./deploy/"
let serverDir = "./OnlineInformationalCards"

let runDotNet cmd workingDir =
    let result =
        DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""

    if result.ExitCode <> 0
    then failwithf "'dotnet %s' failed in %s" cmd workingDir

// Targets
Target.create "Clean" (fun _ -> Shell.cleanDirs [ buildDir; deployDir ])

Target.create "Build" (fun _ ->
    let projects = !! "**/*.fsproj" -- "**Tests/**.fsproj"

    for projectFile in projects do
        DotNet.build id projectFile)


Target.create "RunTests" (fun _ ->
    let testProjects = !! "**Tests/**.fsproj"

    for projectFile in testProjects do
        DotNet.test id projectFile)

Target.create "Watch" (fun _ -> runDotNet "watch run" serverDir)

Target.create "Default" (fun _ -> Trace.trace "Hello World from FAKE")


"Clean" ==> "Build" ==> "RunTests" ==> "Default"


// start build
Target.runOrDefaultWithArguments "Default"
