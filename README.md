# Online Informational Cards (OInC)
View various card decks online for easy access on the road.

# Technical stuff
## FAKE as build tool
This project uses [FAKE](https://fake.build) as a local build tool. This only works with .NET Core SDK Version 5 or newer. To install the tool after cloning the repo execute `dotnet tool restore`.
As FAKE uses F# 5 features at the moment (September 2020) you need to have [dotnet core 5 Preview](https://dotnet.microsoft.com/download/dotnet/5.0) installed.
You also need to add F# 5 Preview support for FSI scripts - in VSCode this is done by adding this to settings.json:
```"FSharp.fsiExtraParameters": [
    "--langversion:preview"
  ]```
Additionally you need to have the dotnet3 sdk as dotnet, and alias the dotnet5 preview sdk as dotnet5beta for my scripts to work.
Doing a default build is done via `dotnet5beta fake run build.fsx`.
