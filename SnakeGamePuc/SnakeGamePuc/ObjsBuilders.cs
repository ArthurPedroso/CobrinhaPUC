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
using SnakeGamePuc.Scripts.MultiplayerMenu;
using SnakeGamePuc.Scripts.HostMenu;
using SnakeGamePuc.Scripts.ClientMenu;

namespace SnakeGamePuc
{
    internal static class ObjsBuilders
    {
        internal static GameObject BuildSnakeGameCtrl()
        {
            GameObject snakeGameCtrl = new GameObject("SnakeGameCtrl");

            snakeGameCtrl.AttachComponent(new SnakeGameCtrl(snakeGameCtrl));

            return snakeGameCtrl;
        }
        internal static GameObject BuildApple(Action _appleCB)
        {
            GameObject apple = new GameObject("Apple");

            apple.AttachComponent(new Transform(apple));
            apple.AttachComponent(new Collider(apple));
            apple.AttachComponent(new ASCIISprite(apple, 'M'));
            apple.AttachComponent(new Apple(apple, _appleCB));

            return apple;
        }
        internal static GameObject BuildSnakeController()
        {
            GameObject snake = new GameObject("Player");

            SoundEmitter appleEat = new SoundEmitter(snake, Resources.OnAppleEaten);
            SoundEmitter failure = new SoundEmitter(snake, Resources.Failure);
            SoundEmitter win = new SoundEmitter(snake, Resources.OnWin);

            snake.AttachComponent(new Transform(snake));
            snake.AttachComponent(new Collider(snake));
            snake.AttachComponent(new ASCIISprite(snake, '@'));
            snake.AttachComponent(appleEat);
            snake.AttachComponent(failure);
            snake.AttachComponent(win);
            snake.AttachComponent(new SnakeController(snake, appleEat, failure, win));

            return snake;
        }
        internal static GameObject BuildSnakeBody()
        {
            GameObject snake = new GameObject("SnakeBody");

            snake.AttachComponent(new Transform(snake));
            snake.AttachComponent(new Collider(snake));
            snake.AttachComponent(new ASCIISprite(snake, 'X'));
            snake.AttachComponent(new SnakeBody(snake));

            return snake;
        }
        internal static GameObject[] BuildWalls()
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
        internal static GameObject BuildUI(Vector2 _pos, string _ui, bool _centerUI = true)
        {
            GameObject ui = new GameObject("Ui");

            ui.AttachComponent(new Transform(ui));
            ui.AttachComponent(new UI(ui, _ui, _centerUI));
            ui.GetComponent<Transform>().Position = _pos;

            return ui;
        }
        internal static GameObject BuildMainMenuCtrl()
        {
            GameObject ui = new GameObject("UiCtrl");

            ui.AttachComponent(new MainMenuCtrl(ui));

            return ui;
        }
        internal static GameObject BuildMultiplayerCtrl()
        {
            GameObject ui = new GameObject("UiCtrl");

            ui.AttachComponent(new MultiplayerMenuCtrl(ui));

            return ui;
        }
        internal static GameObject[] BuildMainMenu()
        {
            return new GameObject[] 
            {
                BuildUI(new Vector2(0.0f, 4.0f), "Snake Game"),                
                BuildUI(new Vector2(-1.0f, 0.0f), "1.Singleplayer"),
                BuildUI(new Vector2(-1.0f, -1.0f), "2.Multiplayer"),
                BuildUI(new Vector2(-1.0f, -2.0f), "3.Sair"),
                BuildMainMenuCtrl()
            };
        }
        internal static GameObject[] BuildMultiplayerMenu()
        {
            return new GameObject[]
            {
                BuildUI(new Vector2(0.0f, 5.0f), "Multiplayer"),
                BuildUI(new Vector2(0.0f, 4.0f), "Selection"),
                BuildUI(new Vector2(-1.0f, 0.0f), "1.Hospedar"),
                BuildUI(new Vector2(-1.0f, -1.0f), "2.Conectar"),
                BuildUI(new Vector2(-1.0f, -2.0f), "3.Sair"),
                BuildMultiplayerCtrl()
            };
        }
        internal static GameObject BuildEndSceneCtrl()
        {
            GameObject endSceneCtrl = new GameObject("EndSceneCtrl");

            endSceneCtrl.AttachComponent(new EndSceneCtrl(endSceneCtrl));

            return endSceneCtrl;

        }
        internal static GameObject[] BuildWinScene()
        {
            return new GameObject[]
            {
                BuildUI(new Vector2(0.0f, 4.0f), "Voce Ganhou!"),
                BuildUI(new Vector2(-1.0f, 0.0f), "1.Sair"),
                BuildEndSceneCtrl()
            };
        }
        internal static GameObject[] BuildLoseScene()
        {
            return new GameObject[]
            {
                BuildUI(new Vector2(0.0f, 4.0f), "Voce Perdeu!"),
                BuildUI(new Vector2(-1.0f, 0.0f), "1.Sair"),
                BuildEndSceneCtrl()
            };
        }

        internal static GameObject BuildHostCtrl()
        {
            GameObject hostCtrl = new GameObject("HostCtrl");

            hostCtrl.AttachComponent(new HostMenuCtrl(hostCtrl));

            return hostCtrl;
        }

        internal static GameObject BuildClientCtrl()
        {
            GameObject clientMenuCtrl = new GameObject("ClientCtrl");

            clientMenuCtrl.AttachComponent(new ClientMenuCtrl(clientMenuCtrl));

            return clientMenuCtrl;
        }

        internal static GameObject[] BuildHostMenu()
        {
            return new GameObject[]
            {
                BuildUI(new Vector2(0.0f, 5.0f), "Iniciar"),
                BuildUI(new Vector2(0.0f, 4.0f), "Hospedagem"),
                BuildHostCtrl()
            };
        }

        internal static GameObject[] BuildClientMenu()
        {
            return new GameObject[]
            {
                BuildUI(new Vector2(0.0f, 5.0f), "Digite o IP"),
                BuildUI(new Vector2(0.0f, 4.0f), "do Hospedeiro"),
                BuildClientCtrl()
            };
        }

        internal static GameObject[] BuildHostGame()
        {
            return new GameObject[] { };
        }
    }
}
