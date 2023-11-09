
using System.Net.Sockets;
using System.Net;
using GameEngine.Exceptions;
using System.Diagnostics;
using System.Windows.Forms;

namespace GameEngine.Net
{
    public class UdpReceive : ThreadedNETModule
    {
        public enum UdpReceiveState
        {
            Idle,
            StartReceiving,
            Receiving
        }


        private byte[] m_receiveBuffer;
        private byte[] m_threadBuffer;
        private UdpReceiveState m_currentState;
        private Socket m_udpReceiveSocket;
        private IAsyncResult m_receiveResult;
        private Stopwatch m_waitReceiveStopWatch;

        public UdpReceiveState State { get { return m_currentState; } }

        internal UdpReceive() : base()
        {
            m_currentState = UdpReceiveState.Idle;
            m_sleep = true;
            m_receiveBuffer = new byte[1024];
        }

        private void OnReceive(IAsyncResult _result)
        {

            Socket? result = null;
            try
            {
                result = (Socket?)_result.AsyncState;
                int x = result.Available;
            }
            catch (ObjectDisposedException ex)
            {
                GameInstance.Debug.LogWarningMsg("Timed Out!");
                return;
            }
            if (State != UdpReceiveState.Receiving) throw new NetException("Wrong Host State! Should be WaitingConnection");

            m_receiveResult = null;
            if (result != null)
            {
                try
                {
                    int bytesReceived = result.EndReceive(_result);
                    if (bytesReceived > 1)
                    {
                        lock (m_threadBuffer)
                        {
                            m_threadBuffer = new byte[bytesReceived];
                            Array.Copy(m_receiveBuffer, m_threadBuffer, bytesReceived);
                        }
                    }
                    m_currentState = UdpReceiveState.StartReceiving;
                    return;

                }
                catch (Exception ex)
                {
                    GameInstance.Debug.LogErrorMsg(ex.ToString());
                }
            }

            GameInstance.Debug.LogWarningMsg("Receive failed!");
            m_currentState = UdpReceiveState.StartReceiving;
        }

        private void Idle()
        {
            if (m_sleep == false)
            {
                if(m_udpReceiveSocket != null) m_udpReceiveSocket.Close();
                m_sleep = true;
            }
        }
        private void StartReceiving()
        {
            m_receiveResult = m_udpReceiveSocket.BeginReceive(m_receiveBuffer, 0, m_receiveBuffer.Length, SocketFlags.None, OnReceive, m_udpReceiveSocket);
        }
        private void ReceiveMessages()
        {
            /*
            if (m_waitReceiveStopWatch.ElapsedMilliseconds >= 500)
            {
                if (m_receiveResult != null && m_receiveResult.IsCompleted) throw new NetException("Wrong Host State! Should be Listening or Idle!");

                m_waitReceiveStopWatch.Stop();
                m_client.Close();
                m_currentState = ClientState.Idle;
            }
            */
        }


        protected override void ModuleLoop()
        {
            base.ModuleLoop();
            switch (m_currentState)
            {
                case UdpReceiveState.Idle:
                    Idle();
                    break;
                case UdpReceiveState.StartReceiving:
                    StartReceiving();
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

                m_udpReceiveSocket.Bind(endPoint);

                m_currentState = UdpReceiveState.StartReceiving;
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
                    m_threadBuffer = null;
                }
            }

            return success;
        }

        public override void StopNetModule()
        {
            throw new NotImplementedException();
        }
    }
}
