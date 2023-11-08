using GameEngine;
using GameEngine.Components;
using GameEngine.Components.Scripts;
using GameEngine.GEMath;
using GameEngine.Input;
using GameEngine.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.ClientMenu
{
    internal class ClientMenuCtrl : Script
    {
        private enum ClientMenuState
        {
            Default,
            EditingIP,
            StartTcpConnect,
            WaitingConnection,
            Done
        }
        private const float k_errorMsgTime = 5.0f;

        private List<UI> m_clientUI;
        private UIInputField m_inputField;
        private UI m_clientStatus;

        private ClientMenuState m_menuState;
        private float m_timer;
        private int m_inputtedPort;
        private List<string> m_inputtedIp;

        public ClientMenuCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            m_clientUI = new List<UI>();
            m_menuState = ClientMenuState.Default;
            m_timer = 0;
            m_inputtedIp = new List<string>();
        }

        private void Timer()
        {
            if (m_timer > 0)
            {
                m_timer -= GameInstance.Renderer.GetDeltaTime();
                if (m_timer <= 0)
                {
                    m_clientStatus.ShowText = false;
                    foreach (UI ui in m_clientUI) ui.ShowText = true;
                    m_menuState = ClientMenuState.Default;
                }
            }
        }

        private void StartTcpConnection()
        {
            if (GameInstance.ClientTCP.State == TcpClient.ClientState.Idle)
            {
                string ipAddress = "";
                for(int i = 0; i < m_inputtedIp.Count; i++)
                {
                    ipAddress += m_inputtedIp[i];
                    if (i != m_inputtedIp.Count - 1) ipAddress += '.';
                }
                if(GameInstance.ClientTCP.ConnectToHost(ipAddress, m_inputtedPort))
                {
                    m_clientStatus.UiText = "Connecting";
                    m_menuState = ClientMenuState.WaitingConnection;

                }
                else
                {
                    m_timer = k_errorMsgTime;
                    m_clientStatus.UiText = "Ip failed";
                    m_menuState = ClientMenuState.Done;
                }
            }
            else
            {
                throw new Exception("Tcp not done!");
            }
        }

        private void WaitForConnection()
        {
            if (GameInstance.ClientTCP.State == TcpClient.ClientState.Idle)
            {
                m_timer = k_errorMsgTime;
                m_clientStatus.UiText = "Timed Out";
                m_menuState = ClientMenuState.Done;
            }
            else if (GameInstance.ClientTCP.State == TcpClient.ClientState.Connected)
            {
                IPEndPoint remote = GameInstance.ClientTCP.GetConnectedEnpoint();
                GameInstance.UDPReceive.StartUdpReceive(remote.Port);
                GameInstance.UDPSend.StartUdpSend(remote);
                m_menuState = ClientMenuState.Done;
                GameInstance.SceneMan.LoadScene("ClientGame");
            }
        }

        private void EditingIp()
        {
            if (m_inputtedIp.Count < 4)
            {
                if (!m_inputField.EditingText)
                {
                    if (m_inputField.UiText.Length > 0)
                    {
                        m_inputtedIp.Add(m_inputField.UiText);
                        m_clientStatus.UiText = "Ip Pt " + (m_inputtedIp.Count + 1).ToString();
                        m_inputField.UiText = "";
                    }
                    else
                    {
                        m_inputField.StartEditingText();
                    }
                }
            }
            else
            {
                if(!m_inputField.EditingText)
                {
                    if(m_inputField.UiText.Length > 0)
                    {
                        m_inputtedPort = int.Parse(m_inputField.UiText);
                        m_menuState = ClientMenuState.StartTcpConnect;
                        m_inputField.ShowText = false;
                        m_inputField.UiText = "";
                        m_clientStatus.UiText = "Conectando";
                        m_inputField.MaxSize = 3;
                    }
                    else
                    {
                        m_inputField.StartEditingText();
                        m_inputField.MaxSize = 4;
                        m_clientStatus.UiText = "Port";
                    }
                }
            }
        }

        private void BuildClientUI()
        {
            GameObject obj = ObjsBuilders.BuildUI(new Vector2(0.0f, 0.0f), "1.Conectar");
            GameInstance.Instantiate(obj);

            m_clientUI.Add(obj.GetComponent<UI>());

            obj = ObjsBuilders.BuildUI(new Vector2(0.0f, -1.0f), "2.Voltar");
            GameInstance.Instantiate(obj);

            m_clientUI.Add(obj.GetComponent<UI>());

            obj = ObjsBuilders.BuildUI(new Vector2(0.0f, 0.0f), "");
            GameInstance.Instantiate(obj);
            m_clientStatus = obj.GetComponent<UI>();
            m_clientStatus.ShowText = false;

            obj = ObjsBuilders.BuildInputField(new Vector2(0.0f, -2.0f), "", 3, true, true);
            GameInstance.Instantiate(obj);
            m_inputField = obj.GetComponent<UIInputField>();
            m_inputField.ShowText = false;
        }

        public override void Start()
        {
            BuildClientUI();
        }

        public override void Update()
        {
            switch (m_menuState)
            {
                case ClientMenuState.Default:
                    if (GameInstance.Input.KeyPressed(InputKey.Key1))
                    {
                        m_inputtedIp.Clear();
                        m_clientStatus.UiText = "Ip Pt 1";
                        m_inputField.ShowText = true;
                        m_clientStatus.ShowText = true;

                        foreach (UI ui in m_clientUI) ui.ShowText = false;

                        m_menuState = ClientMenuState.EditingIP;
                    }
                    else if (GameInstance.Input.KeyPressed(InputKey.Key2))
                        GameInstance.SceneMan.LoadScene("MultiplayerMenu");
                    break;
                case ClientMenuState.EditingIP:
                    EditingIp();
                    break;
                case ClientMenuState.StartTcpConnect:
                    StartTcpConnection();
                    break;
                case ClientMenuState.WaitingConnection:
                    WaitForConnection();
                    break;
                case ClientMenuState.Done:
                    break;
            }
            Timer();
        }
    }
}
