module Server

open System
open Types
open Falco
open Falco.Host
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

let handleException (developerMode: DeveloperMode): ExceptionHandler =
    let (DeveloperMode developerMode) = developerMode

    fun (ex: Exception) (log: ILogger) ->
        let logMessage =
            sprintf "Server error: %s\n\n%s" ex.Message ex.StackTrace
        // match developerMode with
        // | true -> sprintf "Server error: %s\n\n%s" ex.Message ex.StackTrace
        // | false -> "Server Error"

        log.Log(LogLevel.Error, logMessage)

        Response.withStatusCode 500
        >> Response.ofPlainText logMessage

let handleNotFound: HttpHandler =
    Response.withStatusCode 404
    >> Response.ofPlainText "Not found"

let configureWebHost (developerMode: DeveloperMode): ConfigureWebHost =
    let configureLogging (log: ILoggingBuilder) =
        log.SetMinimumLevel(LogLevel.Information)
        |> ignore

    let configureServices (services: IServiceCollection) =
        services.AddRouting().AddResponseCaching().AddResponseCompression()
        |> ignore

    let configure (developerMode: DeveloperMode) (endpoints: HttpEndpoint list) (app: IApplicationBuilder) =
        app.UseExceptionMiddleware(handleException developerMode).UseResponseCaching().UseResponseCompression()
           .UseStaticFiles().UseRouting().UseHttpEndPoints(endpoints).UseNotFoundHandler(handleNotFound)

        |> ignore

    fun (endpoints: HttpEndpoint list) (webHost: IWebHostBuilder) ->
        webHost.UseKestrel().ConfigureKestrel(fun options -> options.ListenAnyIP(5015))
               .ConfigureLogging(configureLogging).ConfigureServices(configureServices)
               .Configure(configure developerMode endpoints)
        |> ignore
