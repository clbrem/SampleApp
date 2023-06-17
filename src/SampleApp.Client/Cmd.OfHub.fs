namespace SampleApp.Client.Agent

open Microsoft.AspNetCore.SignalR
open Microsoft.AspNetCore.SignalR.Client
open Microsoft.Extensions.DependencyInjection
open System.Text.Json.Serialization
open Elmish

module Hubs =
    let factory (baseUrl: System.Uri, url: string) =
        match System.Uri.TryCreate(baseUrl, url) with
        | true, uri ->
            HubConnectionBuilder()
                .WithUrl(uri)
                .WithAutomaticReconnect()
                .AddJsonProtocol(fun opts ->
                    JsonFSharpOptions
                        .Default()
                        .AddToJsonSerializerOptions(opts.PayloadSerializerOptions))
                .Build()
        | false, _ ->
            raise (HubException($"Could not build hub with path {url}"))

module Cmd =

    [<AbstractClass>]
    type OfHub =
        static member send<'A>(hub: HubConnection) : string -> (exn -> 'A) -> Cmd<'A> =
            fun methodName -> Cmd.OfTask.attempt (fun _ -> task { do! hub.SendAsync methodName }) ()

        static member send<'T, 'A>(hub: HubConnection) : string -> 'T -> (exn -> 'A) -> Cmd<'A> =
            fun methodName -> Cmd.OfTask.attempt (fun message -> task { do! hub.SendAsync(methodName, arg1 = message) })

        static member send< 'T1, 'T2,'A>(hub: HubConnection) : string -> 'T1 * 'T2 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2) -> task { do! hub.SendAsync(methodName, arg1 = m1, arg2 = m2) })

        static member send<'T1, 'T2, 'T3,'A>
            (hub: HubConnection)
            : string -> 'T1 * 'T3 * 'T3 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2, m3) ->
                    task { do! hub.SendAsync(methodName, arg1 = m1, arg2 = m2, arg3 = m3) })

        static member send<'T1, 'T2, 'T3, 'T4,'A>
            (hub: HubConnection)
            : string -> 'T1 * 'T3 * 'T3 * 'T4 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2, m3, m4) ->
                    task { do! hub.SendAsync(methodName, arg1 = m1, arg2 = m2, arg3 = m3, arg4 = m4) })

        static member send<'T1, 'T2, 'T3, 'T4, 'T5,'A>
            (hub: HubConnection)
            : string -> 'T1 * 'T3 * 'T3 * 'T4 * 'T5 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2, m3, m4, m5) ->
                    task { do! hub.SendAsync(methodName, arg1 = m1, arg2 = m2, arg3 = m3, arg4 = m4, arg5 = m5) })

        static member send<'T1, 'T2, 'T3, 'T4, 'T5, 'T6,'A>
            (hub: HubConnection)
            : string -> 'T1 * 'T3 * 'T3 * 'T4 * 'T5 * 'T6 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2, m3, m4, m5, m6) ->
                    task {
                        do! hub.SendAsync(methodName, arg1 = m1, arg2 = m2, arg3 = m3, arg4 = m4, arg5 = m5, arg6 = m6)
                    })

        static member send<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'T7,'A>
            (hub: HubConnection)
            : string -> 'T1 * 'T3 * 'T3 * 'T4 * 'T5 * 'T6 * 'T7 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2, m3, m4, m5, m6, m7) ->
                    task {
                        do!
                            hub.SendAsync(
                                methodName,
                                arg1 = m1,
                                arg2 = m2,
                                arg3 = m3,
                                arg4 = m4,
                                arg5 = m5,
                                arg6 = m6,
                                arg7 = m7
                            )
                    })

        static member send< 'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'T7, 'T8,'A>
            (hub: HubConnection)
            : string -> 'T1 * 'T3 * 'T3 * 'T4 * 'T5 * 'T6 * 'T7 * 'T8 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2, m3, m4, m5, m6, m7, m8) ->
                    task {
                        do!
                            hub.SendAsync(
                                methodName,
                                arg1 = m1,
                                arg2 = m2,
                                arg3 = m3,
                                arg4 = m4,
                                arg5 = m5,
                                arg6 = m6,
                                arg7 = m7,
                                arg8 = m8
                            )
                    })

        static member send<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'T7, 'T8, 'T9,'A>
            (hub: HubConnection)
            : string -> 'T1 * 'T3 * 'T3 * 'T4 * 'T5 * 'T6 * 'T7 * 'T8 * 'T9 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2, m3, m4, m5, m6, m7, m8, m9) ->
                    task {
                        do!
                            hub.SendAsync(
                                methodName,
                                arg1 = m1,
                                arg2 = m2,
                                arg3 = m3,
                                arg4 = m4,
                                arg5 = m5,
                                arg6 = m6,
                                arg7 = m7,
                                arg8 = m8,
                                arg9 = m9
                            )
                    })

        static member send<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'T7, 'T8, 'T9, 'T10,'A>
            (hub: HubConnection)
            : string -> 'T1 * 'T3 * 'T3 * 'T4 * 'T5 * 'T6 * 'T7 * 'T8 * 'T9 * 'T10 -> (exn -> 'A) -> Cmd<'A> =
            fun methodName ->
                Cmd.OfTask.attempt (fun (m1, m2, m3, m4, m5, m6, m7, m8, m9, m10) ->
                    task {
                        do!
                            hub.SendAsync(
                                methodName,
                                arg1 = m1,
                                arg2 = m2,
                                arg3 = m3,
                                arg4 = m4,
                                arg5 = m5,
                                arg6 = m6,
                                arg7 = m7,
                                arg8 = m8,
                                arg9 = m9,
                                arg10 = m10
                            )
                    })

        static member receive<'A>(hub: HubConnection) =
            fun methodName (onSent: unit -> 'A) ->
                Cmd.ofEffect (fun dispatch -> hub.On(methodName, onSent >> dispatch) |> ignore)

        static member receive<'T, 'A>(hub: HubConnection) =
            fun methodName (onSent: 'T -> 'A) ->
                Cmd.ofEffect (fun dispatch -> hub.On(methodName, (fun a -> onSent a |> dispatch)) |> ignore)

        static member receive<'T1, 'T2, 'A>(hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'A) ->
                Cmd.ofEffect (fun dispatch -> hub.On(methodName, (fun a b -> onSent a b |> dispatch)) |> ignore)

        static member receive<'T1, 'T2, 'T3, 'A>(hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'A) ->
                Cmd.ofEffect (fun dispatch -> hub.On(methodName, (fun a b c -> onSent a b c |> dispatch)) |> ignore)

        static member receive<'T1, 'T2, 'T3, 'T4, 'A>(hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'A) ->
                Cmd.ofEffect (fun dispatch -> hub.On(methodName, (fun a b c d -> onSent a b c d |> dispatch)) |> ignore)

        static member receive<'T1, 'T2, 'T3, 'T4, 'T5, 'A>(hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'A) ->
                Cmd.ofEffect (fun dispatch ->
                    hub.On(methodName, (fun a b c d e -> onSent a b c d e |> dispatch)) |> ignore)

        static member receive<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'A>(hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'A) ->
                Cmd.ofEffect (fun dispatch ->
                    hub.On(methodName, (fun a b c d e f -> onSent a b c d e f |> dispatch))
                    |> ignore)

        static member receive<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'T7, 'A>(hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'A) ->
                Cmd.ofEffect (fun dispatch ->
                    hub.On(methodName, (fun a b c d e f g -> onSent a b c d e f g |> dispatch))
                    |> ignore)

        static member receive<'T1, 'T2, 'T3, 'T4, 'T5, 'T6, 'T7, 'T8, 'A>(hub: HubConnection) =
            fun methodName (onSent: 'T1 -> 'T2 -> 'T3 -> 'T4 -> 'T5 -> 'T6 -> 'T7 -> 'T8 -> 'A) ->
                Cmd.ofEffect (fun dispatch ->
                    hub.On(methodName, (fun a b c d e f g h -> onSent a b c d e f g h |> dispatch))
                    |> ignore)
