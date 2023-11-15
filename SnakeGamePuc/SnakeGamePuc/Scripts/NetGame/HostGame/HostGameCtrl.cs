using GameEngine;
using GameEngine.Components;
using GameEngine.GEMath;
using GameEngine.Input;
using GameEngine.Net;

namespace SnakeGamePuc.Scripts.NetGame.HostGame
{
    internal class HostGameCtrl : MPlayerGameCtrl
    {

        private List<Vector2> m_possibleSpawnPos;
        private int m_gameAreaX;
        private int m_gameAreaY;

        protected TcpHost m_tcpHost;

        public HostSnakeCtrl SnakeCtrl { private get; set; }
        public ShadowSnakeCtrl ShadowSnakeCtrl { private get; set; }
        public HostGameCtrl(GameObject _attachedGameObject, SoundEmitter _soundEmitter, SoundEmitter _failureSound, SoundEmitter _winSound, SoundEmitter _music) : base(_attachedGameObject, _soundEmitter, _failureSound, _winSound, _music)
        {
            m_possibleSpawnPos = new List<Vector2>();
        }


        private void SendGameEvent(HostMsg _msg)
        {
            if (_msg.T == 0) return;
            if(!m_tcpHost.SendData(new byte[] { _msg.T, _msg.X, _msg.Y}))
            {
                OnDisconnect();
            }
        }

        private void GetShadowSnake()
        {
            byte[] buffer;
            if(m_udpReceive.ReceiveData(out buffer))
            {
                ShadowSnakeCtrl.SetShadow(buffer);
            }
        }

        private void SendSnakeToClient()
        {
            byte[] bytes = SnakeCtrl.SerializeSnake();
            if (bytes != null) m_udpSend.SendData(bytes);
        }

        private void InstantiateApple()
        {
            m_possibleSpawnPos.Clear();
            Vector2[] snakePos = SnakeCtrl.GetSnakePos();
            Vector2 possiblePos;
            bool posIsPossible;
            for (int y = 0; y < m_gameAreaY; y++)
            {
                possiblePos.Y = -(m_gameAreaY / 2.0f) + y;
                for (int x = 0; x < m_gameAreaX; x++)
                {
                    possiblePos.X = -(m_gameAreaX / 2.0f) + x;
                    posIsPossible = true;
                    foreach (Vector2 sPos in snakePos)
                    {
                        if ((sPos - possiblePos).Magnitude() <= 0.0001f)
                        {
                            posIsPossible = false;
                            break;
                        }
                    }
                    if (posIsPossible) m_possibleSpawnPos.Add(possiblePos);
                }
            }

            if (m_possibleSpawnPos.Count > 0)
            {
                GameObject apple = ObjsBuilders.BuildApple(InstantiateApple);
                Random random = new Random();
                Vector2 pos = m_possibleSpawnPos[random.Next(m_possibleSpawnPos.Count)];

                apple.GetComponent<Transform>().Position = pos;

                GameInstance.Instantiate(apple);

                SendGameEvent(new HostMsg(5, (byte)(int)float.Round(pos.X), (byte)(int)float.Round(pos.Y)));
            }
        }

        protected override void TurnOffNet()
        {
            base.TurnOffNet();
            m_tcpHost.StopNetModule();
        }
        protected override bool CheckDisconnect()
        {
            if (base.CheckDisconnect())
                return true;
            else if (m_tcpHost.State != TcpHost.HostState.Connected)
            {
                OnDisconnect();
                return true;
            }
            return false;
        }

        protected override void OnDisconnect()
        {
            m_failureSound.Play();
            m_music.Stop();
            TurnOffNet();
            GameInstance.SceneMan.LoadScene("DisconnectScene");
        }
        public void OnHostDeath()
        {
            m_failureSound.Play();
            m_music.Stop();
            SendGameEvent(new HostMsg(3, 0, 0));
            TurnOffNet();
            GameInstance.SceneMan.LoadScene("LoseScene");
        }
        public void OnClientDeath()
        {
            m_winSound.Play();
            m_music.Stop();
            SendGameEvent(new HostMsg(4, 0, 0));
            TurnOffNet();
            GameInstance.SceneMan.LoadScene("WinScene");
        }

        public void OnClientApple()
        {
            m_onAppleEatSound.Play();
            SendGameEvent(new HostMsg(1, 0, 0));
        }

        public void OnHostApple()
        {
            m_onAppleEatSound.Play();
            SendGameEvent(new HostMsg(2, 0, 0));
        }

        public override void Start()
        {
            base.Start();
            m_tcpHost = GameInstance.HostTCP;

            GameObject[] walls = ObjsBuilders.BuildWalls();
            foreach (GameObject wall in walls) { GameInstance.Instantiate(wall); }
            m_gameAreaX = GameInstance.Renderer.GetWidth() - 2;
            m_gameAreaY = GameInstance.Renderer.GetHeight() - 2;

            InstantiateApple();
        }

        public override void Update()
        {
            GetShadowSnake();
            SendSnakeToClient();

            if (!CheckDisconnect())
            {
                if (GameInstance.Input.KeyPressed(InputKey.Esc))
                {
                    TurnOffNet();
                    m_music.Stop();
                    GameInstance.SceneMan.LoadScene("MainMenu");
                }
            }
        }
    }
}
