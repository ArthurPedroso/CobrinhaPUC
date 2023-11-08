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
        public bool Vizible { get; set; }
        public char Icon { get; private set; }
        public ASCIISprite(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            Vizible = true;
            Icon = ' ';
        }

        public ASCIISprite(GameObject _attachedGameObject, char _icon) : base(_attachedGameObject)
        {
            Vizible = true;
            Icon = _icon;
        }
    }
}
