using GameEngine.Debug;
using GameEngine.Exceptions;
using GameEngine.Patterns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace GameEngine.Net
{
    internal sealed class TcpHost : ThreadedModule
    {
        internal enum HostState
        {
            Idle,
            StartWaitConnection,
            WaitingConnection,
            Listening
        }

        private const int k_maxSendBufferSize = 128;
        private const int k_timeoutSecs = 60;

        private IPAddress m_ipAddress;
        private ConcurrentQueue<byte[]> m_sendBuffer;
        private HostState m_currentState;
        private Socket m_listener;
        private Socket m_handler;
        private int m_waitAcceptTime;

        internal HostState State { get { return m_currentState; } }

        internal TcpHost() : base()
        {
            m_ipAddress = null;
            m_listener = null;
            m_sendBuffer = new ConcurrentQueue<byte[]>();
            m_currentState = HostState.Idle;
        }

        private void OnAcceptConnectionResult(IAsyncResult _result)
        {
            if(State != HostState.WaitingConnection) throw new NetException("Wrong Host State! Should be WaitingConnection");
            Socket? result = (Socket)_result.AsyncState;

            if (result != null)
            {
                m_handler = result.EndAccept(_result);
                m_currentState = HostState.Listening;
            }
            else
            {
                GameInstance.Debug.LogWarningMsg("Accept failed!");
                m_currentState = HostState.Idle;
            }
        }

        private void Idle()
        {
        }

        private void StartWaitConnection()
        {
            if (State != HostState.StartWaitConnection) throw new NetException("Wrong Host State! Should be StartWaitConnection");

            m_waitAcceptTime = 0;
            m_currentState = HostState.WaitingConnection;
            m_listener.BeginAccept(OnAcceptConnectionResult, m_listener);
        }

        private void WaitingConnection()
        {

            if(m_listener.Connected) 
                m_currentState = HostState.Listening;
            else
                m_currentState = HostState.Idle;

        }

        private void ListenForMsgs()
        {
            // Receive message.
            var buffer = new byte[1_024];
            var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            var eom = "<|EOM|>";
            if (response.IndexOf(eom) > -1 /* is end of message */)
            {
                Console.WriteLine(
                    $"Socket server received message: \"{response.Replace(eom, "")}\"");

                var ackMessage = "<|ACK|>";
                var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                await handler.SendAsync(echoBytes, 0);
                Console.WriteLine(
                    $"Socket server sent acknowledgment: \"{ackMessage}\"");

                break;
            }
        }
        protected override void ModuleLoop()
        {
            byte[] msg;
            while (m_sendBuffer.Count > 0)
            {
                if (m_sendBuffer.TryDequeue(out msg) && msg.Length > 0)
                {
                }
            }

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
                case HostState.Listening:
                    ListenForMsgs();
                    break;
            }
        }

        protected override void OnModuleStart()
        {
        }

        protected override void OnModuleStop()
        {
            m_listener?.Close();
        }

        protected override void PreThreadModuleStart()
        {
        }

        protected override void PreThreadModuleStop()
        {
        }

        internal bool ListenToPort(int _port)
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

                m_currentState = HostState.StartWaitConnection;

                return true;
            }catch(SocketException e) 
            {
                GameInstance.Debug.LogErrorMsg(e.ToString());
                return false;
            }
        }
    }

}
