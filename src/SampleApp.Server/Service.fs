namespace SampleApp.Server
open System
open System.Collections.Generic

open System.Timers
open Microsoft.AspNetCore.Hosting
open Bolero.Remoting.Server

type Service(ctx: IRemoteContext, env: IWebHostEnvironment) =
    inherit RemoteHandler<SampleApp.Client.Main.Service>()
    let  _inFlight : Dictionary<Guid, Timer option> = Dictionary()        
    let timer id =
        let t = new Timer(TimeSpan.FromSeconds(10))
        t.AutoReset <- false
        t.Elapsed.AddHandler(
            fun timer _ ->
                do _inFlight[id] <- None
                do (timer :?> Timer).Dispose()
            )        
        t.Start()
        _inFlight[id]<- Some t
    
    override this.Handler = 
        {            
            getStatus = fun () ->
                async {
                    return Guid.NewGuid()                                        
                }
            startComputation = fun id ->
                async {
                    do timer id
                    return ()
                }
            poll = fun id ->
                async {
                    match _inFlight.TryGetValue id with
                    | true, Some _ ->
                        return None 
                    | true, None ->
                        do _inFlight.Remove id |> ignore
                        return Some 1
                    | false, _ -> return Some 0
                }
                
                    
        }
    
