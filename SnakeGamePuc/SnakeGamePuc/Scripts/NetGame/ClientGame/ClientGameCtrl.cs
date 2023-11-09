using GameEngine;
using GameEngine.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame.ClientGame
{
    internal class ClientGameCtrl : MPlayerGameCtrl
    {
        protected TcpClient m_tcpClient;
        public ClientGameCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
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
        }

        public override void Update()
        {
            if (!m_udpSend.SendData(new byte[] { 5, 2 }))
            {
                throw new Exception("Erro send udp");
            }
        }

    }
}
