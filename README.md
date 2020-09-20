# Online Informational Cards (OInC)
View various card decks online for easy access on the road.

# Technical stuff
## FAKE as build tool
This project uses [FAKE](https://fake.build) as a local build tool. This only works with .NET Core SDK Version 3 or newer. To install the tool after cloning the repo execute `dotnet tool restore`.
As FAKE uses F# 5 features at the moment (September 2020) you need to add F# 5 Preview support for FSI scripts - in VSCode this is done by adding this to settings.json:
```"FSharp.fsiExtraParameters": [
    "--langversion:preview"
  ]
```

Starting auto-reloading watch mode for the server is started via `dotnet fake run build.fsx`.
