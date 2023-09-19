using GameEngine;
using GameEngine.Rendering;
using GameEngine.Scenes;

namespace SnakeGamePuc
{
    public class SnakeGame : GameInstance
    {

        public SnakeGame(IRenderer _renderer)
        {

            base(_renderer, new GameScene[] { }, "");
        }
    }
}