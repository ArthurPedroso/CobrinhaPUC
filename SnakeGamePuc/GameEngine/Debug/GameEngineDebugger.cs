using GameEngine.Patterns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Interop;

namespace GameEngine.Debug
{
    internal class GameEngineDebugger : GameEngineDebuggerBase
    {
        private const int k_maxBufferSize = 100;
        private readonly string r_logPath = "log_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
        private ConcurrentQueue<string> m_msgBuffer;
        internal GameEngineDebugger() : base()
        {
            m_msgBuffer = new ConcurrentQueue<string>();
        }
        private void CreateDebbugFile()
        {
            if (!File.Exists(r_logPath))
            {
                File.CreateText(r_logPath).Close();
            }
        }

        protected override void ModuleLoop()
        {
            string msg;
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

        protected override void OnModuleStart()
        {
        }

        protected override void OnModuleStop()
        {
        }

        protected override void PreThreadModuleStart()
        {
            CreateDebbugFile();
        }

        protected override void PreThreadModuleStop()
        {
        }

        public override void LogMsg(string _msg)
        {
            if(m_msgBuffer.Count < k_maxBufferSize)
                m_msgBuffer.Enqueue(_msg);
        }

        public override void LogWarningMsg(string _msg)
        {
            if (m_msgBuffer.Count < k_maxBufferSize)
                m_msgBuffer.Enqueue("!WARNING! " + _msg);
        }

        public override void LogErrorMsg(string _msg)
        {
            if (m_msgBuffer.Count < k_maxBufferSize)
                m_msgBuffer.Enqueue("!ERROR! " + _msg);
        }
    }
}
