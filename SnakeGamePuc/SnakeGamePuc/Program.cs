using GameEngine;
using GameEngine.Scenes;
using GameEngineASCIIRenderer;
using SnakeGamePuc.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc
{
    internal class Program
    {
        public static void Main()
        {
            GameScene[] m_gameScenes = new GameScene[]
            {
                new MainMenu(),
                new SinglePlayerScene(),
                new MultiplayerMenu(),
                new ClientMenu(),
                new ClientGame(),
                new HostMenu(),
                new HostGame(),
                new LoseScene(),
                new WinScene(),
                new DisconnectScene(),
            };

            GameInstance instance = new GameInstance(new ASCIIRenderer(16, 16, 30), m_gameScenes, "MainMenu", false);

            instance.Run();
        }
    }
}
