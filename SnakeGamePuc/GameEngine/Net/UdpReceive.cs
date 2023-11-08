using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using static GameEngine.Net.TcpHost;

namespace GameEngine.Net
{
    public class UdpReceive : ThreadedNETModule
    {
        public enum UdpReceiveState
        {
            Idle,
            Receiving
        }

        private readonly byte[] r_receiveBuffer = new byte[65000];

        private byte[] m_receiveBuffer;
        private UdpReceiveState m_currentState;
        private Socket m_udpReceiveSocket;

        public UdpReceiveState State { get { return m_currentState; } }

        internal UdpReceive() : base()
        {
            m_currentState = UdpReceiveState.Idle;
            m_sleep = true;
            m_receiveBuffer = new byte[0];
        }

        private void Idle()
        {
        }
        private void ReceiveMessages()
        {
            int bytesReceived;
            bytesReceived = m_udpReceiveSocket.Receive(r_receiveBuffer);
            if (bytesReceived > 1)
            {
                lock(m_receiveBuffer)
                {
                    m_receiveBuffer = new byte[bytesReceived];
                    Array.Copy(r_receiveBuffer, m_receiveBuffer, bytesReceived);
                }
            }
        }


        protected override void ModuleLoop()
        {
            base.ModuleLoop();
            switch (m_currentState)
            {
                case UdpReceiveState.Idle:
                    Idle();
                    break;
                case UdpReceiveState.Receiving:
                    ReceiveMessages();
                    break;
            }
        }

        protected override void OnModuleStart()
        {
            m_currentState = UdpReceiveState.Idle;
            m_sleep = true;
        }

        protected override void OnModuleStop()
        {
            m_udpReceiveSocket?.Close();
        }

        protected override void PreThreadModuleStart()
        {
        }

        protected override void PreThreadModuleStop()
        {
        }

        public bool StartUdpReceive(int _port)
        {
            if (State != UdpReceiveState.Idle) return false;
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
                m_udpReceiveSocket = new(
                endPoint.AddressFamily,
                SocketType.Dgram,
                ProtocolType.Udp);

                m_sleep = false;
                m_currentState = UdpReceiveState.Receiving;

                return true;
            }
            catch (Exception e)
            {
                GameInstance.Debug.LogErrorMsg(e.ToString());
                return false;
            }
        }
        public bool ReceiveData(out byte[] _data)
        {
            bool success = false;
            _data = null;
            if (m_currentState != UdpReceiveState.Receiving) return success;

            lock (m_receiveBuffer)
            {
                if (m_receiveBuffer.Length > 0)
                {
                    _data = (byte[])m_receiveBuffer.Clone();
                    success = true;
                }
            }

            if(!success) GameInstance.Debug.LogErrorMsg("UDP Receive Queue FULL!");
            return success;
        }
    }
}
