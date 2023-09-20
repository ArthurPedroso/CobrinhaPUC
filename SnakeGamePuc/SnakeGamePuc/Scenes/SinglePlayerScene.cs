using GameEngine;
using GameEngine.Components;
using GameEngine.Scenes;
using SnakeGamePuc.Scripts;
using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components.Sprites;

namespace SnakeGamePuc.Scenes
{
    internal class SinglePlayerScene : GameScene
    {
        public SinglePlayerScene() : base("SinglePlayerScene")
        {
            GameObject snake = ObjsBuilders.BuildSnakeController();
            AddObj(snake);

            GameObject snakeGame = ObjsBuilders.BuildSnakeGameCtrl();
            snakeGame.GetComponent<SnakeGameCtrl>().SnakeCtrl = snake.GetComponent<SnakeController>();
            AddObj(snakeGame);
        }
    }
}
