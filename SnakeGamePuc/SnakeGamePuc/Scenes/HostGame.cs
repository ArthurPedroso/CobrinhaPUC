using GameEngine;
using GameEngine.Scenes;
using SnakeGamePuc.Scripts;
using SnakeGamePuc.Scripts.NetGame.HostGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scenes
{
    internal class HostGame : GameScene
    {
        public HostGame() : base("HostGame")
        {
        }

        public override void BuildScene()
        {
            GameObject shadowSnake = ObjsBuilders.BuildHostShadowSnakeController();
            AddObj(shadowSnake);


            GameObject snake = ObjsBuilders.BuildHostSnakeController();
            AddObj(snake);

            GameObject snakeGame = ObjsBuilders.BuildHostSnakeGameCtrl();
            snakeGame.GetComponent<HostGameCtrl>().SnakeCtrl = snake.GetComponent<HostSnakeCtrl>();
            snakeGame.GetComponent<HostGameCtrl>().ShadowSnakeCtrl = shadowSnake.GetComponent<HostShadowSnake>();
            AddObj(snakeGame);

            snake.GetComponent<HostSnakeCtrl>().GameCtrl = snakeGame.GetComponent<HostGameCtrl>();
            shadowSnake.GetComponent<HostShadowSnake>().GameCtrl = snakeGame.GetComponent<HostGameCtrl>();
        }
    }
}
