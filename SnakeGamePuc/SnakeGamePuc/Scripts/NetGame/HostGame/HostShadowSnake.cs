using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame.HostGame
{
    internal class HostShadowSnake : ShadowSnakeCtrl
    {
        public HostGameCtrl GameCtrl { private get; set; }
        public HostShadowSnake(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }
    }
}
