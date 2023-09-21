using GameEngine;
using GameEngine.Rendering;
using GameEngine.Scenes;
using SnakeGamePuc.Scenes;

namespace SnakeGamePuc
{
    public class SnakeGame : GameInstance
    {
        public SnakeGame(IRenderer _renderer, GameScene[] _gameScenes) : base(_renderer, _gameScenes, "MainMenu")
        {

        }
    }
}