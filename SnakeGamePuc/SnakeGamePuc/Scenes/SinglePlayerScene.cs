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
            GameObject snake = new GameObject("Player");
            snake.AttachComponent(new Transform(snake));
            snake.AttachComponent(new SnakeController(snake));
            snake.AttachComponent(new Collider(snake));
            snake.AttachComponent(new ASCIISprite(snake, '@'));

            AddObj(snake);
        }
    }
}
