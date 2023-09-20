using GameEngine;
using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts
{
    internal class SnakeGameCtrl : Script
    {

        public SnakeGameCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }

        public override void Start()
        {
            GameObject[] walls = ObjsBuilders.BuildWalls();
            foreach (GameObject wall in walls) { GameInstance.Instantiate(wall); }
        }

        public override void Update()
        {
        }
    }
}
