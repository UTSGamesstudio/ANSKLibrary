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
        public List<Vector3> Verticies { get; set; }
        [ContentSerializer]
        public List<int> VertexIndicies {  get; set; } 
        [ContentSerializer]
        public List<Vector2> Uvs { get; set; } 
        [ContentSerializer]
        public List<int> UvIndicies { get; set; }
        [ContentSerializer]
        public List<int> Edges { get; set; }
        [ContentSerializer]
        public List<Vector3> Normals { get; set; }
        [ContentSerializer]
        public Skeleton Joints { get; set; }
        [ContentSerializer]
        public SkinningData Skin { get; set; }
        [ContentSerializer]
        public List<BlendShapeContent> BlendShapes { get; set; }
        [ContentSerializer]
        public MaterialContent Materials { get; set; }

        public ANSKModelContent(List<Vector3> verts, List<int> vertInd, List<Vector2> uv, List<int> uvInd, List<int> edges, List<Vector3> normals, Skeleton joints, SkinningData skin, List<BlendShapeContent> bShapes, MaterialContent materials)
        {
            Verticies = verts;
            VertexIndicies = vertInd;
            Uvs = uv;
            UvIndicies = uvInd;
            Edges = edges;
            Normals = normals;
            Joints = joints;
            Skin = skin;
            BlendShapes = bShapes;
            Materials = materials;
        }

        private ANSKModelContent()
        {

        }
    }
}
