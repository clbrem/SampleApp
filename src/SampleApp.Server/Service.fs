namespace SampleApp.Server
open System
open Microsoft.AspNetCore.Hosting
open Bolero.Remoting.Server

type Service(ctx: IRemoteContext, env: IWebHostEnvironment) =
    inherit RemoteHandler<SampleApp.Client.Main.Service>()
    override this.Handler = 
        {            
            getStatus = fun () -> async {return Guid.NewGuid()}
            poll = fun _ -> failwith "todo"    
        }
    
