using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Exceptions
{
    [Serializable]
    internal abstract class GameEngineException : Exception
    { 
    }
    [Serializable]
    internal class MathException : GameEngineException
    {
    }
}
