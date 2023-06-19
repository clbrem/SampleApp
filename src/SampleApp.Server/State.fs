namespace SampleApp.Server
open System
type State() =
    let mutable _loggedIn: Map<string,Guid> = Map.empty
    member _.Add(userId: string, user: Guid) =
        _loggedIn <- _loggedIn.Add(userId,user)
    
    member _.List() =
        _loggedIn |> Map.values |> List.ofSeq
    

