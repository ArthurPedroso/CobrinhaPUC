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
        protected SoundEmitter m_onAppleEatSound;
        protected SoundEmitter m_failureSound;
        protected SoundEmitter m_winSound;
        protected SoundEmitter m_music;

        protected MPlayerGameCtrl(GameObject _attachedGameObject, SoundEmitter _soundEmitter, SoundEmitter _failureSound, SoundEmitter _winSound, SoundEmitter _music) : base(_attachedGameObject)
        {
            m_onAppleEatSound = _soundEmitter;
            m_failureSound = _failureSound;
            m_winSound = _winSound;
            m_music = _music;
        }

        protected abstract void OnDisconnect();

        protected virtual bool CheckDisconnect()
        {
            if (m_udpReceive.State == UdpReceive.UdpReceiveState.Idle ||
               m_udpSend.State == UdpSend.UdpSendState.Idle)
            {
                OnDisconnect();
                return true;
            }
            return false;
        }

        protected virtual void TurnOffNet()
        {
            m_udpReceive.StopNetModule();
            m_udpSend.StopNetModule();
        }

        public override void Start()
        {
            m_udpReceive = GameInstance.UDPReceive;
            m_udpSend = GameInstance.UDPSend;
            m_music.Play();
        }
    }
}
