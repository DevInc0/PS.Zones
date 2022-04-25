using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using UnityEngine;
using Point = PS.Zones.Models.Data.Point;

namespace PS.Zones.Models.Perform
{
    [Serializable]
    public sealed class PolygonZone : Zone
    {
        private Point[] _points;

        private byte[] _types;

        [NonSerialized] private GraphicsPath _path;

        [OnSerializing]
        private void OnSerializingHandler(StreamingContext context)
        {
            int pointCount = _path.PointCount;

            PointF[] currentPoints = _path.PathPoints;
            byte[] currentTypes = _path.PathTypes;

            _points = new Point[pointCount];
            _types = new byte[pointCount];

            for (var i = 0; i < pointCount; i += 1)
            {
                _points[i] = new Point(currentPoints[i]);
                _types[i] = currentTypes[i];
            }
        }

        [OnDeserialized]
        private void OnDeserializedHandler(StreamingContext context)
        {
            int pointsCount = _points.Length;
            var points = new PointF[pointsCount];

            for (var i = 0; i < pointsCount; i += 1)
            {
                points[i] = _points[i].AsPointF;
            }

            _path = new GraphicsPath(points, _types);
        }

        public PolygonZone(string name) : base(name) { }

        public void AddPoint(Vector3 position)
        {
            var currentPointsCount = _path.PointCount;
            PointF[] currentPoints = _path.PathPoints;

            var tempPoints = new PointF[_points.Length + 1];

            for (var i = 0; i < currentPointsCount; i += 1)
            {
                tempPoints[i] = currentPoints[i];
            }

            tempPoints[_points.Length] = new PointF(position.x, position.z);

            _path.Reset();
            _path.AddPolygon(tempPoints);
        }

        public override bool IsPositionInside(Vector3 position) => _path.IsVisible(position.x, position.z);
    }
}
