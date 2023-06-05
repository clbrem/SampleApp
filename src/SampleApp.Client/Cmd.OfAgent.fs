namespace SampleApp.Client

module Cmd =
    open Elmish
    module OfAgent =
        let perform<'S> (agent: MailboxProcessor<Dispatch<'S> * 'S>) (message:'S) = 
            Cmd.ofEffect (fun dispatch -> agent.Post(dispatch, message))