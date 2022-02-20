using System;

namespace Tuntenfisch.Commons.Coupling
{
    [Flags]
    public enum AccessFlags
    {
        Unkown = 0,
        Read = 1 << 0,
        Write = 1 << 1
    }
}