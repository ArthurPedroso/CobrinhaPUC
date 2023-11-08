using GameEngine.Exceptions;
using GameEngine.Patterns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static GameEngine.Net.TcpHost;

namespace GameEngine.Net
{
    public class TcpClient : ThreadedNETModule
    {
        public enum ClientState
        {
            Idle,
            StartTryConnection,
            WaitingForConnection,
            Connected
        }

        private const int k_maxSendBufferSize = 128;
        private const int k_timeoutMSecs = 30000;

        private IPAddress m_ipAddress;
        private ConcurrentQueue<byte[]> m_sendBuffer;
        private ConcurrentQueue<byte[]> m_listenBuffer;
        private ClientState m_currentState;
        private Socket m_client;


        //Connection Data
        private IAsyncResult m_asyncConnect;
        private Stopwatch m_waitAcceptStopWatch;
        private EndPoint m_connectEndpoint;
        private CancellationToken m_cancelConnection;

        public ClientState State { get { return m_currentState; } }

        internal TcpClient() : base()
        {
            m_ipAddress = null;
            m_client = null;
            m_sendBuffer = new ConcurrentQueue<byte[]>();
            m_currentState = ClientState.Idle;
            m_sleep = true;
        }

        private void OnConnectionResult(IAsyncResult _result)
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
            if (State != ClientState.WaitingForConnection) throw new NetException("Wrong Host State! Should be WaitingConnection");

            m_asyncConnect = null;
            if (result != null)
            {
                try
                {
                    result.EndConnect(_result);
                    m_currentState = ClientState.Connected;
                    return;

                } catch (Exception ex)
                {
                    GameInstance.Debug.LogErrorMsg(ex.ToString());
                }
            }

            GameInstance.Debug.LogWarningMsg("Connect failed!");
            m_currentState = ClientState.Idle;
            m_sleep = true;
        }

        private void Idle()
        {
        }

        private void StartConnection()
        {
            if (State != ClientState.StartTryConnection) throw new NetException("Wrong Host State! Should be StartWaitConnection");

            m_waitAcceptStopWatch = new Stopwatch();

            m_waitAcceptStopWatch.Start();
            m_currentState = ClientState.WaitingForConnection;
            m_asyncConnect = m_client.BeginConnect(m_connectEndpoint, OnConnectionResult, m_client);
        }

        private void WaitingConnection()
        {
            if (m_waitAcceptStopWatch.ElapsedMilliseconds >= k_timeoutMSecs)
            {
                if (m_asyncConnect != null && m_asyncConnect.IsCompleted) throw new NetException("Wrong Host State! Should be Listening or Idle!");

                m_waitAcceptStopWatch.Stop();
                m_client.Close();
                m_currentState = ClientState.Idle;
            }
        }

        private void ListenForMsgs()
        {
        }
        protected override void ModuleLoop()
        {
            base.ModuleLoop();
            /*
            byte[] msg;
            while (m_sendBuffer.Count > 0)
            {
                if (m_sendBuffer.TryDequeue(out msg) && msg.Length > 0)
                {
                }
            }
            */
            switch (m_currentState)
            {
                case ClientState.Idle:
                    Idle();
                    break;
                case ClientState.StartTryConnection:
                    StartConnection();
                    break;
                case ClientState.WaitingForConnection:
                    WaitingConnection();
                    break;
                case ClientState.Connected:
                    ListenForMsgs();
                    break;
            }
        }

        protected override void OnModuleStart()
        {
            m_currentState = ClientState.Idle;
            m_sleep = true;
        }

        protected override void OnModuleStop()
        {
            m_client?.Close();
        }

        protected override void PreThreadModuleStart()
        {
        }

        protected override void PreThreadModuleStop()
        {
        }

        public bool ConnectToHost(string _address, int _port)
        {
            if (State != ClientState.Idle) return false;
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_address), _port);
                m_client = new(
                endPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

                m_connectEndpoint = endPoint;

                m_sleep = false;
                m_currentState = ClientState.StartTryConnection;

                return true;
            }
            catch (Exception e)
            {
                GameInstance.Debug.LogErrorMsg(e.ToString());
                return false;
            }
        }
    }
}
