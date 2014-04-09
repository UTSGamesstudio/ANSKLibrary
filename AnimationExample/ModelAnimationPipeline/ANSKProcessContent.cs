using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace ModelAnimationPipeline
{
    public class ANSKProcessContent
    {
        NodeContent _node;
        ANSKFbxData _ansk;

        public NodeContent NodeContent { get { return _node; } }
        public ANSKFbxData ANSKData { get { return _ansk; } }

        public ANSKProcessContent(NodeContent node, ANSKFbxData ansk)
        {
            _node = node;
            _ansk = ansk;
        }
    }
}
