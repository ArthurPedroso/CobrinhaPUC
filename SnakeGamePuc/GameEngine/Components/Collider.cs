using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components
{
    public class Collider : Component
    {
        private Rect m_shape;
        public Collider(GameObject _attachedGameObject, Rect _shape) : base(_attachedGameObject)
        {
            m_shape = _shape;
        }
    }
}
