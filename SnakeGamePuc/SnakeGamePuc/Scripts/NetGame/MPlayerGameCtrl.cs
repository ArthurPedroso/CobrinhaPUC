using GameEngine;
using GameEngine.Components;
using GameEngine.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame
{
    internal abstract class MPlayerGameCtrl : Script
    {
        protected UdpReceive m_udpReceive;
        protected UdpSend m_udpSend;

        protected MPlayerGameCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }

        protected abstract void OnDisconnect();

        protected virtual void CheckDisconnect()
        { 
            if(m_udpReceive.State == UdpReceive.UdpReceiveState.Idle ||
               m_udpSend.State == UdpSend.UdpSendState.Idle) 
                OnDisconnect();
        }

        public override void Start()
        {
            m_udpReceive = GameInstance.UDPReceive;
            m_udpSend = GameInstance.UDPSend;
        }
    }
}
