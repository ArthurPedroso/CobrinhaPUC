using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components
{
    public abstract class Component
    {
        private static int s_idCount = 0;
        private readonly int r_id;

        public GameObject AttachedGameObject { get; private set; }
        public int GetID { get => r_id; }
        protected Component(GameObject _attachedGameObject)
        {
            r_id = s_idCount++;
            AttachedGameObject = _attachedGameObject;
        }
    }
}
