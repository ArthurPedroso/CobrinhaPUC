using GameEngine.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static GameEngine.Net.TcpHost;

namespace GameEngine.Net
{
    public class TcpClient : ThreadedNETModule
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

        public void AttemptToConnect(string _address, Action<bool> _result)
        {
            //IPAddress[] addresses = Dns.GetHostAddresses(_address);

            //if (addresses.Length <= 0) _result(false);


        }
    }
}
