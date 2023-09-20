using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components.Sprites
{
    public abstract class Sprite : Component
    {
        protected Sprite(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }
    }
}
