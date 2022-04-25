using PS.Zones.Models.Perform;
using System;
using System.Collections.Generic;

namespace PS.Zones.Main
{
    [Serializable]
    public sealed class DBContext
    {
        public IReadOnlyList<Zone> Zones => zones;

        internal readonly List<Zone> zones = new List<Zone>();
    }
}
