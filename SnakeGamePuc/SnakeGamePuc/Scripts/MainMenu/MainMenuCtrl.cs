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
        SoundEmitter m_intro;
        public MainMenuCtrl(GameObject _attachedGameObject, SoundEmitter _intro) : base(_attachedGameObject)
        {
            m_intro = _intro;
        }

        public override void Start()
        {
            m_intro.Play();
        }

        public override void Update()
        {
            if (GameInstance.Input.KeyPressed(InputKey.Key1))
            {
                m_intro.Stop();
                GameInstance.SceneMan.LoadScene("SinglePlayerScene");
            }
            else if (GameInstance.Input.KeyPressed(InputKey.Key2))
            {
                m_intro.Stop();
                GameInstance.SceneMan.LoadScene("MultiplayerMenu");
            }
            else if (GameInstance.Input.KeyPressed(InputKey.Key3))
            {
                m_intro.Stop();
                GameInstance.QuitGame();
            }
        }
    }
}
