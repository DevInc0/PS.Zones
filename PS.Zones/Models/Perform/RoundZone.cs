using System;
using UnityEngine;
using Point = PS.Zones.Models.Data.Point;

namespace PS.Zones.Models.Perform
{
    [Serializable]
    public sealed class RoundZone : Zone
    {
        public Point Center
        {
            get => _center;
            internal set => _center = value;
        }

        public float Radius
        {
            get => Mathf.Sqrt(_sqrRadius);
            internal set => _sqrRadius = value * value;
        }

        private Point _center;

        private float _sqrRadius;

        public RoundZone(string name, Vector3 center, float radius) : base(name)
        {
            _center = new Point(center.x, center.z);
            _sqrRadius = radius * radius;
        }

        public override bool IsPositionInside(Vector3 position)
        {
            float xDifference = _center.X - position.x;
            float zDifference = _center.Z - position.z;

            return (xDifference * xDifference) + (zDifference * zDifference) <= _sqrRadius;
        }
    }
}
