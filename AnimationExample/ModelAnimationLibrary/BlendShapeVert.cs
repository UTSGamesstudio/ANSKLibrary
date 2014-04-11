using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ModelAnimationLibrary
{
    public class BlendShapeVert
    {
        private int _indice;
        private Vector3 _vert;
        private Vector3 _blendTo;
        private int _prevVal;
        
        public int Indice { get { return _indice; } }
        public Vector3 Vertex { get { return _vert; } }
        public Vector3 BlendTo { get { return _blendTo; } set { _blendTo = value; } }

        public BlendShapeVert(int indice, Vector3 vert)
        {
            _indice = indice;
            _vert = vert;
            _blendTo = Vector3.Zero;
            _prevVal = 0;
        }

        public void Blend(int val)
        {
            float temp = val - _prevVal;

            if (temp == 0)
                _blendTo = Vector3.Zero;
            else
                _blendTo = new Vector3(_vert.X / temp, _vert.Y / temp, _vert.Z / temp);

            _prevVal = val;
        }

        public void ResetBlend()
        {
            _blendTo = Vector3.Zero;

            Blend(0);
        }
    }
}
