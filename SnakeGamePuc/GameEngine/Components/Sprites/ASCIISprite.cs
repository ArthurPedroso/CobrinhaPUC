using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components.Sprites
{
    public class ASCIISprite : Sprite
    {
        public char Icon { get; private set; }
        public ASCIISprite(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            Icon = ' ';
        }

        public ASCIISprite(GameObject _attachedGameObject, char _icon) : base(_attachedGameObject)
        {
            Icon = _icon;
        }
    }
}
