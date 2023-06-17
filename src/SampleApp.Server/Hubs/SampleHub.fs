namespace SampleApp.Server.Hubs
open Microsoft.AspNetCore.SignalR
open System.Threading.Tasks
type SampleHub() =
    inherit Hub()
    member this.SendMessage(user: string, message: string) =
        task {
            do! Task.Delay 5000
            do! this.Clients.All.SendAsync("ReceiveMessage", user, message)
        }
        

    
