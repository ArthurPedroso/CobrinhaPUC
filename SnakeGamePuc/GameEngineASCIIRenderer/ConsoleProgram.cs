using SnakeGamePuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineASCIIRenderer
{
    public class ConsoleProgram
    {
        [STAThread]
        public static void Main()
        {
            SnakeGame instance = new SnakeGame(new ASCIIRenderer(16, 16, 30));
        }
    }
}