﻿using GameEngine;
using GameEngine.Components.Sprites;
using GameEngine.Components;
using GameEngine.GEMath;
using GameEngine.Net;
using SnakeGamePuc.Scripts.NetGame.HostGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GameEngine.Scenes;
using GameEngine.Input;

namespace SnakeGamePuc.Scripts.NetGame.ClientGame
{
    internal class ClientGameCtrl : MPlayerGameCtrl
    {
        private int m_gameAreaX;
        private int m_gameAreaY;

        public ClientSnakeCtrl SnakeCtrl { private get; set; }
        public ShadowSnakeCtrl ShadowSnakeCtrl { private get; set; }

        protected TcpClient m_tcpClient;
        private GameObject m_apple;

        public ClientGameCtrl(GameObject _attachedGameObject, SoundEmitter _soundEmitter, SoundEmitter _failureSound, SoundEmitter _winSound, SoundEmitter _music) : base(_attachedGameObject, _soundEmitter, _failureSound, _winSound, _music)
        {
        }

        private void CreateShadowApple(Vector2 _coord)
        {
            GameObject apple = new GameObject("Apple");

            apple.AttachComponent(new Transform(apple));
            apple.AttachComponent(new ASCIISprite(apple, 'M'));

            m_apple = apple;
            m_apple.GetComponent<Transform>().Position = _coord;

            GameInstance.Instantiate(apple);
        }
        private void DeleteShadowApple()
        {
            if(m_apple != null) GameInstance.RemoveObj(m_apple);
        }
        private void ParseGameEvent(HostMsg _msg)
        {
            Vector2 pos = new Vector2(_msg.X, _msg.Y);

            if (pos.X > 128) pos.X -= 256;
            if (pos.Y > 128) pos.Y -= 256;


            switch(_msg.T)
            {
                case 1:
                    m_onAppleEatSound.Play();
                    DeleteShadowApple();
                    SnakeCtrl.EatApple();
                    break;
                case 2:
                    m_onAppleEatSound.Play();
                    DeleteShadowApple();
                    break;
                case 3:
                    EndGame(MPlayerEndGameType.Win);
                    break;
                case 4:
                    EndGame(MPlayerEndGameType.Lose);
                    break;
                case 5:
                    CreateShadowApple(pos);
                    break;
                default: 
                    break;
            }
        }
        private void GetGameEvents()
        {
            byte[][] dataArr;
            if(m_tcpClient.ReceiveData(out dataArr))
            {
                foreach (byte[] data in dataArr)
                {
                    if (data.Length >= 3 && data[0] != 0)
                    {
                        ParseGameEvent(new HostMsg(data[0], data[1], data[2]));
                    }
                    else
                        GameInstance.Debug.LogErrorMsg("Empty!");
                }
            }
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

        protected override void TurnOffNet()
        {
            base.TurnOffNet();
            m_tcpClient.StopNetModule();
        }

        private void EndGame(MPlayerEndGameType _endGameType)
        {
            TurnOffNet();
            switch (_endGameType)
            {
                case MPlayerEndGameType.Disconnect:
                    m_music.Stop();
                    m_failureSound.Play();
                    GameInstance.SceneMan.LoadScene("Disconnect");
                    break;
                case MPlayerEndGameType.Lose:
                    m_music.Stop();
                    m_failureSound.Play();
                    GameInstance.SceneMan.LoadScene("LoseScene");
                    break;
                case MPlayerEndGameType.Win:
                    m_music.Stop();
                    m_winSound.Play();
                    GameInstance.SceneMan.LoadScene("WinScene");
                    break;
            }
        }
        protected override bool CheckDisconnect()
        {
            if (base.CheckDisconnect())
                return true;
            else if (m_tcpClient.State != TcpClient.ClientState.Connected)
            { 
                OnDisconnect();
                return true;
            }
            return false;
        }

        protected override void OnDisconnect()
        {
            TurnOffNet();
            GameInstance.SceneMan.LoadScene("DisconnectScene");
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

            if (!CheckDisconnect())
            {
                if (GameInstance.Input.KeyPressed(InputKey.Esc))
                {
                    m_music.Stop();
                    TurnOffNet();
                    GameInstance.SceneMan.LoadScene("MainMenu");
                }
            }
        }

    }
}
