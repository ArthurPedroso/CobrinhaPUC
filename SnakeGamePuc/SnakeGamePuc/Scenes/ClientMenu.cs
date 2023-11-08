using GameEngine;
using GameEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scenes
{
    internal class ClientMenu : GameScene
    {
        public ClientMenu() : base("ClientMenu")
        {
        }

        public override void BuildScene()
        {
            foreach (GameObject obj in ObjsBuilders.BuildClientMenu())
            {
                AddObj(obj);
            }
        }
    }
}
