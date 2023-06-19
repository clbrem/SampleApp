namespace SampleApp.Server

open System.Text.Json
open System.Text.Json.Serialization
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.ResponseCompression
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Bolero
open Bolero.Remoting.Server
open Bolero.Server
open SampleApp
open Bolero.Templating.Server

type Startup() =

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    member this.ConfigureServices(services: IServiceCollection) =
        
        services.AddMvc() |> ignore
        services.AddServerSideBlazor() |> ignore
        services.AddSingleton<State>() |> ignore
        services.AddSignalR().AddJsonProtocol(
            fun opts ->
                JsonFSharpOptions
                    .Default()
                    .AddToJsonSerializerOptions(opts.PayloadSerializerOptions)
                )
        |> ignore
        services.AddResponseCompression(
            fun opts ->
                opts.MimeTypes <- Seq.append ResponseCompressionDefaults.MimeTypes ["application/octet-stream"]
            ) |> ignore
        services
            .AddAuthorization()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .Services
            .AddBoleroRemoting<Service>()
            .AddBoleroHost()
#if DEBUG
            .AddHotReload(templateDir = __SOURCE_DIRECTORY__ + "/../SampleApp.Client")
#endif
        |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseWebAssemblyDebugging()

        app
            .UseResponseCompression()            
            .UseAuthentication()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthorization()
            .UseBlazorFrameworkFiles()
            .UseEndpoints(fun endpoints ->
#if DEBUG
                endpoints.UseHotReload()
#endif
                endpoints.MapHub<Hubs.SampleHub>("/hubs") |> ignore
                endpoints.MapBoleroRemoting() |> ignore
                endpoints.MapBlazorHub() |> ignore

                endpoints.MapFallbackToBolero(Index.page) |> ignore
                
                )
            
        |> ignore
        

module Program =

    [<EntryPoint>]
    let main args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseStaticWebAssets()
            .UseStartup<Startup>()
            .Build()            
            .Run()
        0