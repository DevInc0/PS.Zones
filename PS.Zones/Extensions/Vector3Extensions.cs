using PS.Zones.Models.Data;
using UnityEngine;

namespace PS.Zones.Extensions
{
    public static class Vector3Extensions
    {
        public static Point ToPoint(this Vector3 vector3) => new Point(vector3);
    }
}
