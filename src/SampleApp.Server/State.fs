namespace SampleApp.Server
open System
type State() =
    let timeout = TimeSpan.FromMinutes 1
    let mutable _loggedIn: Map<Guid, System.Timers.Timer> = Map.empty
    member private this.Remove(user: Guid) =
        _loggedIn <- _loggedIn |> Map.remove user        
    member this.Add(user: Guid) =
        let timer = new System.Timers.Timer(timeout)        
        timer.Elapsed.AddHandler(
            fun timer evt ->
                do this.Remove(user)
                do (timer :?> System.Timers.Timer).Dispose()                
            )
        timer.Start()
        _loggedIn <- _loggedIn.Add(user, timer)
    
    member _.List() =
        _loggedIn |> Map.keys |> List.ofSeq
    
    

