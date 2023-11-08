using GameEngine;
using GameEngine.Components;
using GameEngine.Components.Scripts;
using GameEngine.GEMath;
using GameEngine.Input;
using GameEngine.Net;
using GameEngine.Scenes;
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
            StartTcpHost,
            WaitingConnection,
            Done
        }
        private const float k_errorMsgTime = 5.0f;

        private List<UI> m_hostUI;
        private UI m_hostStatus;

        private HostMenuState m_menuState;
        private float m_timer;
        public HostMenuCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            m_hostUI = new List<UI>();
            m_menuState = HostMenuState.Default;
            m_timer = 0;
        }

        private void Timer()
        {
            if(m_timer > 0)
            {
                m_timer -= GameInstance.Renderer.GetDeltaTime();
                if(m_timer <= 0)
                {
                    m_hostStatus.ShowText = false;
                    foreach (UI ui in m_hostUI) ui.ShowText = true;
                    m_menuState = HostMenuState.Default;
                }
            }
        }

        private void StartTcpHost()
        {
            if (GameInstance.HostTCP.State == TcpHost.HostState.Idle)
            {
                if (GameInstance.HostTCP.ListenToPort(7778))
                {
                    m_hostStatus.UiText = "Waiting";
                    m_menuState = HostMenuState.WaitingConnection;
                }
                else
                {
                    m_timer = k_errorMsgTime;
                    m_hostStatus.UiText = "Failed Bind";
                    m_menuState = HostMenuState.Done;
                }
            }
            else
            {
                throw new Exception("Tcp not done!");
            }

        }
        private void WaitForConnection()
        {
            if (GameInstance.HostTCP.State == TcpHost.HostState.Idle)
            {
                m_timer = k_errorMsgTime;
                m_hostStatus.UiText = "Timed Out";
                m_menuState = HostMenuState.Done;
            }
            else if (GameInstance.HostTCP.State == TcpHost.HostState.Listening)
            {
                m_menuState = HostMenuState.Done;
                GameInstance.SceneMan.LoadScene("HostGame");
            }
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
                        m_hostStatus.UiText = "Starting";
                        m_menuState = HostMenuState.StartTcpHost;
                    }
                    else if (GameInstance.Input.KeyPressed(InputKey.Key2))
                        GameInstance.SceneMan.LoadScene("MultiplayerMenu");
                    break;
                case HostMenuState.StartTcpHost:
                    StartTcpHost();
                    break;
                case HostMenuState.WaitingConnection:
                    WaitForConnection();
                    break;
                case HostMenuState.Done:
                    break;
            }
            Timer();
        }
    }
}
