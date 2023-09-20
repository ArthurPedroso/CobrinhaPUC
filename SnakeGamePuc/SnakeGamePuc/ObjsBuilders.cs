using GameEngine.Components.Sprites;
using GameEngine.Components;
using GameEngine;
using SnakeGamePuc.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc
{
    internal static class ObjsBuilders
    {
        public static GameObject BuildSnakeController()
        {
            GameObject snake = new GameObject("Player");

            snake.AttachComponent(new Transform(snake));
            snake.AttachComponent(new Collider(snake));
            snake.AttachComponent(new ASCIISprite(snake, '@'));
            snake.AttachComponent(new SnakeController(snake));

            return snake;
        }
        public static GameObject BuildSnakeBody()
        {
            GameObject snake = new GameObject("SnakeBody");

            snake.AttachComponent(new Transform(snake));
            snake.AttachComponent(new Collider(snake));
            snake.AttachComponent(new ASCIISprite(snake, 'X'));
            snake.AttachComponent(new SnakeBody(snake));

            return snake;
        }
    }
}
