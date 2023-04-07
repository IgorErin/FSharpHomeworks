module Introduction

type Either<'a> = Left of string | Right of 'a

type Number = string

type Name = string 

type Record = { Name: Name; Number: Number }

let getName record = record.Name

let getNumber record = record.Number 

type SearchId = Name of Name | Number of Number

type Path = string 

type Action =
    | Add of Record
    | Search of SearchId
    | Show of (Record -> unit)
    | Save of (Record list -> unit)
    | Read of (unit -> Record list Either)
    
module Magazine =
    let add list record = record :: list
    
    let searchByName name =List.filter (getName >> ((=) name))
        
    let searchByNumber number = List.filter (getNumber >> ((=) number))
    
    let showAll show list = List.iter show list
        
    let search list = function
        | Name name -> searchByName name list 
        | Number number -> searchByNumber number list 
        
    let run list = function
        | Add record -> Right <| add list record
        | Search id -> Right <| search list id 
        | Show show ->
            showAll show list 
            
            Right <| list 
        | Save save ->
            save list
            
            Right <| list
        | Read read -> read ()
