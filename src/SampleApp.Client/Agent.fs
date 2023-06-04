namespace Client

open System.Timers

module Agent =
    let private tryDispose (maybeTimer: System.Timers.Timer option) =
        match maybeTimer with
        | Some t -> t.Dispose()
        | None -> ()
    let wait (timeout: System.TimeSpan) (action: 'T -> unit) (msg: 'T)=
        let timer = new System.Timers.Timer(timeout)
        timer.Elapsed.AddHandler (fun _ _ -> action msg )
        timer.Start()
        timer
        
    let delay(timeout: System.TimeSpan) =
        MailboxProcessor.Start(
            fun inbox ->
                let rec loop timer =
                    async {
                       let! dispatch, msg  = inbox.Receive()                         
                       tryDispose timer                       
                       use timer = wait timeout dispatch msg 
                       return! Some timer |> loop                        
                    }
                loop None                
            )
        

