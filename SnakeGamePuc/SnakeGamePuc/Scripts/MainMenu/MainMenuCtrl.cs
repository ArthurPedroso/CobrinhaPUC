using GameEngine;
using GameEngine.Components;
using GameEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.MainMenu
{
    internal class MainMenuCtrl : Script
    {
        public MainMenuCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
            if (GameInstance.Input.KeyPressed(InputKey.Key1)) GameInstance.SceneMan.LoadScene("SinglePlayerScene");
            else if(GameInstance.Input.KeyPressed(InputKey.Key2)) GameInstance.QuitGame();
        }
    }
}
