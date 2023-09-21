using GameEngine.Components.Sprites;
using GameEngine.Components;
using GameEngine;
using SnakeGamePuc.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.GEMath;

namespace SnakeGamePuc
{
    internal static class ObjsBuilders
    {
        public static GameObject BuildSnakeGameCtrl()
        {
            GameObject snakeGameCtrl = new GameObject("SnakeGameCtrl");

            snakeGameCtrl.AttachComponent(new SnakeGameCtrl(snakeGameCtrl));

            return snakeGameCtrl;
        }
        public static GameObject BuildApple(Action _appleCB)
        {
            GameObject apple = new GameObject("Apple");

            apple.AttachComponent(new Transform(apple));
            apple.AttachComponent(new Collider(apple));
            apple.AttachComponent(new ASCIISprite(apple, 'M'));
            apple.AttachComponent(new Apple(apple, _appleCB));

            return apple;
        }
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
        public static GameObject[] BuildWalls()
        {
            List<GameObject> walls = new List<GameObject>();
            int width = GameInstance.Renderer.GetWidth();
            int height = GameInstance.Renderer.GetHeight();
            for (int i = 0; i < width; i++)
            {
                GameObject wall = new GameObject("Wall");

                wall.AttachComponent(new Transform(wall));
                wall.AttachComponent(new Collider(wall));
                wall.AttachComponent(new ASCIISprite(wall, '0'));
                wall.GetComponent<Transform>().Position = new Vector2(i - (width / 2.0f), -(height / 2.0f) + 1);
                walls.Add(wall);
            }
            for (int i = 0; i < width; i++)
            {
                GameObject wall = new GameObject("Wall");

                wall.AttachComponent(new Transform(wall));
                wall.AttachComponent(new Collider(wall));
                wall.AttachComponent(new ASCIISprite(wall, '0'));
                wall.GetComponent<Transform>().Position = new Vector2(i - (width / 2.0f), (height / 2.0f));
                walls.Add(wall);
            }
            for (int i = 0; i < height; i++)
            {
                GameObject wall = new GameObject("Wall");

                wall.AttachComponent(new Transform(wall));
                wall.AttachComponent(new Collider(wall));
                wall.AttachComponent(new ASCIISprite(wall, '0'));
                wall.GetComponent<Transform>().Position = new Vector2((width / 2.0f) - 1, i - (height / 2.0f));
                walls.Add(wall);
            }
            for (int i = 0; i < height; i++)
            {
                GameObject wall = new GameObject("Wall");

                wall.AttachComponent(new Transform(wall));
                wall.AttachComponent(new Collider(wall));
                wall.AttachComponent(new ASCIISprite(wall, '0'));
                wall.GetComponent<Transform>().Position = new Vector2(-(width / 2.0f), i - (height / 2.0f));
                walls.Add(wall);
            }
            return walls.ToArray();
        }
        public static GameObject[] BuildMainMenu()
        {
            return null;
        }
    }
}
