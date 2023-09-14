using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Rendering
{
    public interface IRenderer
    {
        public void RenderSprites(IRenderer renderer);
    }
}
