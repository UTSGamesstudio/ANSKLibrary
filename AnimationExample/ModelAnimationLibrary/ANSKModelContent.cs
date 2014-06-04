using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ModelAnimationLibrary
{
    public class ANSKModelContent
    {
        [ContentSerializer]
        public MeshContent Mesh { get; set; }
        [ContentSerializer]
        public Skeleton Joints { get; set; }
        [ContentSerializer]
        public SkinningData Skin { get; set; }
        [ContentSerializer]
        public List<BlendShapeContent> BlendShapes { get; set; }

        public ANSKModelContent(List<Vector3> verts, List<short> vertInd, List<Vector2> uv, List<int> uvInd, List<int> edges, List<Vector3> normals, Skeleton joints, SkinningData skin, List<BlendShapeContent> bShapes, MaterialContent materials)
        {
            Mesh = new MeshContent(verts, vertInd, uv, uvInd, edges, normals, materials);
            Joints = joints;
            Skin = skin;
            BlendShapes = bShapes;
        }

        private ANSKModelContent()
        {

        }
    }
}
