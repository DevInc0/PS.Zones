using PS.Zones.Models.Perform;
using System;
using System.Collections.Generic;

namespace PS.Zones.Main
{
    [Serializable]
    public sealed class DBContext
    {        
        internal bool TryAddZone(Zone newZone)
        {
            if (Zones.Exists(z => z.Name == newZone.Name)) return false;

            Zones.Add(newZone);
            return true;
        }

        internal bool RemoveZoneByName(string name)
        {
            int removedCount = Zones.RemoveAll(z => z.Name == name);

            return removedCount != 0;
        }

        internal bool RemoveZone(Zone newZone)
        {
            if (newZone == default) throw new ArgumentNullException(nameof(newZone));

            return Zones.Remove(newZone);
        }

        internal readonly List<Zone> Zones = new List<Zone>();
    }
}
