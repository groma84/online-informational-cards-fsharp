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
let publishDir = "./publish/"
let serverDir = "./OnlineInformationalCards/"
let farmerDir = "./FarmerTemplate/"

let runDotNet cmd workingDir =
    let result =
        DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""

    if result.ExitCode <> 0
    then failwithf "'dotnet %s' failed in %s" cmd workingDir

let runDotNet5 cmd workingDir =
    let result =
        DotNet.exec
            (DotNet.Options.withWorkingDirectory workingDir
             >> DotNet.Options.withDotNetCliPath "dotnet5beta")
            cmd
            ""

    if result.ExitCode <> 0
    then failwithf "'dotnet %s' failed in %s" cmd workingDir

// Targets
Target.create "Clean" (fun _ -> Shell.cleanDirs [ publishDir ])

Target.create "Build" (fun _ ->
    let projects =
        !! "**/*.fsproj"
        -- "**Tests/**.fsproj"
        -- "**/Farmer*.fsproj"

    for projectFile in projects do
        DotNet.build id projectFile)


Target.create "RunTests" (fun _ ->
    let testProjects = !! "**Tests/**.fsproj"

    for projectFile in testProjects do
        DotNet.test id projectFile)

Target.create "Watch" (fun _ -> runDotNet "watch run" serverDir)

Target.create "Publish" (fun _ ->
    DotNet.publish (fun config ->
        { config with
              OutputPath = Some publishDir }) serverDir)

Target.create "FarmerDeploy" (fun _ -> runDotNet "run" farmerDir)

Target.create "CopyWebConfig" (fun _ -> System.IO.File.Copy("azure.web.config", "./publish/web.config", true))

Target.create "Default" (fun _ -> Trace.trace "Hello World from FAKE")



"Watch" ==> "Default"

"Clean"
==> "Build"
==> "RunTests"
==> "Publish"
==> "CopyWebConfig"
==> "FarmerDeploy"


// start build
Target.runOrDefaultWithArguments "Default"
