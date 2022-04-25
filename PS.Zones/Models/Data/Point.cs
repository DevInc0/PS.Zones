using System;
using System.Drawing;
using UnityEngine;

namespace PS.Zones.Models.Data
{
    [Serializable]
    public struct Point
    {
        public float X
        {
            get => _x;
            internal set => _x = value;
        }

        public float Z
        {
            get => _z;
            internal set => _z = value;
        }

        private float _x;

        private float _z;

        public PointF AsPointF => new PointF(_x, _z);

        public Point(float x, float z)
        {
            _x = x;
            _z = z;
        }

        public Point(PointF pointF)
        {
            _x = pointF.X;
            _z = pointF.Y;
        }

        public Point(Vector3 vector3)
        {
            _x = vector3.x;
            _z = vector3.z;
        }

        public Point(Vector2 vector2)
        {
            _x = vector2.x;
            _z = vector2.y;
        }
    }
}
