using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components
{
    public abstract class Script : Component
    {
        public Script(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }

        public abstract void Start();
        public abstract void Update();
    }
}
