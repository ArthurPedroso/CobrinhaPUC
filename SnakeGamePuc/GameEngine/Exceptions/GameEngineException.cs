using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Exceptions
{
    [Serializable]
    internal class GameEngineException : Exception
    {
        public GameEngineException()
        {
        }

        public GameEngineException(string? message) : base(message)
        {
        }
    }
    [Serializable]
    internal class MathException : GameEngineException
    {
        public MathException()
        {
        }

        public MathException(string? message) : base(message)
        {
        }
    }
    [Serializable]
    internal class NetException : GameEngineException
    {
        public NetException()
        {
        }

        public NetException(string? message) : base(message)
        {
        }
    }
}
