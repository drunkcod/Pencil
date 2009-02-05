#light

namespace Pencil.Unit

type ISuite =
    abstract Tests : (ITestResult -> ITestResult) list

module Suite =        
    let Fact m f = fun (result:ITestResult) -> 
        f (result.Begin m)
        
    let Theory m inputs f = fun (result:ITestResult) ->
        inputs |> Seq.fold (fun result x -> f x result) (result.Begin m)

    let Suite x = {new ISuite with member this.Tests = x }
