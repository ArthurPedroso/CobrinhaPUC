using GameEngine.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Net
{
    public abstract class ThreadedNETModule : ThreadedModule
    {
        private const int k_sleepTimeMS = 500;

        protected bool m_sleep;

        protected override void ModuleLoop()
        {
            if(m_sleep) Thread.Sleep(k_sleepTimeMS);
        }

        public abstract void StopNetModule();
    }
}
