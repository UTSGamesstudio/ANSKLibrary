using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelAnimationLibrary
{
    public struct float4
    {
        private float _num1, _num2, _num3, _num4;

        /*public float4()
        {
            _num1 = -1;
            _num2 = -1;
            _num3 = -1;
            _num4 = -1;
        }*/

        public void Init()
        {
            _num1 = -1;
            _num2 = -1;
            _num3 = -1;
            _num4 = -1;
        }

        public float4(float num1, float num2, float num3, float num4)
        {
            _num1 = num1;
            _num2 = num2;
            _num3 = num3;
            _num4 = num4;
        }

        public void AddFloat(float num)
        {
            if (_num1 == -1)
                _num1 = num;
            else if (_num2 == -1)
                _num2 = num;
            else if (_num3 == -1)
                _num3 = num;
            else if (_num4 == -1)
                _num4 = num;
        }

        public float[] ToFloatArray()
        {
            return new float[4] { _num1, _num2, _num3, _num4 };
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 1:
                        return _num1;
                    case 2:
                        return _num2;
                    case 3:
                        return _num3;
                    case 4:
                        return _num4;
                    default:
                        return -1;
                }
            }
        }
    };
}
