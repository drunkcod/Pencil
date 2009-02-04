#light

namespace Pencil.Unit

type ISuite =
    abstract Tests : (ITestResult -> unit) list