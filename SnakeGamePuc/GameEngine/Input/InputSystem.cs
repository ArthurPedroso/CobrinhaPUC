using System.Runtime.InteropServices;

namespace GameEngine.Input
{
    internal class InputSystem
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string moduleName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);


        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private Thread m_inputThread;
        private bool m_stopThread;

        public KeyPressed KeyPressed { get; private set; }

        internal InputSystem()
        {
            m_inputThread = new Thread(new ThreadStart(PoolInput));
            m_stopThread = false;
            KeyPressed = 0;
        }
        ~InputSystem()
        {
            m_stopThread = true;
            m_inputThread.Join();
        }
        private void PoolInput()
        {

            IntPtr hook = SetHook(HookCallback);
            while (!m_stopThread)
            {
            }
            UnhookWindowsHookEx(hook);
        }
        public void StartPoolingLoop()
        {
            //m_inputThread.Start();
            IntPtr hook = SetHook(HookCallback);
            while (!KeyPressed.HasFlag(KeyPressed.A))
            {
                Console.WriteLine("Uga" + DateTime.Now);
                Thread.Sleep(100);
            }
            UnhookWindowsHookEx(hook);

        }
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            IntPtr hook = SetWindowsHookEx(WH_KEYBOARD_LL, proc, IntPtr.Zero, 0);
            return hook;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Console.WriteLine("Chamou");
            if (nCode >= 0 && wParam == WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                switch ((ConsoleKey)vkCode)
                {
                    case ConsoleKey.A:
                        KeyPressed |= KeyPressed.A;
                        break;
                    case ConsoleKey.B:
                        KeyPressed |= KeyPressed.B;
                        break;
                    case ConsoleKey.C:
                        KeyPressed |= KeyPressed.C;
                        break;
                    case ConsoleKey.D:
                        KeyPressed |= KeyPressed.D;
                        break;
                    case ConsoleKey.E:
                        KeyPressed |= KeyPressed.E;
                        break;
                    case ConsoleKey.F:
                        KeyPressed |= KeyPressed.F;
                        break;
                    case ConsoleKey.G:
                        KeyPressed |= KeyPressed.G;
                        break;
                    case ConsoleKey.H:
                        KeyPressed |= KeyPressed.H;
                        break;
                    case ConsoleKey.I:
                        KeyPressed |= KeyPressed.I;
                        break;
                    case ConsoleKey.J:
                        KeyPressed |= KeyPressed.J;
                        break;
                    case ConsoleKey.K:
                        KeyPressed |= KeyPressed.K;
                        break;
                    case ConsoleKey.L:
                        KeyPressed |= KeyPressed.L;
                        break;
                    case ConsoleKey.M:
                        KeyPressed |= KeyPressed.M;
                        break;
                    case ConsoleKey.N:
                        KeyPressed |= KeyPressed.N;
                        break;
                    case ConsoleKey.O:
                        KeyPressed |= KeyPressed.O;
                        break;
                    case ConsoleKey.P:
                        KeyPressed |= KeyPressed.P;
                        break;
                    case ConsoleKey.Q:
                        KeyPressed |= KeyPressed.Q;
                        break;
                    case ConsoleKey.R:
                        KeyPressed |= KeyPressed.R;
                        break;
                    case ConsoleKey.S:
                        KeyPressed |= KeyPressed.S;
                        break;
                    case ConsoleKey.T:
                        KeyPressed |= KeyPressed.T;
                        break;
                    case ConsoleKey.U:
                        KeyPressed |= KeyPressed.U;
                        break;
                    case ConsoleKey.V:
                        KeyPressed |= KeyPressed.V;
                        break;
                    case ConsoleKey.W:
                        KeyPressed |= KeyPressed.W;
                        break;
                    case ConsoleKey.X:
                        KeyPressed |= KeyPressed.X;
                        break;
                    case ConsoleKey.Y:
                        KeyPressed |= KeyPressed.Y;
                        break;
                    case ConsoleKey.Z:
                        KeyPressed |= KeyPressed.Z;
                        break;
                    default:
                        break;
                }
            }
            return CallNextHookEx(0, nCode, wParam, lParam);
        }
        public static void Main(string[] _args)
        {
            InputSystem input = new InputSystem();
            input.StartPoolingLoop();
            //while (!input.KeyPressed.HasFlag(KeyPressed.A)) Console.WriteLine("uga");
        }
    }
}
