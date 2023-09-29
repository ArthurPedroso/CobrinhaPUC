using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Patterns
{
    internal abstract class ThreadedModule
    {
        protected Thread m_thread;
        private bool m_run;
        internal ThreadedModule()
        {
            m_run = false;
            m_thread = new Thread(new ThreadStart(ThreadLoop));
        }

        private void ThreadLoop()
        {
            OnModuleStart();
            while (m_run)
            {
                ModuleLoop();
            }
            OnModuleStop();
        }

        protected abstract void ModuleLoop();
        protected abstract void OnModuleStart();
        protected abstract void OnModuleStop();
        protected abstract void PreThreadModuleStart();
        protected abstract void PostThreadModuleStop();
        internal void StartModuleThread()
        {
            if (!m_run)
            {
                m_run = true;
                PreThreadModuleStart();
                m_thread.Start();
            }
        }
        internal void StopModuleThread()
        {
            if (m_run)
            {
                m_run = false;
                m_thread.Join();
                PostThreadModuleStop();
            }
        }
    }
}
