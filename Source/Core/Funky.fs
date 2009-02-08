#light

namespace Pencil.Core
open System.Collections.Generic

[<AutoOpen>]
module Funky =
    let inline GetEnumerator (x:^src) = (^src : (member GetEnumerator : unit -> IEnumerator<_>)(x))
    
    let inline Iter f s =
        use iter = GetEnumerator s
        while iter.MoveNext() do
            f iter.Current
