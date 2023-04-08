module Introduction

open System
open System.IO

type number = string

type name = string

type Record =
    { Name: name
      Number: number }

type Magazine = Record list

type Path = string

type Response =
    | Name of name
    | Number of number

type Result<'a> =
    | Error of string
    | Success of Magazine
    | SuccessWith of 'a * Magazine

type Action<'a> =
    | Add of Record
    | SearchNumber of name
    | SearchName of number
    | Show of (Record list -> Result<'a>)
    | SaveToFile of Path
    | ReadFromFile of Path

module Magazine =
    /// <summary>
    /// Record projection to Name.
    /// </summary>
    /// <param name="record"></param>
    let getName record = record.Name

    /// <summary>
    /// Record projection to Number.
    /// </summary>
    /// <param name="record"></param>
    let getNumber record = record.Number

    /// <summary>
    /// Save list of records.
    /// </summary>
    /// <param name="list">List to save.</param>
    /// <param name="path">Path.</param>
    let save list (path: string) =
        if File.Exists(path) then
            use streamWriter = new StreamWriter(path)

            let helper (writer: StreamWriter) =
                function
                | head :: _ -> writer.Write($"{head.Name} {head.Number}")
                | [] -> ()

            streamWriter.WriteLine(List.length list)

            helper streamWriter list

            // implicit dispose
            // so GC not to collect ahead of time
            streamWriter.Dispose()

            Success []
        else
            Error "file does not exist"

    /// <summary>
    /// Read records from file.
    /// </summary>
    /// <param name="path">Path to file.</param>
    let read (path: string) =
        if File.Exists(path) then
            use streamReader = new StreamReader(path)

            let rec helper (reader: StreamReader) count acc =
                if count > 0 then
                    let newRecord =
                        reader.ReadLine().Split()
                        |> Array.take 2
                        |> (fun array ->
                            { Name = array.[0]
                              Number = array.[1] }
                        )

                    helper reader (count - 1) <| newRecord :: acc
                else
                    acc

            let size = streamReader.ReadLine().Split() |> Array.map int |> Array.tryLast

            match size with
            | Some size ->
                let result = Success <| helper streamReader size []

                // implicit dispose
                // so GC not to collect ahead of time
                streamReader.Dispose()

                result
            | None -> Error "TODO" // TODO()
        else
            Error "file does not exist"

    /// <summary>
    /// Run magazine.
    /// </summary>
    /// <param name="list">List of source records.</param>
    let run list =
        function
        | Action.Add record -> Success <| record :: list
        | Action.SearchNumber name ->
            List.tryFind (getName >> ((=) name)) list
            |> function
                | Some record -> SuccessWith <| (Response.Number record.Number, list)
                | None -> Error "name not found"
        | Action.SearchName number ->
            List.tryFind (getNumber >> ((=) number)) list
            |> function
                | Some record -> SuccessWith <| (Response.Name record.Name, list)
                | None -> Error "number not found"
        | Action.Show show -> show list
        | Action.SaveToFile path -> save list path
        | Action.ReadFromFile path -> read path

type Message<'a, 'b> =
    | Exit
    | Command of 'a
    | Unbound

module Interactive =
    /// <summary>
    /// Parse string array.
    /// </summary>
    let parse show =
        function
        | [| "add"; name; number |] ->
            Command
            <| Add
                { Name = name
                  Number = number }
        | [| "searchNumber"; name |] -> Command <| SearchNumber name
        | [| "searchName"; number |] -> Command <| SearchName number
        | [| "show" |] -> Command <| Show show
        | [| "save"; path |] -> Command <| SaveToFile path
        | [| "read"; path |] -> Command <| ReadFromFile path
        | [| "exit" |] -> Exit
        | _ -> Unbound

    /// <summary>
    /// Run interactive phone magazine.
    /// </summary>
    /// <param name="showData">Function that show data.</param>
    /// <param name="showList">Function that show records.</param>
    /// <param name="showError">Function that show error.</param>
    let run showData showList showError () =
        let rec helper list =
            let string = Console.ReadLine()

            string.Split()
            |> Array.filter (not << String.IsNullOrEmpty)
            |> parse showList
            |> function
                | Command command ->
                    match Magazine.run list command with
                    | Success list -> helper list
                    | SuccessWith(data, list) ->
                        showData data
                        helper list
                    | Error message ->
                        showError message
                        helper list
                | Unbound ->
                    showError $"unbound command: %A{string}, try again"
                    helper list
                | Exit -> ()

        helper []
