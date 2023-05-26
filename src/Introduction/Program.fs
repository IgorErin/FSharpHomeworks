module Crawler

open System.Net.Http
open System.Text.RegularExpressions

module Static =
    let client = new HttpClient()
    
    let regex = Regex(@"<a href=""?(https://?\S*)""", RegexOptions.Compiled)
       
type Either<'a> = Left of string | Right of 'a
    
let mapUrl map (url: string) = async {
    try 
        let! response =
            Static.client.GetAsync(url) |> Async.AwaitTask
        
        let! response = map response
        
        return Right response
    with e ->
        return Left e.Message
}

let getPageAsync =
    (fun (response: HttpResponseMessage) -> async {
        return! response.Content.ReadAsStringAsync() |> Async.AwaitTask
    })
    |> mapUrl
    
let getPageAndSizeAsync url =
    (fun (response: HttpResponseMessage) -> async {
        let! page = response.Content.ReadAsByteArrayAsync() |> Async.AwaitTask
        
        return (url, page.Length)
    })
    |> fun map -> mapUrl map url  

let runAsync url = async {    
    let! page = getPageAsync url 
    
    match page with
    | Right page ->
        let result = 
            Static.regex.Matches(page)
            // get all url
            |> Seq.map (fun rawUrl -> rawUrl.Groups.[1].Value)
            // get all pages async
            |> Seq.map getPageAndSizeAsync
            |> Async.Parallel
            |> Right
        
        // |> return doesn't work 
        return result
    | Left message -> return (Left message)
}


[<EntryPoint>]
let main _ =
    runAsync "https://www.lettercount.com/"
    |> Async.RunSynchronously
    |> function
    | Right result ->
        result
        |> Async.RunSynchronously
        |> Seq.choose (function Right some -> Some some | _ -> None )
        |> fun actual -> printfn "%A" actual
        
    | _ -> failwith "Fail!"
        
    
    0
