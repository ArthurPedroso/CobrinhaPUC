using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GEMath
{
    public struct Rectangle
    {
        public Vector2 TopLeft;
        public Vector2 BottomRight;
        public Rectangle(Vector2 _topLeft, Vector2 _bottomRight)
        {
            TopLeft = _topLeft;
            BottomRight = _bottomRight;
        }

        public bool Overlaps(Rectangle _otherRect)
        {
            throw new NotImplementedException();
        }
    }
}
