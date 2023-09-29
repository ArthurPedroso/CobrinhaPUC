using GameEngine.Debug;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Net
{
    internal sealed class TcpHost
    {
        private const int k_maxSendBufferSize = 128;
        private IPAddress m_ipAddress;
        private Thread m_tcpThread;
        private ConcurrentQueue<byte[]> m_sendBuffer;
        private bool m_run;
        internal TcpHost()
        {
            m_tcpThread = new Thread(new ThreadStart(TcpLoop));
            m_sendBuffer = new ConcurrentQueue<byte[]>();
            m_run = false;
        }

        private void TcpLoop()
        {
            byte[] msg;
            while (m_run)
            {
                while (m_sendBuffer.Count > 0)
                {
                    if (m_sendBuffer.TryDequeue(out msg) && msg.Length > 0)
                    {
                    }
                }
            }
        }
        internal void StartTcpThread(string _address)
        {
            if (!m_run)
            {
                m_run = true;
                m_ipAddress = Dns.GetHostEntry(_address).AddressList[0];
                m_tcpThread.Start();
            }
        }
        internal void StopTcpThread()
        {
            if (m_run)
            {
                m_run = false;
                m_tcpThread.Join();
            }
        }
    }

}
