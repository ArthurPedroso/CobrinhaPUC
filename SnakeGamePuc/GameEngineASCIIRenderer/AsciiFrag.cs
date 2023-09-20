using GameEngine.Components;
using GameEngine.Components.Sprites;
using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineASCIIRenderer
{
    internal struct AsciiFrag
    {
        public char Value;
        public Matrix3x3 ModelMatrix;

        public AsciiFrag(char _value, Matrix3x3 _modelMatrix)
        {
            Value = _value;
            ModelMatrix = (Matrix3x3)_modelMatrix.Clone();
        }
        public AsciiFrag(ASCIISprite _asciiSprite)
        {
            Transform tr = _asciiSprite.AttachedGameObject.GetComponent<Transform>();
            if (tr != null)
                ModelMatrix = tr.ModelMatClone;
            else
                ModelMatrix = new Matrix3x3();

            Value = _asciiSprite.Icon;
        }
    }
}
