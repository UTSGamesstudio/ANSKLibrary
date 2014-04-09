using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ModelAnimationLibrary
{
    public class ANSKTagData
    {
        [ContentSerializer]
        private SkinningData _skin;
        [ContentSerializer]
        private List<BlendShapeContent> _bShapes;
        [ContentSerializer]
        private ANSKModelContent _modelContent;

        public SkinningData SkinData { get { return _skin; } set { _skin = value; } }
        public List<BlendShapeContent> BlendShapes { get { return _bShapes; } set { _bShapes = value; } }
        public ANSKModelContent ModelContent { get { return _modelContent; } set { _modelContent = value; } }

        public ANSKTagData(SkinningData skin)
        {
            _skin = skin;
            _bShapes = new List<BlendShapeContent>();
        }

        public ANSKTagData() { }
    }
}
