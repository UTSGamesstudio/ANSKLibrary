using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ANSKLibrary
{
    public class ANSKModelContent
    {
        [ContentSerializer]
        public List<Vector3> Verticies { get; set; }
        [ContentSerializer]
        public List<int> VertexIndicies { get; set; }
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
        public ANSKTagData TagData { get; set; }

        public ANSKModelContent(List<Vector3> verts, List<int> vertInd, List<Vector2> uv, List<int> uvInd, List<int> edges, List<Vector3> normals, Skeleton joints, ANSKTagData tagData)
        {
            Verticies = verts;
            VertexIndicies = vertInd;
            Uvs = uv;
            UvIndicies = uvInd;
            Edges = edges;
            Normals = normals;
            Joints = joints;
            TagData = tagData;
        }

        private ANSKModelContent()
        {

        }
    }
}
