using System;
using System.Collections.Generic;
using System.Linq;
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
}
