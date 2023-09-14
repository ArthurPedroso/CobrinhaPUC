using GameEngine.Components.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Rendering
{
    public interface IRenderer
    {
        public void RenderSprites(ImageSprite[] _imageSprites);
        public void RenderSprites(ASCIISprite[] _asciiSprites);
        public void RenderSprites(ImageSprite[] _imageSprites, ASCIISprite[] _asciiSprites);
    }
}
