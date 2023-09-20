using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GEMath
{
    public struct Rect
    {
        public Vector2 TopLeft;
        public Vector2 BottomRight;
        public Rect(Vector2 _topLeft, Vector2 _bottomRight)
        {
            TopLeft = _topLeft;
            BottomRight = _bottomRight;
        }

        public bool Overlaps(Rect _otherRect)
        {
            throw new NotImplementedException();
        }
    }
}
