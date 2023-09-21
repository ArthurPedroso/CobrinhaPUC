using GameEngine;
using GameEngine.Components;
using GameEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts
{
    internal class EndSceneCtrl : Script
    {
        public EndSceneCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
            if (GameInstance.Input.KeyReleased(InputKey.Key1)) GameInstance.SceneMan.LoadScene("MainMenu");
        }
    }
}
