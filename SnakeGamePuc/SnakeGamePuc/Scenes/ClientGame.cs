using GameEngine;
using GameEngine.Scenes;
using SnakeGamePuc.Scripts;
using SnakeGamePuc.Scripts.NetGame;
using SnakeGamePuc.Scripts.NetGame.ClientGame;
using SnakeGamePuc.Scripts.NetGame.HostGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scenes
{
    internal class ClientGame : GameScene
    {
        public ClientGame() : base("ClientGame")
        {
        }

        public override void BuildScene()
        {
            GameObject shadowSnake = ObjsBuilders.BuildClientShadowSnakeController();
            AddObj(shadowSnake);


            GameObject snake = ObjsBuilders.BuildClientSnakeController();
            AddObj(snake);

            GameObject snakeGame = ObjsBuilders.BuildClientSnakeGameCtrl();
            snakeGame.GetComponent<ClientGameCtrl>().SnakeCtrl = snake.GetComponent<ClientSnakeCtrl>();
            snakeGame.GetComponent<ClientGameCtrl>().ShadowSnakeCtrl = shadowSnake.GetComponent<ShadowSnakeCtrl>();
            AddObj(snakeGame);
        }
    }
}
