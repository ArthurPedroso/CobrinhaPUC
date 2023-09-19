using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components
{
    public class Transform : Component
    {
        private Matrix3x3 m_transformMat;

        public Matrix3x3 ModelMatClone { get => (Matrix3x3)m_transformMat.Clone(); }
        public Vector2 Position 
        { 
            get { return new Vector2(m_transformMat.GetElement(2, 0), m_transformMat.GetElement(2, 1)); }
            set { m_transformMat.SetElement(2, 0, value.X); m_transformMat.SetElement(2, 1, value.Y); }
        }
        public Vector2 Scale 
        { 
            get { return new Vector2(m_transformMat.GetElement(0, 0), m_transformMat.GetElement(1, 1)); }
            set { m_transformMat.SetElement(0, 0, value.X); m_transformMat.SetElement(1, 1, value.Y); }
        }
        public Vector2 Rotation { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }

        public Transform(GameObject _go, Matrix3x3 transformMat) : base(_go)
        {
            m_transformMat = transformMat;
        }
    }
}
