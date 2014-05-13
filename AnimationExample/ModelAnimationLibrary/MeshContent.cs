using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ModelAnimationLibrary
{
    public class MeshContent
    {
        [ContentSerializer]
        public List<Vector3> Verticies { get; set; }
        [ContentSerializer]
        public List<short> VertexIndicies { get; set; }
        [ContentSerializer]
        public List<Vector2> Uvs { get; set; }
        [ContentSerializer]
        public List<int> UvIndicies { get; set; }
        [ContentSerializer]
        public List<int> Edges { get; set; }
        [ContentSerializer]
        public List<Vector3> Normals { get; set; }
        [ContentSerializer]
        public MaterialContent Materials { get; set; }

        public MeshContent(List<Vector3> verts, List<short> vertInd, List<Vector2> uvs, List<int> uvInd, List<int> edges, List<Vector3> normals, MaterialContent materialContent)
        {
            Verticies = verts;
            VertexIndicies = vertInd;
            Uvs = uvs;
            UvIndicies = uvInd;
            Edges = edges;
            Normals = normals;
            Materials = materialContent;
        }

        public MeshContent() { }
    }
}
