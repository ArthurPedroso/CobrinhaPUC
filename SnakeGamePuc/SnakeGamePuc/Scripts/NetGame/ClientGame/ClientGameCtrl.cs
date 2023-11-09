using GameEngine;
using GameEngine.Net;
using SnakeGamePuc.Scripts.NetGame.HostGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame.ClientGame
{
    internal class ClientGameCtrl : MPlayerGameCtrl
    {
        private int m_gameAreaX;
        private int m_gameAreaY;

        public ClientSnakeCtrl SnakeCtrl { private get; set; }
        public ShadowSnakeCtrl ShadowSnakeCtrl { private get; set; }

        protected TcpClient m_tcpClient;
        public ClientGameCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }
        private void ParseGameEvent()
        {

        }
        private void GetGameEvents()
        {
            byte[][] data;
            m_tcpClient.ReceiveData
        }
        private void GetShadowSnake()
        {
            byte[] buffer = null;
            if (m_udpReceive.ReceiveData(out buffer))
            {
                ShadowSnakeCtrl.SetShadow(buffer);
            }
        }

        private void SendSnakeToHost()
        {
            byte[] bytes = SnakeCtrl.SerializeSnake();
            if(bytes != null) m_udpSend.SendData(bytes);
        }
        protected override void CheckDisconnect()
        {
            base.CheckDisconnect();
            if(m_tcpClient.State != TcpClient.ClientState.Connected)
                OnDisconnect();
        }

        protected override void OnDisconnect()
        {
            throw new Exception("Disconnected!");
        }

        public override void Start()
        {
            base.Start();
            m_tcpClient = GameInstance.ClientTCP;

            GameObject[] walls = ObjsBuilders.BuildWalls();
            foreach (GameObject wall in walls) { GameInstance.Instantiate(wall); }
            m_gameAreaX = GameInstance.Renderer.GetWidth() - 2;
            m_gameAreaY = GameInstance.Renderer.GetHeight() - 2;
        }

        public override void Update()
        {
            GetShadowSnake();
            SendSnakeToHost();
            GetGameEvents();

            //if (GameInstance.Input.KeyPressed(InputKey.Esc))
            //    GameInstance.QuitGame();
        }

    }
}
