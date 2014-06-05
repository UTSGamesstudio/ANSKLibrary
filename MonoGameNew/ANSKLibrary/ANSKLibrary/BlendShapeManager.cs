using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ANSKLibrary
{
    public class BlendShapeManager
    {
        private List<BlendShapeComplex> _bShapes;

        public BlendShapeManager(List<BlendShapeContent> shapes, ANSKModel model)
        {
            _bShapes = new List<BlendShapeComplex>();
             for (int i = 0; i < shapes.Count; i++)
                 _bShapes.Add(new BlendShapeComplex(new BlendShape(shapes[i]), model));
        }

        public void ChangeBlendValue(string name, int value)
        {
            if (value < 0 || value > 1)
                return;

            for (int i = 0; i < _bShapes.Count; i++)
            {
                if (_bShapes[i].Shape.Name.Contains(name))
                    _bShapes[i].Blend(value);
            }
        }

        public void Update(GameTime g)
        {

        }
    }
}
