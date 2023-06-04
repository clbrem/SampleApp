namespace SampleApp.Client
module Main =

    open System
    open Elmish
    open Bolero
    open Bolero.Html
    open Bolero.Remoting
    open Bolero.Remoting.Client
    open Bolero.Templating.Client

    /// Routing endpoints definition.
    type Page =
        | [<EndPoint "/">] Home
        | [<EndPoint "/">] Status of string

    type Message =
        | SetPage of Page
        | Redirect of Page
        | ToggleStatus
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
            poll: Guid -> Async<unit option>        
        }
        interface IRemoteService with
          member this.BasePath = "/api"    
        
    /// The Elmish application's model.
    type Model =
        {
            page: Page
            status: Status
        }
    let init = {page = Home; status=Complete}

    /// Connects the routing system to the Elmish application.
    let router = Router.infer SetPage (fun model -> model.page)
    type Main = Template<"wwwroot/main.html">
    let update (service: Service) message model =
        match message with 
        | SetPage page ->        
            {model with page = page},
            match page with
            | Home -> Cmd.OfAsync.perform service.getStatus () (string >> Status >> Redirect)
            | Status _ -> Cmd.none
        | Redirect page ->
            {model with page = page}, Cmd.none
        | ToggleStatus ->
            {model with status = toggleStatus model.status}, Cmd.none

    let view model dispatch =
        Main().HeaderContent(
            Main.StandardNav()
                .Home(router.getRoute Home)
                .Elt()
            ).Button(
               cond (model.status)
               <| function
                   | Waiting ->
                       Main.Spinner().Elt()
                   | Complete ->                   
                       Main
                           .BlueButton()
                           .Clicked(fun _ -> dispatch ToggleStatus)
                           .Elt()
            ).Elt()    

    type MyApp() =
        inherit ProgramComponent<Model, Message>()
        
        override this.Program =
            let service = this.Remote<Service>()
            Program.mkProgram (fun _ -> init, Cmd.none) (update service) view
            |> Program.withRouter router
    #if DEBUG
            |> Program.withHotReload
    #endif