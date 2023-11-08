using GameEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components.Scripts
{
    public class UIInputField : UI
    {
        private readonly bool r_onlyNums;
        private bool m_editingText;
        
        public bool EditingText { get => m_editingText; }
        public int MaxSize { get; set; }

        public UIInputField(GameObject _attachedGameObject, string _text, int _maxSize = 999999, bool _centerText = true, bool _onlyNums = false) : base(_attachedGameObject, _text, _centerText)
        {
            m_editingText = false;
            r_onlyNums = _onlyNums;
            MaxSize = _maxSize;
        }

        private void EditText()
        {

            InputSystem input = GameInstance.Input;

            if (input.KeyPressed(InputKey.Backspace))
            {
                UiText = UiText.Substring(0, UiText.Length - 1);
            }
            else if (input.KeyPressed(InputKey.Enter))
            {
                FinishEditingText();
            }
            else if (UiText.Length >= MaxSize)
            {
                return;
            }
            else if (r_onlyNums)
            {
                if (input.KeyPressed(InputKey.Key0) || input.KeyPressed(InputKey.KeyPad0))
                    UiText += "0";
                else if (input.KeyPressed(InputKey.Key1) || input.KeyPressed(InputKey.KeyPad1))
                    UiText += "1";
                else if (input.KeyPressed(InputKey.Key2) || input.KeyPressed(InputKey.KeyPad2))
                    UiText += "2";
                else if (input.KeyPressed(InputKey.Key3) || input.KeyPressed(InputKey.KeyPad3))
                    UiText += "3";
                else if (input.KeyPressed(InputKey.Key4) || input.KeyPressed(InputKey.KeyPad4))
                    UiText += "4";
                else if (input.KeyPressed(InputKey.Key5) || input.KeyPressed(InputKey.KeyPad5))
                    UiText += "5";
                else if (input.KeyPressed(InputKey.Key6) || input.KeyPressed(InputKey.KeyPad6))
                    UiText += "6";
                else if (input.KeyPressed(InputKey.Key7) || input.KeyPressed(InputKey.KeyPad7))
                    UiText += "7";
                else if (input.KeyPressed(InputKey.Key8) || input.KeyPressed(InputKey.KeyPad8))
                    UiText += "8";
                else if (input.KeyPressed(InputKey.Key9) || input.KeyPressed(InputKey.KeyPad9))
                    UiText += "9";
            }
            else
            {
                if (input.KeyPressed(InputKey.A))
                    UiText += "A";
                else if (input.KeyPressed(InputKey.B))
                    UiText += "B";
                else if (input.KeyPressed(InputKey.C))
                    UiText += "C";
                else if (input.KeyPressed(InputKey.D))
                    UiText += "D";
                else if (input.KeyPressed(InputKey.E))
                    UiText += "E";
                else if (input.KeyPressed(InputKey.F))
                    UiText += "F";
                else if (input.KeyPressed(InputKey.G))
                    UiText += "G";
                else if (input.KeyPressed(InputKey.H))
                    UiText += "H";
                else if (input.KeyPressed(InputKey.I))
                    UiText += "I";
                else if (input.KeyPressed(InputKey.J))
                    UiText += "J";
                else if (input.KeyPressed(InputKey.K))
                    UiText += "K";
                else if (input.KeyPressed(InputKey.L))
                    UiText += "L";
                else if (input.KeyPressed(InputKey.M))
                    UiText += "M";
                else if (input.KeyPressed(InputKey.N))
                    UiText += "N";
                else if (input.KeyPressed(InputKey.O))
                    UiText += "O";
                else if (input.KeyPressed(InputKey.P))
                    UiText += "P";
                else if (input.KeyPressed(InputKey.Q))
                    UiText += "Q";
                else if (input.KeyPressed(InputKey.R))
                    UiText += "R";
                else if (input.KeyPressed(InputKey.S))
                    UiText += "S";
                else if (input.KeyPressed(InputKey.T))
                    UiText += "T";
                else if (input.KeyPressed(InputKey.U))
                    UiText += "U";
                else if (input.KeyPressed(InputKey.V))
                    UiText += "V";
                else if (input.KeyPressed(InputKey.W))
                    UiText += "W";
                else if (input.KeyPressed(InputKey.X))
                    UiText += "X";
                else if (input.KeyPressed(InputKey.Y))
                    UiText += "Y";
                else if (input.KeyPressed(InputKey.Z))
                    UiText += "Z";
                else if (input.KeyPressed(InputKey.Key0) || input.KeyPressed(InputKey.KeyPad0))
                    UiText += "0";
                else if (input.KeyPressed(InputKey.Key1) || input.KeyPressed(InputKey.KeyPad1))
                    UiText += "1";
                else if (input.KeyPressed(InputKey.Key2) || input.KeyPressed(InputKey.KeyPad2))
                    UiText += "2";
                else if (input.KeyPressed(InputKey.Key3) || input.KeyPressed(InputKey.KeyPad3))
                    UiText += "3";
                else if (input.KeyPressed(InputKey.Key4) || input.KeyPressed(InputKey.KeyPad4))
                    UiText += "4";
                else if (input.KeyPressed(InputKey.Key5) || input.KeyPressed(InputKey.KeyPad5))
                    UiText += "5";
                else if (input.KeyPressed(InputKey.Key6) || input.KeyPressed(InputKey.KeyPad6))
                    UiText += "6";
                else if (input.KeyPressed(InputKey.Key7) || input.KeyPressed(InputKey.KeyPad7))
                    UiText += "7";
                else if (input.KeyPressed(InputKey.Key8) || input.KeyPressed(InputKey.KeyPad8))
                    UiText += "8";
                else if (input.KeyPressed(InputKey.Key9) || input.KeyPressed(InputKey.KeyPad9))
                    UiText += "9";
            }
        }

        public override void Update()
        {
            base.Update();
            if (m_editingText) EditText();
        }

        public void StartEditingText()
        {
            m_editingText = true;
            UiText = "";
        }

        public void FinishEditingText()
        { 
            m_editingText = false;
        }

    }
}
