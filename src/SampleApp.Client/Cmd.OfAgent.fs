namespace SampleApp.Client

module Cmd =
    open Elmish
    module OfAgent =
        let perform<'S,'T> (agent: MailboxProcessor<Dispatch<'S> * 'T>) (message:'T) = 
            Cmd.ofEffect (fun dispatch -> agent.Post(dispatch, message))