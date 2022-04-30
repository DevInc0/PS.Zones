using PS.Zones.Main;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PS.Zones.Models.Perform
{
    public sealed class PSZones
    {
        public static IReadOnlyList<Zone> Instance => Plugin.DataContext.Zones;

        public static ZonesEvents Events;

        public static bool TryAddZone(Zone newZone)
        {
            if (Plugin.DataContext.Zones.Exists(z => z.Name == newZone.Name)) return false;

            Plugin.DataContext.Zones.Add(newZone);
            return true;
        }

        public static bool RemoveZone(Zone zone)
        {
            if (zone == default) throw new ArgumentNullException(nameof(zone));

            return Plugin.DataContext.Zones.Remove(zone);
        }

        public static bool RemoveZoneByName(string name)
        {
            int removedCount = Plugin.DataContext.Zones.RemoveAll(z => z.Name == name);

            return removedCount != 0;
        }

        public static bool TryGetZoneByName(string name, out Zone zone)
        {
            try
            {
                zone = Instance.FirstOrDefault(z => z.Name == name);
            }
            catch
            {
                zone = default;
            }

            return zone != default;
        }

        internal static void Init()
        {
            Events = new ZonesEvents();
        }
    }
}
