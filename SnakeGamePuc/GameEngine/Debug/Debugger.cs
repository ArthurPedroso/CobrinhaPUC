using GameEngine.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Debug
{
    internal abstract class GameEngineDebuggerBase : ThreadedModule, IDebugger
    {
        public abstract void LogErrorMsg(string _msg);

        public abstract void LogMsg(string _msg);

        public abstract void LogWarningMsg(string _msg);
    }
}
