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
        public ClientGameCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }
        private void CreateShadowApple(Vector2 _coord)
        {
            GameObject apple = new GameObject("Apple");

            apple.AttachComponent(new Transform(apple));
            apple.AttachComponent(new ASCIISprite(apple, 'M'));

            m_apple = apple;

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

            if (_msg.T == 0)
            {
                CreateShadowApple(pos);
            }
            else if (_msg.T == 1)
            {
                DeleteShadowApple();
                SnakeCtrl.EatApple();
            }
            else if (_msg.T == 2) DeleteShadowApple();
            else if (_msg.T == 3) GameInstance.Debug.LogMsg("YouWIN!");
            else if (_msg.T == 4) GameInstance.Debug.LogMsg("YouLoose!");
        }
        private void GetGameEvents()
        {
            byte[][] dataArr;
            if(m_tcpClient.ReceiveData(out dataArr))
            {
                foreach (byte[] data in dataArr)
                {
                    HostMsg? msg = JsonSerializer.Deserialize<HostMsg>(Encoding.Default.GetString(data));

                    if (msg != null)
                    {
                        ParseGameEvent(msg);
                    }
                    else
                        GameInstance.Debug.LogErrorMsg("Null Json!");
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
