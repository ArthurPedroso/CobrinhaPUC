
using System.Net.Sockets;
using System.Net;
using GameEngine.Exceptions;
using System.Diagnostics;
using System.Windows.Forms;
using static GameEngine.Net.UdpSend;

namespace GameEngine.Net
{
    public class UdpReceive : ThreadedNETModule
    {
        public enum UdpReceiveState
        {
            Idle,
            Receiving
        }


        private byte[] m_receiveBuffer;
        private byte[] m_threadBuffer;
        private UdpReceiveState m_currentState;
        private Socket m_udpReceiveSocket;

        public UdpReceiveState State { get { return m_currentState; } }

        internal UdpReceive() : base()
        {
            m_currentState = UdpReceiveState.Idle;
            m_sleep = true;
            m_receiveBuffer = new byte[512];
            m_threadBuffer = null;
        }

        private void Idle()
        {
            if (m_sleep == false)
            {
                if(m_udpReceiveSocket != null) m_udpReceiveSocket.Close();
                m_sleep = true;
            }
        }
        private void ReceiveMessages()
        {
            int bytesReceived = m_udpReceiveSocket.Receive(m_receiveBuffer);
            if (bytesReceived > 1)
            {
                lock (m_threadBuffer)
                {
                    m_threadBuffer = new byte[bytesReceived];
                    Array.Copy(m_receiveBuffer, m_threadBuffer, bytesReceived);
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
        }

        protected override void OnModuleStop()
        {
            m_udpReceiveSocket?.Close();
            Array.Clear(m_receiveBuffer);
            m_threadBuffer = null;
        }

        protected override void PreThreadModuleStart()
        {
            m_currentState = UdpReceiveState.Idle;
            m_sleep = true;
            m_threadBuffer = new byte[0];
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

                m_udpReceiveSocket.Bind(endPoint);

                m_currentState = UdpReceiveState.Receiving;
                m_sleep = false;

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
            if (m_currentState == UdpReceiveState.Idle) return success;

            lock (m_threadBuffer)
            {
                if (m_threadBuffer.Length > 0)
                {
                    _data = (byte[])m_threadBuffer.Clone();
                    success = true;
                }
            }

            return success;
        }
    }
}
