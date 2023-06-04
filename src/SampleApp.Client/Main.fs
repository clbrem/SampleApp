module SampleApp.Client.Main

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


type Message =
    | SetPage of Page
/// The Elmish application's model.
type Model =
    {
        page: Page
    }
let init = {page = Home}

/// Connects the routing system to the Elmish application.
let router = Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/main.html">
let update message model =
    match message with 
    | SetPage page ->
        {model with page = page}, Cmd.none

let view model dispatch =
    Main().HeaderContent(
        Main.StandardNav()
            .Home(router.getRoute Home)
            .Elt()
        ).Elt()    

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkProgram (fun _ -> init, Cmd.none) update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif