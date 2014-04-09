using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelAnimationLibrary
{
    public struct int4
    {
        private int _num1, _num2, _num3, _num4;

        public int Count
        {
            get
            {
                int count = 0;
                if (_num1 != -1 || _num1 != null) count++;
                else if (_num2 != -1 || _num2 != null) count++;
                else if (_num3 != -1 || _num3 != null) count++;
                else if (_num4 != -1 || _num4 != null) count++;

                return count;
            }
        }

        /*public int4()
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

        public int4(int num1, int num2, int num3, int num4)
        {
            _num1 = num1;
            _num2 = num2;
            _num3 = num3;
            _num4 = num4;
        }

        public void AddInt(int num)
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

        public int[] ToIntArray()
        {
            return new int[4] { _num1, _num2, _num3, _num4 };
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _num1;
                    case 1:
                        return _num2;
                    case 2:
                        return _num3;
                    case 3:
                        return _num4;
                    default:
                        return -1;
                }
            }
        }
    };
}
