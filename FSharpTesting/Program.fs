// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open JoshuaKearney.Measurements

[<EntryPoint>]
let main argv = 
    let dist = Distance.Units.Yard.Multiply(4.5)

    printfn "%A" dist
    Console.Read() |> ignore

    0