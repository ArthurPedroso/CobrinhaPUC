using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GEMath
{
    public struct Vector2
    {
        public float X;
        public float Y;
        public Vector2(float _x, float _y)
        {
            X = _x;
            Y = _y;
        }
        public Vector2()
        {
            X = 0.0f;
            Y = 0.0f;
        }
        public Vector2 Normalized() => this / Magnitude();
        public float Magnitude() => MathF.Sqrt((X * X) + (Y * Y));

        public static Vector2 operator +(Vector2 _v1, Vector2 _v2) => new Vector2(_v1.X + _v2.X, _v1.Y + _v2.Y);
        public static Vector2 operator -(Vector2 _v1, Vector2 _v2) => new Vector2(_v1.X - _v2.X, _v1.Y - _v2.Y);
        public static Vector2 operator *(Vector2 _v1, Vector2 _v2) => new Vector2(_v1.X * _v2.X, _v1.Y * _v2.Y);
        public static Vector2 operator /(Vector2 _v1, float _f2) => new Vector2(_v1.X / _f2, _v1.Y / _f2);
        public static Vector2 operator *(Vector2 _v1, float _f2) => new Vector2(_v1.X * _f2, _v1.Y * _f2);
        public static bool operator ==(Vector2 _v1, Vector2 _v2) => (_v1.X == _v2.X && _v1.Y == _v2.Y);
        public static bool operator !=(Vector2 _v1, Vector2 _v2) => (_v1.X != _v2.X || _v1.Y != _v2.Y);

        public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);
        public static readonly Vector2 Up = new Vector2(0.0f, 1.0f);
        public static readonly Vector2 Down = new Vector2(0.0f, -1.0f);
        public static readonly Vector2 Right = new Vector2(1.0f, 0.0f);
        public static readonly Vector2 Left = new Vector2(-1.0f, 0.0f);
        public static readonly Vector2 One = new Vector2(1.0f, 1.0f);
    }
}
