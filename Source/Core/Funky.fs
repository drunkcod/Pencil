#light

namespace Pencil.Core
open System.Collections.Generic

[<AutoOpen>]
module Funky =
    let inline GetEnumerator (x:^src) = (^src : (member GetEnumerator : unit -> IEnumerator<_>)(x))

    let inline Each action sequence =
        use items = GetEnumerator sequence
        while items.MoveNext() do
            action items.Current

    let FilterMap p f = (Seq.filter p) >> (Seq.map f)