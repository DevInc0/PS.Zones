using PS.Zones.Models.Perform;
using System;
using System.Collections.Generic;

namespace PS.Zones.Main
{
    [Serializable]
    public sealed class DBContext
    {
        internal readonly List<Zone> Zones = new List<Zone>();
    }
}
