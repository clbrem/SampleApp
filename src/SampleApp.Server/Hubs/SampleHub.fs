namespace SampleApp.Server.Hubs
open System
open SampleApp.Server
open Microsoft.AspNetCore.SignalR
open System.Threading.Tasks

type SampleHub(state: State) =
    inherit Hub()

    member this.Ping() =
        task {
            do! Task.Delay 1000
            do! this.Clients.Caller.SendAsync("Pong")
        }

    member this.LogIn(user: Guid) =
        task {
            let userId = this.Context.UserIdentifier
            do state.Add(userId,user)
            do!
                this.Clients.All.SendAsync(
                    "Friends",
                    state.List()
                )
        }
