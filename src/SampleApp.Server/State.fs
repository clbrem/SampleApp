namespace SampleApp.Server
open System
open System.Collections.Generic

type Operations =
    | Add of Guid
    | Remove of Guid
    | Get of AsyncReplyChannel<Guid list>

type State() =
    
    let timeout = TimeSpan.FromSeconds 10
    let remove (loggedIn: Map<Guid, System.Timers.Timer>) user =
        match loggedIn |> Map.tryFind user with        
        | Some t ->
            do t.Dispose()
            loggedIn |> Map.remove user 
        | None -> loggedIn
    let add (loggedIn: Map<Guid, System.Timers.Timer>) onLogOut (user: Guid) =
        let timer = new System.Timers.Timer(timeout)
        timer.Elapsed.AddHandler(
            fun timer evt ->
                onLogOut user
            )
        timer.Start()
        loggedIn |> Map.add user timer
    let handler = MailboxProcessor.Start(
        fun inbox ->
            let rec loop loggedIn =
                async {
                    match! inbox.Receive() with
                    | Add user ->
                        return! add loggedIn (Remove >> inbox.Post) user |> loop
                    | Remove user ->
                        return! remove loggedIn user |> loop
                    | Get channel ->
                        loggedIn |> Map.keys |> List.ofSeq |> channel.Reply
                        return! loop loggedIn
                }
            loop Map.empty
        )
    member _.Add(guid: Guid) = handler.Post(Add guid)
    member _.List(caller: Guid) =
        handler.PostAndAsyncReply(Get)
    
    

