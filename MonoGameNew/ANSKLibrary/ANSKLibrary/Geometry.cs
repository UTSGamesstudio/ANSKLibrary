using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ANSKLibrary
{
    public class Geometry
    {
        public List<Vector3> _vertices;
        public List<short> _vertIndices;
        public List<Vector2> _uvs;
        public List<int> _uvIndices;
        public List<int> _edges;
        public List<Vector3> _normals;

        public List<Vector3> _verticies { get { return _vertices; } set { _vertices = value; } }
        public List<short> VertexIndicies { get { return _vertIndices; } set { _vertIndices = value; } }
        public List<Vector2> Uvs { get { return _uvs; } set { _uvs = value; } }
        public List<int> UvIndicies { get { return _uvIndices; } set { _uvIndices = value; } }
        public List<int> Edges { get { return _edges; } set { _edges = value; } }
        public List<Vector3> Normals { get { return _normals; } set { _normals = value; } }

        public Geometry()
        {
            _vertices = new List<Vector3>();
            _vertIndices = new List<short>();
            _uvs = new List<Vector2>();
            _uvIndices = new List<int>();
            _edges = new List<int>();
            _normals = new List<Vector3>();
        }

        public Geometry(MeshContent content)
        {
            _vertices = content.Verticies;
            _vertIndices = content.VertexIndicies;
            _uvs = content.Uvs;
            _uvIndices = content.UvIndicies;
            _edges = content.Edges;
            _normals = content.Normals;
        }
    }
}
