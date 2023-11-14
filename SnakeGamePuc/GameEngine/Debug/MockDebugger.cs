using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Debug
{
    internal class MockDebugger : GameEngineDebuggerBase
    {

        protected override void ModuleLoop()
        {
        }

        protected override void OnModuleStart()
        {
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

        internal override void StartModuleThread()
        {
        }

        internal override void StopModuleThread()
        {
        }

        public override void LogErrorMsg(string _msg)
        {
        }

        public override void LogMsg(string _msg)
        {
        }

        public override void LogWarningMsg(string _msg)
        {
        }
    }
}
