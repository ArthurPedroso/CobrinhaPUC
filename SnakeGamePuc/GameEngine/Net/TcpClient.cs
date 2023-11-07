using GameEngine.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Net
{
    internal class TcpClient : ThreadedModule
    {
        protected override void ModuleLoop()
        {
            throw new NotImplementedException();
        }

        protected override void OnModuleStart()
        {
            throw new NotImplementedException();
        }

        protected override void OnModuleStop()
        {
            throw new NotImplementedException();
        }

        protected override void PreThreadModuleStart()
        {
            throw new NotImplementedException();
        }

        protected override void PreThreadModuleStop()
        {
            throw new NotImplementedException();
        }

        internal void AttemptToConnect(string _address, Action<bool> _result)
        {
            IPAddress[] addresses = Dns.GetHostAddresses(_address);

            if (addresses.Length <= 0) _result(false);


        }
    }
}
