using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components
{
    public abstract class Component
    {
        public GameObject AttachedGameObject { get; private set; }

        protected Component(GameObject _attachedGameObject)
        {
            AttachedGameObject = _attachedGameObject;
        }
    }
}
