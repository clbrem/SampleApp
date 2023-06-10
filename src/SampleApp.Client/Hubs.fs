namespace SampleApp.Client.Agent

module Hubs =
    open Microsoft.AspNetCore.SignalR.Client
    type Message<'S,'T> =
        | Send of 'S
        | Receive of 'T
    
    