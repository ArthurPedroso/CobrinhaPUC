using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;

namespace GameEngine.Net
{
    public class UdpSend : ThreadedNETModule
    {
        public enum UdpSendState
        {
            Idle,
            Sending
        }

        private const int k_maxQueueSize = 5;

        private ConcurrentQueue<byte[]> m_sendBuffer;
        private UdpSendState m_currentState;
        private Socket m_udpSendSocket;
        private EndPoint m_sendTo;

        public UdpSendState State { get { return m_currentState; } }

        internal UdpSend() : base()
        {
            m_sendBuffer = new ConcurrentQueue<byte[]>();
            m_currentState = UdpSendState.Idle;
            m_sleep = true;
        }

        private void Idle()
        {
        }

        private void SendMessages()
        {
            byte[] msg;
            while (m_sendBuffer.Count > 0)
            {
                if (m_sendBuffer.TryDequeue(out msg) && msg.Length > 0)
                {
                    if (m_udpSendSocket.SendTo(msg, m_sendTo) != msg.Length)
                    {
                        m_udpSendSocket.Close();
                        m_currentState = UdpSendState.Idle;
                        GameInstance.Debug.LogWarningMsg("UDP Send Socket Error!");
                    }
                }
            }
        }

        protected override void ModuleLoop()
        {
            base.ModuleLoop();
            switch (m_currentState)
            {
                case UdpSendState.Idle:
                    Idle();
                    break;
                case UdpSendState.Sending:
                    SendMessages();
                    break;
            }
        }

        protected override void OnModuleStart()
        {
        }

        protected override void OnModuleStop()
        {
            m_udpSendSocket?.Close();
            m_sendBuffer.Clear();
            m_sendTo = null;
        }

        protected override void PreThreadModuleStart()
        {
            m_currentState = UdpSendState.Idle;
            m_sleep = true;
        }

        protected override void PreThreadModuleStop()
        {
        }
        public bool StartUdpSend(string _ip, int _port)
        {
            if (State != UdpSendState.Idle) return false;
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
                m_udpSendSocket = new(
                endPoint.AddressFamily,
                SocketType.Dgram,
                ProtocolType.Udp);
                m_sendTo = endPoint;

                m_sleep = false;
                m_currentState = UdpSendState.Sending;

                return true;
            }
            catch (Exception e)
            {
                GameInstance.Debug.LogErrorMsg(e.ToString());
                return false;
            }
        }
        public bool SendData(byte[] _data)
        {
            if (m_currentState != UdpSendState.Sending) return false;

            if (m_sendBuffer.Count < k_maxQueueSize)
            {
                m_sendBuffer.Enqueue(_data);
                return true;
            }
            else
                GameInstance.Debug.LogErrorMsg("TCP Send Queue FULL!");

            return false;
        }
    }
}
