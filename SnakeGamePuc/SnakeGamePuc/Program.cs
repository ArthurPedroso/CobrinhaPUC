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

        [STAThread]
        public static void Main()
        {
            GameScene[] m_gameScenes = new GameScene[]
            {
                new MainMenu(),
                new SinglePlayerScene(),
                new MultiplayerMenu(),
                new ClientMenu(),
                new HostMenu(),
                new LoseScene(),
                new WinScene()
            };
            GameInstance instance = new GameInstance(new ASCIIRenderer(16, 16, 8), m_gameScenes, "MainMenu");
        }
    }
}
