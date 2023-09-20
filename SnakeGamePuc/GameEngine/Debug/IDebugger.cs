using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Debug
{
    public interface IDebugger
    {
        public void LogMsg(string _msg);
        public void LogWarningMsg(string _msg);
        public void LogErrorMsg(string _msg);
    }
}
