using GameEngine;
using GameEngine.Components;
using GameEngine.Components.Scripts;
using GameEngine.GEMath;
using GameEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.HostMenu
{
    internal class HostMenuCtrl : Script
    {
        private enum HostMenuState
        {
            Default,
            WaitingConnection
        }
        private List<UI> m_hostUI;
        private UI m_hostStatus;

        private HostMenuState m_menuState;

        public HostMenuCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            m_hostUI = new List<UI>();
            m_menuState = HostMenuState.Default;
        }

        private void BuildHostUI()
        {
            GameObject obj = ObjsBuilders.BuildUI(new Vector2(0.0f, 0.0f), "1.Hospedar");
            GameInstance.Instantiate(obj);

            m_hostUI.Add(obj.GetComponent<UI>());

            obj = ObjsBuilders.BuildUI(new Vector2(0.0f, -1.0f), "2.Voltar");
            GameInstance.Instantiate(obj);

            m_hostUI.Add(obj.GetComponent<UI>());

            obj = ObjsBuilders.BuildUI(new Vector2(0.0f, 0.0f), "");
            GameInstance.Instantiate(obj);

            m_hostStatus = obj.GetComponent<UI>();
            m_hostStatus.ShowText = false;
        }

        public override void Start()
        {
            BuildHostUI();
        }

        public override void Update()
        {
            switch(m_menuState)
            {
                case HostMenuState.Default:
                    if (GameInstance.Input.KeyPressed(InputKey.Key1))
                    {
                        foreach (UI ui in m_hostUI) ui.ShowText = false;
                        m_hostStatus.ShowText = true;
                        m_hostStatus.UiText = "Waiting";
                        m_menuState = HostMenuState.WaitingConnection;
                    }
                    else if (GameInstance.Input.KeyPressed(InputKey.Key2))
                        GameInstance.SceneMan.LoadScene("MultiplayerMenu");
                    break;
                case HostMenuState.WaitingConnection:
                    break;
            }
        }
    }
}
