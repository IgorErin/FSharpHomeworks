namespace Interpreter

type Id = string

type Term =
    | Abs of Id * Term
    | App of Term * Term
    | Var of Id

module Lambda =
    /// <summary>
    /// Get new variable by current variable and free variables list.
    /// </summary>
    /// <param name="variable">Current variable to replace.</param>
    /// <param name="freeVariables">Free variables list.</param>
    let rec getNewVariable variable freeVariables =
        let rawVariable = sprintf $"%s{variable}$"

        if not <| List.exists ((=) rawVariable) freeVariables then
            rawVariable
        else
            getNewVariable rawVariable freeVariables

    /// <summary>
    /// Get free variables from term.
    /// </summary>
    /// <param name="term">Term.</param>
    let getFreeVariables term =
        let rec getFreeVariables boundVariables freeVariables =
            function
            | Var name ->
                if List.exists ((=) name) boundVariables then
                    freeVariables
                else
                    name :: freeVariables
            | Abs(name, term) ->
                getFreeVariables (name :: boundVariables) freeVariables term
            | App(leftTerm, rightTerm) ->
                let getFreeVariables = getFreeVariables boundVariables freeVariables

                let leftResult = getFreeVariables leftTerm
                let rightResult = getFreeVariables rightTerm

                leftResult @ rightResult

        getFreeVariables [] [] term

    /// <summary>
    /// Substitute the term N instead of a variable named x
    /// </summary>
    /// <param name="x">Variable name.</param>
    /// <param name="N">Term to substitute.</param>
    let rec substitute x N =
        function
        | Var name as var -> if name = x then N else var
        | Abs(y, P) as abs ->
            if y = x then
                abs
            else
                let NFreeVariables = getFreeVariables N
                let PFreeVariables = getFreeVariables P

                let isXInPFreeVariables = List.exists ((=) x) PFreeVariables
                let isYInNFreeVariables = List.exists ((=) y) NFreeVariables

                if isXInPFreeVariables && isYInNFreeVariables then
                    let z = getNewVariable y <| NFreeVariables @ PFreeVariables

                    substitute y (Var z) P |> substitute x N |> (fun term -> Abs(z, term))
                else
                    substitute x N P |> fun term -> Abs(y, term)
        | App(leftTerm, rightTerm) ->
            let substitute = substitute x N

            App(substitute leftTerm, substitute rightTerm)

    /// <summary>
    /// Reduce term by betta reduction.
    /// </summary>
    let rec reduce =
        function
        | Var _ as var -> var
        | Abs(id, term) -> Abs(id, reduce term)
        | App(Abs(name, body), rightTerm) -> substitute name rightTerm body
        | App(leftTerm, rightTerm) -> App(reduce leftTerm, reduce rightTerm)
