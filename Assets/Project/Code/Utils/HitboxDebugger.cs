using System.Collections.Generic;
using UnityEngine;

namespace Project.Code.Utils
{
    public class HitboxDebugger : MonoBehaviour
    {
        private static HitboxDebugger _instance;
        public static HitboxDebugger Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("HitboxDebugger");
                    _instance = go.AddComponent<HitboxDebugger>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private struct DebugShape
        {
            public enum ShapeType { Sphere, Box }
            public ShapeType Type;
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Size;
            public Color Color;
            public float ExpirationTime;
        }

        private readonly List<DebugShape> _shapes = new List<DebugShape>();

        public void DrawSphere(Vector3 position, float radius, Color color, float duration = 1f)
        {
            _shapes.Add(new DebugShape
            {
                Type = DebugShape.ShapeType.Sphere,
                Position = position,
                Size = Vector3.one * radius,
                Color = color,
                ExpirationTime = Time.time + duration
            });
        }

        public void DrawBox(Vector3 position, Vector3 size, Quaternion rotation, Color color, float duration = 1f)
        {
            _shapes.Add(new DebugShape
            {
                Type = DebugShape.ShapeType.Box,
                Position = position,
                Size = size,
                Rotation = rotation,
                Color = color,
                ExpirationTime = Time.time + duration
            });
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                _shapes.RemoveAll(s => Time.time > s.ExpirationTime);
            }

            foreach (var shape in _shapes)
            {
                Gizmos.color = shape.Color;
                switch (shape.Type)
                {
                    case DebugShape.ShapeType.Sphere:
                        Gizmos.DrawWireSphere(shape.Position, shape.Size.x);
                        break;
                    case DebugShape.ShapeType.Box:
                        Matrix4x4 rotationMatrix = Matrix4x4.TRS(shape.Position, shape.Rotation, shape.Size);
                        Gizmos.matrix = rotationMatrix;
                        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                        Gizmos.matrix = Matrix4x4.identity;
                        break;
                }
            }
        }
    }
}
