using System;

namespace Tuntenfisch.Commons.Coupling.Scriptables
{
    [Flags]
    public enum AccessFlags
    {
        Unkown = 0,
        Read = 1,
        Write = 2
    }
}