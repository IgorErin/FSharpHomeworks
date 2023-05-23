module Crawler

open System.Net.Http
open System.Text.RegularExpressions

module Static =
    let client = new HttpClient()
    
    let regex = Regex(@"a href=""http:(\S*)")
       
type Either<'a> = Left of string | Right of 'a
    
let getPageAsync (url: string) = async {
    try 
        let! response =
            Static.client.GetAsync(url) |> Async.AwaitTask

        let! response =
            response.Content.ReadAsStringAsync() |> Async.AwaitTask
        
        return Right response
    with e ->
        return Left e.Message
}

let runAsync url = async {
    let! page = getPageAsync url
    
    match page with
    | Right page ->
        Static.regex.Matches(page)
        |> Seq.map (fun url -> url.[8
        
        ()
    
    ()
}
    

let getCount url = async {
    
}
