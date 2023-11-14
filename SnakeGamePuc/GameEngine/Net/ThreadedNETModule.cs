using GameEngine.Patterns;

namespace GameEngine.Net
{
    public abstract class ThreadedNETModule : ThreadedModule
    {
        private const int k_sleepTimeMS = 500;

        protected bool m_sleep;

        protected ThreadedNETModule()
        {
            m_thread = null;
        }

        public bool NETRunning { get => Running; }

        protected override void ModuleLoop()
        {
            if(m_sleep) Thread.Sleep(k_sleepTimeMS);
        }

        public void StopNetModule()
        {
            StopModuleThread();
            m_thread = null;
        }

        public void StartNetModule()
        {
            if(m_thread == null) m_thread = new Thread(new ThreadStart(ThreadLoop));
            StartModuleThread();
        }
    }
}
