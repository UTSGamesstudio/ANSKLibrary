using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModelAnimationLibrary
{
    public class MeshPart
    {
        private List<ANSKVertexDeclaration> _vertDecs;
        private Material _material;
        private List<short> _indices;
        private Effect _effect;

        public MeshPart(Material material)
        {
            _vertDecs = new List<ANSKVertexDeclaration>();
        }

        public void AddVertex(Vector3 vertex, Vector2 uv, Vector3 normal, int4 indices, float4 weights, int boneCount, short indice)
        {
            _vertDecs.Add(new ANSKVertexDeclaration(vertex, _material.DiffuseColour.ToVector4(), uv, normal, indices, weights, boneCount));
            _indices.Add(indice);
        }

        public void Reset()
        {
            _vertDecs.Clear();
            _indices.Clear();
        }

        public List<short> CollectIndices()
        {
            return _indices;
        }

        public List<ANSKVertexDeclaration> CollectVertices()
        {
            return _vertDecs;
        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
