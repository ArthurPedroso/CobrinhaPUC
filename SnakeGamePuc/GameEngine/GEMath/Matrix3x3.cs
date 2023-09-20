using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameEngine.GEMath
{
    public class Matrix3x3 : Matrix
    {
        public Matrix3x3() : base(3, 3)
        {
            SetElement(0, 0, 1.0f);
            SetElement(1, 1, 1.0f);
            SetElement(2, 2, 1.0f);
        }
    }
}
