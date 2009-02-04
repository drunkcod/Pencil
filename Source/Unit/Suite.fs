#light

namespace Pencil.Unit

type ISuite =
    abstract Tests : (ITestResult -> ITestResult) list
