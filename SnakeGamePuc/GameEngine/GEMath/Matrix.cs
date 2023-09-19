using GameEngine.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GEMath
{
    public class Matrix : ICloneable
    {

        private float[] m_data;
        public int NumOfRows { get; private set; }
        public int NumOfColumns { get; private set; }
        public int Size { get; private set; }

        //public float[] MatrixData { get { return m_data; } }

        public Matrix(int _rows, int _columns)
        {
            NumOfColumns = _columns; 
            NumOfRows = _rows; 
            Size = _rows * _columns; 
            m_data = new float[Size];
        }

        /// <summary>
        /// Cria uma matrix unidimensional com apenas 1 linha
        /// </summary>
        public Matrix(float[] _data)
        {
            NumOfColumns = _data.Length;
            NumOfRows = 1;
            Size = _data.Length;
            m_data = _data;
        }

        public float GetElement(int _column, int _row)
        {
            if (_row >= NumOfRows || _column >= NumOfColumns) throw new MathException();
            return m_data[(_row * NumOfColumns) + _column];
        }

        public float GetElement(int _elementIndex)
        {
            if (_elementIndex >= Size) throw new MathException();
            return m_data[_elementIndex];
        }

        public void SetElement(int _column, int _row, float _newValue)
        {
            if (_row >= NumOfRows || _column >= NumOfColumns) throw new MathException();
            m_data[(_row * NumOfColumns) + _column] = _newValue;
        }

        public void SetElement(int _elementIndex, float _newValue)
        {
            if (_elementIndex >= Size) throw new MathException();
            m_data[_elementIndex] = _newValue;
        }

        public object Clone()
        {
            Matrix m = (Matrix)MemberwiseClone();
            m.m_data = (float[])m_data.Clone();
            return m;
        }

        public static Matrix operator *(Matrix _m1, Matrix _m2)
        {
            if (_m1.NumOfColumns != _m2.NumOfRows) throw new MathException();

            Matrix newMatrix = new Matrix(_m1.NumOfRows, _m2.NumOfColumns);
            float newValue;

            for (int i = 0; i < newMatrix.Size; i++)
            {
                newValue = 0;
                for (int j = 0; j < _m1.NumOfColumns; j++)
                {
                    newValue += _m1.GetElement(j, i / _m1.NumOfRows) * _m2.GetElement(i % _m2.NumOfColumns, j);
                }
                newMatrix.SetElement(i, newValue);
            }

            return newMatrix;
        }

        public static Matrix operator -(Matrix _m1, Matrix _m2)
        {
            if (_m1.Size != _m2.Size) throw new MathException();

            Matrix newMatrix = new Matrix(_m1.NumOfRows, _m1.NumOfColumns);

            for (int i = 0; i < newMatrix.Size; i++)
            {
                newMatrix.SetElement(i, _m1.GetElement(i) - _m2.GetElement(i));
            }

            return newMatrix;
        }

        public static Matrix operator +(Matrix _m1, Matrix _m2)
        {
            if (_m1.Size != _m2.Size) throw new MathException();

            Matrix newMatrix = new Matrix(_m1.NumOfRows, _m1.NumOfColumns);

            for (int i = 0; i < newMatrix.Size; i++)
            {
                newMatrix.SetElement(i, _m1.GetElement(i) + _m2.GetElement(i));
            }

            return newMatrix;
        }
    }
}
