﻿using GameEngine.Patterns;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace GameEngine.Input
{
    
    //https://stackoverflow.com/questions/72058558/how-can-i-add-system-windows-forms-to-wpf-application-when-adding-via-reference
    /// <summary>
    /// Provide a way to handle a global keyboard hooks
    /// <remarks>This hook is called in the context of the thread that installed it. 
    /// The call is made by sending a message to the thread that installed the hook.
    /// Therefore, the thread that installed the hook must have a message loop.</remarks>
    /// </summary>
    public sealed class InputSystem : IDisposable
    {
        private class HookActions
        {
            public HookActions(Action excetue, Action<object> dispose = null)
            {
                Execute = excetue;
                Dispose = dispose;
            }

            public Action Execute { get; set; }
            public Action<object> Dispose { get; set; }

        }
        private class KeyCombination : IEquatable<KeyCombination>
        {
            private readonly bool _canModify;
            public KeyCombination(List<Key> keys)
            {
                _keys = keys ?? new List<Key>();
            }

            public KeyCombination()
            {
                _keys = new List<Key>();
                _canModify = true;
            }

            public void Add(Key key)
            {
                if (_canModify)
                {
                    _keys.Add(key);
                }
            }

            public void Remove(Key key)
            {
                if (_canModify)
                {
                    _keys.Remove(key);
                }
            }

            public void Clear()
            {
                if (_canModify)
                {
                    _keys.Clear();
                }
            }

            public int Count { get { return _keys.Count; } }

            private readonly List<Key> _keys;

            public bool Equals(KeyCombination other)
            {
                return other._keys != null && _keys != null && KeysEqual(other._keys);
            }

            private bool KeysEqual(List<Key> keys)
            {
                if (keys == null || _keys == null || keys.Count != _keys.Count) return false;
                for (int i = 0; i < _keys.Count; i++)
                {
                    if (_keys[i] != keys[i])
                        return false;
                }
                return true;
            }

            public override bool Equals(object obj)
            {
                if (obj is KeyCombination)
                    return Equals((KeyCombination)obj);
                return false;
            }

            public override int GetHashCode()
            {
                if (_keys == null) return 0;

                //http://stackoverflow.com/a/263416
                //http://stackoverflow.com/a/8094931
                //assume keys not going to modify after we use GetHashCode
                unchecked
                {
                    int hash = 19;
                    for (int i = 0; i < _keys.Count; i++)
                    {
                        hash = hash * 31 + _keys[i].GetHashCode();
                    }
                    return hash;
                }
            }

            public override string ToString()
            {
                if (_keys == null)
                    return string.Empty;

                var sb = new StringBuilder((_keys.Count - 1) * 4 + 10);
                for (int i = 0; i < _keys.Count; i++)
                {
                    if (i < _keys.Count - 1)
                        sb.Append(_keys[i] + " , ");
                    else
                        sb.Append(_keys[i]);
                }
                return sb.ToString();
            }
        }
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
        #endregion

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private readonly IntPtr m_hookId = IntPtr.Zero;

        private Dictionary<int, KeyValuePair<KeyCombination, HookActions>> m_hookEvents;
        private KeyCombination m_pressedKeys;
        private LowLevelKeyboardProc m_proc;
        private bool m_disposed;

        public InputSystem()
        {
            m_proc = HookCallback;
            m_hookEvents = new Dictionary<int, KeyValuePair<KeyCombination, HookActions>>();
            m_hookId = SetHook(m_proc);
            m_pressedKeys = new KeyCombination();
        }
        ~InputSystem()
        {
            Dispose(false);
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
                    m_hookEvents = null;
                    m_pressedKeys = null;
                    GC.SuppressFinalize(this);
                }
                m_disposed = true;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private bool ValidateKeys(IEnumerable<Key> keys)
        {
            return keys.All(t => IsKeyValid((int)t));
        }

        private bool IsKeyValid(int key)
        {
            // 'alt' is sys key and hence is disallowed.
            // a - z and shift, ctrl. 
            return key >= 44 && key <= 69 || key >= 116 && key <= 119;
        }
        

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(m_hookId, nCode, wParam, lParam);

            var result = new IntPtr(0);
            if (wParam == (IntPtr)WM_KEYDOWN)
            {
                m_pressedKeys.Add(KeyInterop.KeyFromVirtualKey(Marshal.ReadInt32(lParam))); // vkCode (in KBDLLHOOKSTRUCT) is DWORD (actually it can be 0-254)
                if (m_pressedKeys.Count >= 2)
                {
                    var keysToAction = m_hookEvents.Values.FirstOrDefault(val => val.Key.Equals(m_pressedKeys));
                    if (keysToAction.Value != null)
                    {
                        keysToAction.Value.Execute();
                        // don't try to get the action again after the execute because it may removed already
                        result = new IntPtr(1);
                    }
                }
            }
            else if (wParam == (IntPtr)WM_KEYUP)
            {
                m_pressedKeys.Clear();
            }

            // in case we processed the message, prevent the system from passing the message to the rest of the hook chain
            // return result.ToInt32() == 0 ? CallNextHookEx(_hookId, nCode, wParam, lParam) : result;
            return CallNextHookEx(m_hookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// Un register a keyboard hook event
        /// </summary>
        /// <param name="id">event id to remove</param>
        /// <param name="obj">parameter to pass to dispose method</param>
        public void UnRegisterInput(int id, object obj = null)
        {
            if (m_hookEvents == null || id < 0 || !m_hookEvents.ContainsKey(id)) return;

            var hook = m_hookEvents[id];

            if (hook.Value != null && hook.Value.Dispose != null)
            {
                try
                {
                    hook.Value.Dispose(obj);
                }
                catch (Exception)
                {
                    // need to be define if we need to throw the exception
                }
            }

            m_hookEvents.Remove(id);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Register a keyboard hook event
        /// </summary>
        /// <param name="keys">The short keys. minimum is two keys</param>
        /// <param name="execute">The action to run when the key ocmbination has pressed</param>
        /// <param name="message">Empty if no error occurred otherwise error message</param>
        /// <param name="runAsync">True if the action should execute in the background. -Be careful from thread affinity- Default is false</param>
        /// <param name="dispose">An action to run when unsubscribing from keyboard hook. can be null</param>
        /// <returns>Event id to use when unregister</returns>
        public int RegisterInput(List<Key> keys, Action execute, out string message, bool runAsync = false, Action<object> dispose = null)
        {
            if (m_hookEvents == null)
            {
                message = "Can't register";
                return -1;
            }

            if (keys == null || execute == null)
            {
                message = "'keys' and 'execute' can't be null";
                return -1;
            }

            if (keys.Count < 2)
            {
                message = "You must provide at least two keys";
                return -1;
            }

            if (!ValidateKeys(keys))
            {
                message = "Unallowed key. Only 'shift', 'ctrl' and 'a' - 'z' are allowed";
                return -1;
            }

            var kc = new KeyCombination(keys);
            int id = kc.GetHashCode();
            if (m_hookEvents.ContainsKey(id))
            {
                message = "The key combination is already exist it the application";
                return -1;
            }

            // if the action should run async, wrap it with Task
            Action asyncAction = null;
            if (runAsync)
                asyncAction = () => Task.Run(() => execute);

            m_hookEvents[id] = new KeyValuePair<KeyCombination, HookActions>(kc, new HookActions(asyncAction ?? execute, dispose));
            message = string.Empty;
            return id;
        }
    }
    
}