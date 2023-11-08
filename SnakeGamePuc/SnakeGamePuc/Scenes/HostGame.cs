using GameEngine;
using GameEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scenes
{
    internal class HostGame : GameScene
    {
        public HostGame() : base("HostGame")
        {
        }

        public override void BuildScene()
        {
            foreach (GameObject obj in ObjsBuilders.BuildHostGame())
            {
                AddObj(obj);
            }
        }
    }
}
