using GameEngine;
using GameEngine.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame.HostGame
{
    internal class HostGameCtrl : MPlayerGameCtrl
    {
        protected TcpHost m_tcpHost;
        public HostGameCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }
        protected override void CheckDisconnect()
        {
            base.CheckDisconnect();
            if (m_tcpHost.State != TcpHost.HostState.Connected)
                OnDisconnect();
        }

        protected override void OnDisconnect()
        {
            throw new Exception("Disconnected!");
        }

        public override void Start()
        {
            base.Start();
            m_tcpHost = GameInstance.HostTCP;
        }

        public override void Update()
        {

        }
    }
}
