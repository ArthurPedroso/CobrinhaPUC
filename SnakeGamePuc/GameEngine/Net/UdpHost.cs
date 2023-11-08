using GameEngine.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameEngine.Net.TcpHost;

namespace GameEngine.Net
{
    public class UdpHost : ThreadedNETModule
    {
        protected override void ModuleLoop()
        {
            base.ModuleLoop();
        }

        protected override void OnModuleStart()
        {
            m_sleep = true;
        }

        protected override void OnModuleStop()
        {
        }

        protected override void PreThreadModuleStart()
        {
        }

        protected override void PreThreadModuleStop()
        {
        }
    }
}
