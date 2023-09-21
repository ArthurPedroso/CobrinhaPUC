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
using GameEngine.Components.Scripts;
using SnakeGamePuc.Scripts.MainMenu;

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
        public static GameObject BuildUI(Vector2 _pos, string _ui, bool _centerUI = true)
        {
            GameObject ui = new GameObject("Ui");

            ui.AttachComponent(new Transform(ui));
            ui.AttachComponent(new UI(ui, _ui, _centerUI));
            ui.GetComponent<Transform>().Position = _pos;

            return ui;
        }
        public static GameObject BuildMainMenuCtrl()
        {
            GameObject ui = new GameObject("UiCtrl");

            ui.AttachComponent(new MainMenuCtrl(ui));

            return ui;
        }
        public static GameObject[] BuildMainMenu()
        {
            return new GameObject[] 
            {
                BuildUI(new Vector2(0.0f, 4.0f), "Snake Game"),                
                BuildUI(new Vector2(-1.0f, 0.0f), "1. Jogar"),
                BuildUI(new Vector2(-1.0f, -1.0f), "2. Sair"),
                BuildMainMenuCtrl()
            };
        }
    }
}
