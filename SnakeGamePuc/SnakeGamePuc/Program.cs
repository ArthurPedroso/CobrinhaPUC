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
                new SinglePlayerScene()
            };
            SnakeGame instance = new SnakeGame(new ASCIIRenderer(16, 16, 30), m_gameScenes);
        }
    }
}
