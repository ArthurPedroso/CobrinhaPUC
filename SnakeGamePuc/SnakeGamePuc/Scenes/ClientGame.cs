using GameEngine;
using GameEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scenes
{
    internal class ClientGame : GameScene
    {
        public ClientGame() : base("ClientGame")
        {
        }

        public override void BuildScene()
        {
            foreach (GameObject obj in ObjsBuilders.BuildClientGame())
            {
                AddObj(obj);
            }

        }
    }
}
