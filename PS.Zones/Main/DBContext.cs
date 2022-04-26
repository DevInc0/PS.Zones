using PS.Zones.Models.Perform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PS.Zones.Main
{
    [Serializable]
    public sealed class DBContext
    {
        public IReadOnlyList<Zone> Zones => _zones;

        internal bool TryAddZone(Zone newZone)
        {
            if (_zones.Exists(z => z.Name == newZone.Name)) return false;

            _zones.Add(newZone);
            return true;
        }

        internal bool RemoveZoneByName(string name)
        {
            int removedCount = _zones.RemoveAll(z => z.Name == name);

            return removedCount != 0;
        }

        internal bool RemoveZone(Zone newZone)
        {
            if (newZone == default) throw new ArgumentNullException(nameof(newZone));

            return _zones.Remove(newZone);
        }

        public bool TryGetZoneByName(string name, out Zone zone)
        {
            zone = _zones.FirstOrDefault(z => z.Name == name);

            return zone != default;
        }

        private readonly List<Zone> _zones = new List<Zone>();
    }
}
