using GameEngine.Debug;
using GameEngine.Exceptions;
using GameEngine.Patterns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace GameEngine.Net
{
    public sealed class TcpHost : ThreadedNETModule
    {
        public enum HostState
        {
            Idle,
            StartWaitConnection,
            WaitingConnection,
            Connected
        }

        private const int k_timeoutMSecs = 30000;
        private const int k_maxQueueSize = 50;

        private ConcurrentQueue<byte[]> m_sendBuffer;
        private ConcurrentQueue<byte[]> m_receiveBuffer;
        private HostState m_currentState;
        private Socket m_listener;
        private Socket m_handler;
        

        //Accept Data
        private IAsyncResult m_asyncAccept;
        private Stopwatch m_waitAcceptStopWatch;

        public HostState State { get { return m_currentState; } }

        internal TcpHost() : base()
        {
            m_listener = null;
            m_sendBuffer = new ConcurrentQueue<byte[]>();
            m_receiveBuffer = new ConcurrentQueue<byte[]>();
            m_currentState = HostState.Idle;
            m_sleep = true;
        }

        private void OnAcceptConnectionResult(IAsyncResult _result)
        {
            Socket? result = null;
            try
            {
                result = (Socket?)_result.AsyncState;
                int x = result.Available;

            }catch (System.ObjectDisposedException ex)
            {
                GameInstance.Debug.LogWarningMsg("Timed Out!");
                return;
            }
            if (State != HostState.WaitingConnection) throw new NetException("Wrong Host State! Should be WaitingConnection");

            m_asyncAccept = null;
            if (result != null)
            {
                m_waitAcceptStopWatch.Stop();
                m_handler = result.EndAccept(_result);
                m_currentState = HostState.Connected;
            }
            else
            {
                GameInstance.Debug.LogWarningMsg("Accept failed!");
                m_currentState = HostState.Idle;
                m_sleep = true;
            }
        }

        private void Idle()
        {
        }

        private void StartWaitConnection()
        {
            if (State != HostState.StartWaitConnection) throw new NetException("Wrong Host State! Should be StartWaitConnection");

            m_waitAcceptStopWatch = new Stopwatch();

            m_waitAcceptStopWatch.Start();
            m_currentState = HostState.WaitingConnection;
            m_asyncAccept = m_listener.BeginAccept(OnAcceptConnectionResult, m_listener);
        }

        private void WaitingConnection()
        {
            if (m_waitAcceptStopWatch.ElapsedMilliseconds >= k_timeoutMSecs) 
            {
                if(m_asyncAccept != null && m_asyncAccept.IsCompleted) throw new NetException("Wrong Host State! Should be Listening or Idle!");

                m_waitAcceptStopWatch.Stop();
                m_listener.Close();
                m_currentState = HostState.Idle;
            }
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            if (m_receiveBuffer.Count < k_maxQueueSize)
            {
                if (m_handler.Receive(buffer) > 4)
                {
                    m_receiveBuffer.Enqueue(buffer);
                }
            }
            else
                GameInstance.Debug.LogErrorMsg("TCP Receive Queue FULL!");
        }

        private void SendMessages()
        {
            byte[] msg;
            while (m_sendBuffer.Count > 0)
            {
                if (m_sendBuffer.TryDequeue(out msg) && msg.Length > 0)
                {
                    if(m_handler.Send(msg) != msg.Length)
                    {
                        m_listener?.Close();
                        m_handler.Close();
                        m_currentState = HostState.Idle;
                        GameInstance.Debug.LogWarningMsg("TCP Disconnected!");
                    }
                }
            }
        }

        protected override void ModuleLoop()
        {
            base.ModuleLoop();
            switch(m_currentState) 
            {
                case HostState.Idle:
                    Idle();
                    break;
                case HostState.StartWaitConnection:
                    StartWaitConnection();
                    break;
                case HostState.WaitingConnection:
                    WaitingConnection();
                    break;
                case HostState.Connected:
                    SendMessages();
                    break;
            }
        }

        protected override void OnModuleStart()
        {
            m_currentState = HostState.Idle;
            m_sleep = true;
        }

        protected override void OnModuleStop()
        {
            m_listener?.Close();
            m_handler?.Close();
        }

        protected override void PreThreadModuleStart()
        {
        }

        protected override void PreThreadModuleStop()
        {
        }

        public IPEndPoint GetConnectedEnpoint()
        {
            IPEndPoint result = null;
            if (m_currentState == HostState.Connected)
                result = m_handler.RemoteEndPoint as IPEndPoint;
            return result;
        }

        public bool ListenToPort(int _port)
        {
            if (State != HostState.Idle) return false;
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
                m_listener = new(
                endPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

                m_listener.Bind(endPoint);
                m_listener.Listen(1); //Apenas aceita 1 conexao s

                m_sleep = false;
                m_currentState = HostState.StartWaitConnection;

                return true;
            }catch(SocketException e) 
            {
                GameInstance.Debug.LogErrorMsg(e.ToString());
                return false;
            }
        }
        public bool SendData(byte[] _data)
        {
            if (m_currentState != HostState.Connected) return false;

            if (m_sendBuffer.Count < k_maxQueueSize)
            {
                m_sendBuffer.Enqueue(_data);
                return true;
            }
            else
                GameInstance.Debug.LogErrorMsg("TCP Send Queue FULL!");

            return false;
        }
        /*
        public bool ReceiveData(out byte[][] _data)
        {
            _data = null;
            if (m_currentState != HostState.Connected) return false;

            if (m_receiveBuffer.Count < k_maxQueueSize)
            {
                _data = m_receiveBuffer.ToArray();
                return true;
            }
            else
                GameInstance.Debug.LogErrorMsg("TCP Receive Queue FULL!");

            return false;
        }
        */

        public override void StopNetModule()
        {
            throw new NotImplementedException();
        }
    }

}
