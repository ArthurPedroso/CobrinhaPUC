using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame.HostGame
{
    internal class HostSnakeCtrl : MPlayerSnakeCtrl
    {
        public HostGameCtrl GameCtrl { private get; set; }
        public HostSnakeCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }
    }
}
