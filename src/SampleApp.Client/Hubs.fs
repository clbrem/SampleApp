namespace SampleApp.Client.Agent
open Microsoft.AspNetCore.SignalR.Client
open Microsoft.AspNetCore.Components
open Microsoft.Extensions.DependencyInjection
open System.Text.Json.Serialization

module Hubs =
    open Microsoft.AspNetCore.SignalR.Client    
    let factory () =        
        let hub = 
          HubConnectionBuilder()
              .WithUrl("/hubs")
              .WithAutomaticReconnect()
              .AddJsonProtocol(
                  fun opts ->
                    JsonFSharpOptions
                        .Default()
                        .AddToJsonSerializerOptions(opts.PayloadSerializerOptions)
              ).Build()
        hub.On<string, string>(
            "ReceiveMessage",
            fun user message ->                
                    failwith "todo"
            )
        
        
    
          
          
          
          
        
    type Message<'S,'T> =
        | Send of 'S
        | Receive of 'T
    
    
    