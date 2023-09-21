using GameEngine.Patterns;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;

namespace GameEngine.Input
{

    //https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application
    /// <summary>
    /// Provide a way to handle a global keyboard hooks
    /// <remarks>This hook is called in the context of the thread that installed it. 
    /// The call is made by sending a message to the thread that installed the hook.
    /// Therefore, the thread that installed the hook must have a message loop.</remarks>
    /// </summary>
    public sealed class InputSystem
    {
        #region externDlls
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetCurrentThreadId();
        #endregion

        private class InputState : ICloneable
        {
            public InputKey KeysPressed;
            public InputKey KeysHolded;
            public InputKey KeysReleased;

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private LowLevelKeyboardProc m_proc;
        private Thread m_inputThread;
        private IntPtr m_hookId = IntPtr.Zero;
        private bool m_disposed;
        private bool m_run;

        private InputState m_localThreadInputState;

        private InputState m_gameThreadInputState;
        //public InputKey KeysPressed { get => m_gameThreadInputState.KeysPressed; }
        //public InputKey KeysHolded { get => m_gameThreadInputState.KeysHolded; }
        //public InputKey KeysReleased { get => m_gameThreadInputState.KeysReleased; }

        public InputSystem()
        {
            m_proc = HookCallback;
            m_inputThread = new Thread(new ThreadStart(InputLoop));
            m_run = false;
            m_localThreadInputState = new InputState();
            m_gameThreadInputState = new InputState();
            m_gameThreadInputState.KeysPressed = InputKey.None;
            m_gameThreadInputState.KeysHolded = InputKey.None;
            m_gameThreadInputState.KeysReleased = InputKey.None;

            m_localThreadInputState.KeysPressed = InputKey.None;
            m_localThreadInputState.KeysHolded = InputKey.None;
            m_localThreadInputState.KeysReleased = InputKey.None;

        }
        ~InputSystem()
        {
        }
        private void Dispose(bool dispose)
        {
            try
            {
                if (m_disposed)
                    return;

                UnhookWindowsHookEx(m_hookId);
                if (dispose)
                {
                    m_proc = null;
                    GC.SuppressFinalize(this);
                }
                m_disposed = true;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private void OnKeyDown(InputKey _inputKey)
        {
            lock (m_localThreadInputState)
            {
                m_localThreadInputState.KeysHolded |= _inputKey;
                m_localThreadInputState.KeysReleased &= ~_inputKey;
            }
        }
        private void OnKeyUp(InputKey _inputKey)
        {
            lock (m_localThreadInputState)
            {
                m_localThreadInputState.KeysReleased |= _inputKey;
                m_localThreadInputState.KeysHolded &= ~_inputKey;
            }
        }

        private InputKey WindowsKeysToInputKeys(int _vkCode)
        {
            switch ((ConsoleKey)_vkCode)
            {
                case ConsoleKey.A:
                    return InputKey.A;
                case ConsoleKey.B:
                    return InputKey.B;
                case ConsoleKey.C:
                    return InputKey.C;
                case ConsoleKey.D:
                    return InputKey.D;
                case ConsoleKey.E:
                    return InputKey.E;
                case ConsoleKey.F:
                    return InputKey.F;
                case ConsoleKey.G:
                    return InputKey.G;
                case ConsoleKey.H:
                    return InputKey.H;
                case ConsoleKey.I:
                    return InputKey.I;
                case ConsoleKey.J:
                    return InputKey.J;
                case ConsoleKey.K:
                    return InputKey.K;
                case ConsoleKey.L:
                    return InputKey.L;
                case ConsoleKey.M:
                    return InputKey.M;
                case ConsoleKey.N:
                    return InputKey.N;
                case ConsoleKey.O:
                    return InputKey.O;
                case ConsoleKey.P:
                    return InputKey.P;
                case ConsoleKey.Q:
                    return InputKey.Q;
                case ConsoleKey.R:
                    return InputKey.R;
                case ConsoleKey.S:
                    return InputKey.S;
                case ConsoleKey.T:
                    return InputKey.T;
                case ConsoleKey.U:
                    return InputKey.U;
                case ConsoleKey.V:
                    return InputKey.V;
                case ConsoleKey.W:
                    return InputKey.W;
                case ConsoleKey.X:
                    return InputKey.X;
                case ConsoleKey.Y:
                    return InputKey.Y;
                case ConsoleKey.Z:
                    return InputKey.Z;
                case ConsoleKey.D1:
                    return InputKey.Key1;
                case ConsoleKey.D2:
                    return InputKey.Key2;
                case ConsoleKey.D3:
                    return InputKey.Key3;
                case ConsoleKey.D4:
                    return InputKey.Key4;
                case ConsoleKey.Escape:
                    return InputKey.Esc;
                default:
                    return InputKey.None;
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(m_hookId, nCode, wParam, lParam);

            var result = new IntPtr(0);
            if (wParam == WM_KEYDOWN)
            {
                OnKeyDown(WindowsKeysToInputKeys(Marshal.ReadInt32(lParam)));
            }
            else if (wParam == WM_KEYUP)
            {
                OnKeyUp(WindowsKeysToInputKeys(Marshal.ReadInt32(lParam)));
            }

            // in case we processed the message, prevent the system from passing the message to the rest of the hook chain
            // return result.ToInt32() == 0 ? CallNextHookEx(_hookId, nCode, wParam, lParam) : result;
            return CallNextHookEx(m_hookId, nCode, wParam, lParam);
        }
        private void InputLoop()
        {
            m_hookId = SetHook(m_proc);

            Application.Run();

            Dispose(true);
        }

        public void StartInputLoop()
        {
            if (!m_run)
            {
                m_run = true;
                m_inputThread.Start();
            }
        }

        public void StopInputLoop()
        {
            if (m_run)
            {
                m_run = false;
                Application.Exit();
                m_inputThread.Join();
            }
        }

        public void UpdateInputState()
        {
            m_gameThreadInputState.KeysHolded |= m_gameThreadInputState.KeysPressed;
            lock (m_localThreadInputState)
            {
                m_gameThreadInputState.KeysPressed = (~m_gameThreadInputState.KeysPressed) & m_localThreadInputState.KeysHolded;
                m_gameThreadInputState.KeysReleased = m_localThreadInputState.KeysReleased;
                m_localThreadInputState.KeysReleased = InputKey.None;
            }
            m_gameThreadInputState.KeysHolded &= ~m_gameThreadInputState.KeysReleased;
        }

        public bool KeyPressed(InputKey _key) 
        {
            return (m_gameThreadInputState.KeysPressed & _key) != 0;
        }
        public bool KeyHolded(InputKey _key)
        {
            return (m_gameThreadInputState.KeysHolded & _key) != 0;
        }
        public bool KeyReleased(InputKey _key)
        {
            return (m_gameThreadInputState.KeysReleased & _key) != 0;
        }
    }
    
}
