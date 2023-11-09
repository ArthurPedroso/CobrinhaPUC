using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame
{
    [System.Serializable]
    internal class HostMsg
    {
        public byte T;
        public byte X;
        public byte Y;

        public HostMsg(byte t, byte x, byte y)
        {
            T = t;
            X = x;
            Y = y;
        }
    }
}
