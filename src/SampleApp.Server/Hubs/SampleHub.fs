namespace SampleApp.Server.Hubs
open System
open SampleApp.Server
open Microsoft.AspNetCore.SignalR
open System.Threading.Tasks



type SampleHub(state: State) =
    inherit Hub()
    
    member this.Ping() =
        task {            
            do! this.Clients.Caller.SendAsync("Pong")
        }
    member this.LogIn(user: Guid) =
        task {
            do state.Add(user )            
            do!
                this.Clients.All.SendAsync(
                    "Friends",
                    state.List()
                )
        }
    
