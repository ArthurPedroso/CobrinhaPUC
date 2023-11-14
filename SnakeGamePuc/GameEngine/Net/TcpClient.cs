using GameEngine.Exceptions;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

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

        private const int k_maxQueueSize = 50;
        private const int k_timeoutMSecs = 30000;
        private ConcurrentQueue<byte[]> m_sendBuffer;
        private ConcurrentQueue<byte[]> m_receiveBuffer;
        private ClientState m_currentState;
        private Socket m_client;


        //Connection Data
        private IAsyncResult m_asyncConnect;
        private Stopwatch m_waitAcceptStopWatch;
        private EndPoint m_connectEndpoint;

        public ClientState State { get { return m_currentState; } }

        internal TcpClient() : base()
        {
            m_client = null;
            m_sendBuffer = new ConcurrentQueue<byte[]>();
            m_receiveBuffer = new ConcurrentQueue<byte[]>();
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

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[3];
            if (m_receiveBuffer.Count < k_maxQueueSize)
            {

                try
                {
                    if (m_client.Receive(buffer) > 0)
                    {
                        m_receiveBuffer.Enqueue(buffer);
                    }
                }catch(SocketException e)
                {
                    GameInstance.Debug.LogWarningMsg(e.ToString());
                    m_currentState = ClientState.Idle;
                    StopNetModule();
                }
            }
            else
                GameInstance.Debug.LogErrorMsg("TCP Receive Queue FULL!");
        }
        /*
        private void SendMessages()
        {
            byte[] msg;
            while (m_sendBuffer.Count > 0)
            {
                if (m_sendBuffer.TryDequeue(out msg) && msg.Length > 0)
                {
                    if (m_client.Send(msg) != msg.Length)
                    {
                        m_client.Close();
                        m_currentState = ClientState.Idle;
                        GameInstance.Debug.LogWarningMsg("TCP Disconnected!");
                    }
                }
            }
        }
        */

        protected override void ModuleLoop()
        {
            base.ModuleLoop();
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
                    ReceiveMessages();
                    break;
            }
        }

        protected override void OnModuleStart()
        {
        }

        protected override void OnModuleStop()
        {
            m_client?.Close();
            m_sleep = true;
            m_sendBuffer.Clear();
            m_receiveBuffer.Clear();
            m_asyncConnect = null;
            m_waitAcceptStopWatch = null;
            m_connectEndpoint = null;
        }

        protected override void PreThreadModuleStart()
        {
            m_currentState = ClientState.Idle;
            m_sleep = true;
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
        /*
        public bool SendData(byte[] _data)
        {
            if (m_currentState != ClientState.Connected) return false;

            if (m_sendBuffer.Count < k_maxQueueSize)
            {
                m_sendBuffer.Enqueue(_data);
                return true;
            }
            else
                GameInstance.Debug.LogErrorMsg("TCP Send Queue FULL!");

            return false;
        }
        */
        public bool ReceiveData(out byte[][] _data)
        {
            List<byte[]> data= new List<byte[]>();
            byte[] outQueue;
            _data = null;

            if (m_currentState != ClientState.Connected) return false;

            while(m_receiveBuffer.Count > 0)
            {
                if(m_receiveBuffer.TryDequeue(out outQueue))
                {
                    data.Add(outQueue);
                }
                else
                {
                    throw new Exception("Cant dequeue");
                    return false;
                }
            }
            _data = data.ToArray();
            return true;
        }

        public IPEndPoint GetConnectedEnpoint()
        {
            IPEndPoint result = null;
            if (m_currentState == ClientState.Connected)
                result = m_client.RemoteEndPoint as IPEndPoint;
            return result;
        }
    }
}
