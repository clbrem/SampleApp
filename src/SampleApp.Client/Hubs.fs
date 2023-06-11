namespace SampleApp.Client.Agent
open Microsoft.AspNetCore.SignalR
open Microsoft.AspNetCore.SignalR.Client
open Microsoft.Extensions.DependencyInjection
open System.Text.Json
open System.Text.Json.Serialization

module Hubs =
    open Microsoft.AspNetCore.SignalR.Client
    let factory (url: string) =
        HubConnectionBuilder()
          .WithUrl(url)
          .WithAutomaticReconnect()
          .AddJsonProtocol(
              fun opts ->
                JsonFSharpOptions
                    .Default()
                    .AddToJsonSerializerOptions(opts.PayloadSerializerOptions)
          ).Build()
    
          
          
          
          
        
    type Message<'S,'T> =
        | Send of 'S
        | Receive of 'T
    
    
    