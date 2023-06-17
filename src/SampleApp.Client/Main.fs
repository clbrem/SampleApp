namespace SampleApp.Client

open Microsoft.AspNetCore.SignalR.Client

module Main =

    open System
    open Elmish
    open Bolero
    open Bolero.Html
    open Bolero.Remoting
    open Bolero.Remoting.Client
    open Bolero.Templating.Client
    open SampleApp.Client
    open SampleApp.Client.Agent


    /// Routing endpoints definition.
    type Page =
        | [<EndPoint "/">] Home
        | [<EndPoint "/">] Status of string

    type Message =
        | SetPage of Page
        | Redirect of Page
        | ToggleStatus        
        | Poll
        | ReceivePoll of int option
        | SendHub
        | ReceiveHub
    type PingPong =
        | Ping
        | Pong
    type Status =
        | Complete
        | Waiting
        
    let toggleStatus =
        function
        | Complete -> Waiting
        | Waiting -> Complete
    type Service =
        {
            getStatus : unit -> Async<Guid>
            startComputation: Guid -> Async<unit> 
            poll: Guid -> Async<int option>        
        }
        interface IRemoteService with
          member this.BasePath = "/api"    
        
    /// The Elmish application's model.
    type Model =
        {
            page: Page
            status: Status
            pingPong: PingPong
            
        }
    let init = {page = Home; status=Complete; pingPong = Pong}

    /// Connects the routing system to the Elmish application.
    let router = Router.infer SetPage (fun model -> model.page)
    type Main = Template<"wwwroot/main.html">
    let update (hub: HubConnection) (service: Service) poll message model =        
        match message with 
        | SetPage page ->            
            {model with page = page},
            match page with
            | Home -> Cmd.batch [
                Cmd.OfAsync.perform service.getStatus () (string >> Status >> Redirect)                
                ]
            | Status _ ->
                Cmd.OfTask.attempt (
                    fun _ -> task {
                        do! hub.StartAsync()
                    }) () (fun _ -> SetPage Home)
                
        | Redirect page ->
            Console.WriteLine($"{page}")
            {model with page = page}, Cmd.none
        | ToggleStatus ->
            {model with status = toggleStatus model.status},
            match model.page, model.status with
            | Status st, Complete ->
                Cmd.batch [Cmd.OfAsync.attempt service.startComputation (Guid st) (fun _ -> Redirect Home); Cmd.OfAgent.perform poll (Some Poll)]
            | _, _ -> Cmd.none
        | Poll ->
            match model.page with
            | Home -> model, Cmd.none
            | Status st ->
                model, Cmd.OfAsync.perform service.poll (Guid st) ReceivePoll
        | ReceivePoll (Some _) ->
            { model with status = Complete },            
            Cmd.OfAgent.perform poll None                    
        | ReceivePoll None ->
            model, Cmd.none
        | SendHub ->
            { model with pingPong = Ping},
            Cmd.batch [
                Cmd.OfHub.receive<string, string,Message> hub "ReceiveMessage" (fun user msg -> ReceiveHub) 
                Cmd.OfHub.send<string,string,Message>
                    hub
                    "SendMessage"
                    ("hi","bye")
                    (fun exc -> Console.WriteLine $"Errored!!{exc.Message}"; SetPage Home)
                ]
        | ReceiveHub ->
            {model with pingPong = Pong},
            Cmd.none
            
            
            
        

    let view model dispatch =
        Main().HeaderContent(
            Main.StandardNav()
                .Home(router.getRoute Home)
                .Elt()
            ).Button(
               cond model.pingPong
               <| function
                   | Ping ->
                       Main.Spinner().Elt()
                   | Pong ->                   
                       Main
                           .BlueButton()
                           .Clicked(fun _ -> dispatch SendHub)
                           .Elt()
            ).Elt()    

    type MyApp() =
        inherit ProgramComponent<Model, Message>()
        
        override this.Program =
            let baseUri = Uri(this.NavigationManager.BaseUri)
            let service = this.Remote<Service>()
            use poll = Timers.poll (TimeSpan.FromSeconds(1))
            let hub = Hubs.factory(baseUri,"/hubs")
            
            Program.mkProgram (fun _ -> init, Cmd.none) (update hub service poll) view
            |> Program.withRouter router
    #if DEBUG
            |> Program.withHotReload
    #endif