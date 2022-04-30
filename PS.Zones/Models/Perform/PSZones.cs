using PS.Zones.Main;
using System.Collections.Generic;
using System.Linq;

namespace PS.Zones.Models.Perform
{
    public sealed class PSZones
    {
        public static IReadOnlyList<Zone> Instance => Plugin.DataContext.Zones;

        public static ZonesEvents Events;

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
