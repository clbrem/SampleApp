module SampleApp.Server.Index

open Bolero
open Bolero.Html
open Bolero.Server.Html
open SampleApp

let page = doctypeHtml {
    head {
        meta { attr.charset "UTF-8" }
        meta { attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" }
        title { "A Sample Page" }
        ``base`` { attr.href "/" }
        link { attr.rel "stylesheet"; attr.href "css/tailwind.css" }
        link { attr.rel "stylesheet"; attr.href "css/index.css" }
        link { attr.rel "preconnect"; attr.href "https://fonts.googleapis.com"}
        link { attr.rel "preconnect"; attr.href "https://fonts.gstatic.com"; attr.crossorigin ""}
        link { attr.rel "stylesheet"; attr.href "https://fonts.googleapis.com/css2?family=Overpass+Mono:wght@400;600&family=Source+Sans+Pro:ital,wght@0,400;0,700;1,400&family=Spectral:ital,wght@0,400;0,600;1,400&display=swap" }

    }
    body {        
        div { attr.id "main"; comp<Client.Main.MyApp> }
        boleroScript
    }
}