using GameEngine.Patterns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GameEngine.Debug
{
    internal class GameEngineDebugger : IDebugger
    {
        private const int k_maxBufferSize = 100;
        private readonly string r_logPath = "log_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
        private Thread m_debuggerThread;
        private ConcurrentQueue<string> m_msgBuffer;
        private bool m_run;
        internal GameEngineDebugger()
        {
            m_debuggerThread = new Thread(new ThreadStart(DebugLoop));
            m_msgBuffer = new ConcurrentQueue<string>();
            m_run = false;
        }

        private void DebugLoop()
        {
            string msg;
            while (m_run)
            {
                while (m_msgBuffer.Count > 0)
                {
                    if (m_msgBuffer.TryDequeue(out msg) && !string.IsNullOrEmpty(msg))
                    {
                        StreamWriter sw = File.AppendText(r_logPath);
                        sw.WriteLine("[" + DateTime.Now.ToString() + "]:" + msg);
                        sw.Close();
                    }
                }
                Thread.Sleep(500);
            }
        }
        private void CreateDebbugFile()
        {
            if (!File.Exists(r_logPath))
            {
                File.CreateText(r_logPath).Close();
            }
        }

        internal void StartDebugger()
        {
            if (!m_run)
            {
                m_run = true;
                CreateDebbugFile();
                m_debuggerThread.Start();
            }
        }
        internal void StopDebugger()
        {
            if (m_run)
            {
                m_run = false;
                m_debuggerThread.Join();
            }
        }

        public void LogMsg(string _msg)
        {
            if(m_msgBuffer.Count < k_maxBufferSize)
                m_msgBuffer.Enqueue(_msg);
        }

        public void LogWarningMsg(string _msg)
        {
            if (m_msgBuffer.Count < k_maxBufferSize)
                m_msgBuffer.Enqueue("!WARNING! " + _msg);
        }

        public void LogErrorMsg(string _msg)
        {
            if (m_msgBuffer.Count < k_maxBufferSize)
                m_msgBuffer.Enqueue("!ERROR! " + _msg);
        }
    }
}
