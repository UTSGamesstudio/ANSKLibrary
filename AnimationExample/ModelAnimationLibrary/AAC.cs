using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModelAnimationLibrary
{
    // Stands for "Advanced Animation Control"
    public class AAC
    {
        private BlendShapeManager _bManager;

        public BlendShapeManager BlendShapesControl { get { return _bManager; } }

        public AAC()
        {
            _bManager = null;
        }

        public void LoadBlendShapes(List<BlendShapeContent> shapes, ANSKModel model)
        {
            if (shapes != null || shapes.Count != 0)
                _bManager = new BlendShapeManager(shapes, model);
        }

        public void Update(GameTime g)
        {
            if (_bManager != null)
                _bManager.Update(g);
        }
    }
}
