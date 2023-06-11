namespace SampleApp.Server.Hubs
open Microsoft.AspNetCore.SignalR
type SampleHub() =
    inherit Hub()
    member this.SendMessage(user: string, message: string) =
        this.Clients.All.SendAsync("ReceiveMessage", user, message)

