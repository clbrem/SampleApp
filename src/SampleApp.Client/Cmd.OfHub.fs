namespace SampleApp.Client.Agent
open Microsoft.AspNetCore.SignalR.Client
open Microsoft.AspNetCore.Components
open Microsoft.Extensions.DependencyInjection
open System.Text.Json.Serialization
open Elmish
module Hubs =    
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
module Cmd =
    [<AbstractClass>]
    type OfHub =
    
        static member send (hub: HubConnection) methodName message ofError =
            Cmd.OfTask.attempt
                (fun msg -> task {do! hub.SendAsync (methodName, message)})                     
                message
                ofError
        static member receive<'A> (hub: HubConnection) =
            fun methodName (onSent: unit -> 'A)  ->
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, onSent >> dispatch) |> ignore
                        ) 
        static member receive<'T,'A> (hub: HubConnection) =
            fun methodName (onSent: 'T -> 'A) -> 
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, fun a -> onSent a |> dispatch) |> ignore
                        )
        static member receive<'T1,'T2,'A> (hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'A) -> 
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, fun a b -> onSent a b |> dispatch) |> ignore
                        )         
        static member receive<'T1,'T2,'T3,'A> (hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 ->'T3 -> 'A) -> 
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, fun a b c -> onSent a b c |> dispatch) |> ignore
                        )
        static member receive<'T1,'T2,'T3,'T4,'A> (hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 ->'T3->'T4->'A) -> 
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, fun a b c d-> onSent a b c d|> dispatch) |> ignore
                        )
        static member receive<'T1,'T2,'T3,'T4,'T5,'A> (hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 ->'A) -> 
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, fun a b c d e-> onSent a b c d e |> dispatch) |> ignore
                        )                
        static member receive<'T1,'T2,'T3,'T4,'T5,'T6,'A> (hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 ->'T6 ->'A) -> 
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, fun a b c d e f-> onSent a b c d e f |> dispatch) |> ignore
                        )
        static member receive<'T1,'T2,'T3,'T4,'T5,'T6,'T7,'A> (hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 ->'T6->'T7 ->'A) -> 
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, fun a b c d e f g-> onSent a b c d e f g |> dispatch) |> ignore
                        )
        static member receive<'T1,'T2,'T3,'T4,'T5,'T6,'T7,'T8,'A> (hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'T8 ->'A) -> 
                Cmd.ofEffect (
                    fun dispatch ->
                        hub.On(methodName, fun a b c d e f g h-> onSent a b c d e f g h|> dispatch) |> ignore
                        )                

               
                
                
                
            
              
    