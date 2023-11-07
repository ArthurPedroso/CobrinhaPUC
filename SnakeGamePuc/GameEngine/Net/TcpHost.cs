﻿using GameEngine.Debug;
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
    public sealed class TcpHost : ThreadedModule
    {
        public enum HostState
        {
            Idle,
            StartWaitConnection,
            WaitingConnection,
            Listening
        }

        private const int k_maxSendBufferSize = 128;
        private const int k_timeoutMSecs = 60000;

        private IPAddress m_ipAddress;
        private ConcurrentQueue<byte[]> m_sendBuffer;
        private HostState m_currentState;
        private Socket m_listener;
        private Socket m_handler;

        //Accept Data
        private IAsyncResult m_asyncAccept;
        private Stopwatch m_waitAcceptStopWatch;

        public HostState State { get { return m_currentState; } }

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

            m_waitAcceptStopWatch = new Stopwatch();
            m_currentState = HostState.WaitingConnection;
            m_listener.BeginAccept(OnAcceptConnectionResult, m_listener);
        }

        private void WaitingConnection()
        {
            if(m_waitAcceptStopWatch.ElapsedMilliseconds >= k_timeoutMSecs) 
            {
                if(m_asyncAccept.IsCompleted) throw new NetException("Wrong Host State! Should be Listening or Idle!");

                m_listener.EndAccept(null);
            }
        }

        private void ListenForMsgs()
        {
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
