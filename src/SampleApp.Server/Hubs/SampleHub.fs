namespace SampleApp.Server.Hubs
open System
open SampleApp.Server
open Microsoft.AspNetCore.SignalR
open System.Threading.Tasks

type SampleHub(state: State) =
    inherit Hub()
    
    member this.Ping(user: Guid) =
        task {
            let! items = state.List(user)
            do state.Add(user)
            do! this.Clients.Caller.SendAsync("Friends", items )  
            do! this.Clients.Caller.SendAsync("Pong")
        }
    member this.LogIn(user: Guid) =
        task {
            do state.Add(user)
            let! users = state.List(user)
            do!
                this.Clients.All.SendAsync(
                    "Friends",
                    users
                )
        }
    
