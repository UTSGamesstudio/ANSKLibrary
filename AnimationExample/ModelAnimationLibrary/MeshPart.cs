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
        public const int PIXELSHADERLAMBERT = 0;
        public const int PIXELSHADERBLINN = 1;

        private List<ANSKVertexDeclaration> _vertDecs;
        private Material _material;
        private List<short> _indices;
        private Effect _effect;
        private VertexBuffer _vertBuffer;
        private IndexBuffer _indBuffer;
        private GraphicsDevice _graphics;
        private bool _usable;

        public Effect MeshEffect { get { return _effect; } set { _effect = value; } }
        public Material MeshMaterial { get { return _material; } }
        public GraphicsDevice MeshGraphicsDevice { get { return _graphics; } set { _graphics = value; } }

        public MeshPart(Material material)
        {
            _vertDecs = new List<ANSKVertexDeclaration>();
            _indices = new List<short>();
            _material = material;
            _usable = false;
        }

        public void AddVertex(Vector3 vertex, Vector2 uv, Vector3 normal, int4 indices, float4 weights, int boneCount, short indice)
        {
            _usable = true;
            try
            {
                _vertDecs.Add(new ANSKVertexDeclaration(vertex, _material.DiffuseColour.ToVector4(), uv, normal, indices, weights, boneCount));
            }
            catch (Exception e)
            {

            }
            _indices.Add(indice);
        }

        public void Reset()
        {
            _usable = false;
            _vertDecs.Clear();
            _indices.Clear();
        }

        public void Finalise()
        {
            if (_usable)
            {
                _vertBuffer = new VertexBuffer(_graphics, typeof(ANSKVertexDeclaration), _vertDecs.Count, BufferUsage.None);
                _indBuffer = new IndexBuffer(_graphics, typeof(short), _indices.Count, BufferUsage.None);
                _vertBuffer.SetData<ANSKVertexDeclaration>(_vertDecs.ToArray());
                _indBuffer.SetData<short>(_indices.ToArray());
            }
        }

        public List<short> CollectIndices()
        {
            return _indices;
        }

        public List<ANSKVertexDeclaration> CollectVertices()
        {
            return _vertDecs;
        }

        public void Draw(GameTime gameTime, Matrix[] bones)
        {
            if (!_usable) return;

            if (_material.Name.Contains(Material.NameLambert))
                _effect.Parameters["psArrayIndex"].SetValue(PIXELSHADERLAMBERT);
            else if (_material.Name.Contains(Material.NameBlinn))
            {
                _effect.Parameters["specColour"].SetValue(((BlinnMaterial)_material).SpecularColour.ToVector4());
                _effect.Parameters["specPos"].SetValue(((BlinnMaterial)_material).Specular);
                _effect.Parameters["psArrayIndex"].SetValue(PIXELSHADERBLINN);
            }

            _effect.Parameters["vsArrayIndex"].SetValue(0);

            _graphics.SetVertexBuffer(_vertBuffer);
            _graphics.Indices = _indBuffer;

            for (int i = 0; i < _vertDecs.Count; i++)
            {
                DebugShapeRenderer.AddBoundingSphere(new BoundingSphere(Vector3.Transform(_vertDecs[i].Position, bones[_vertDecs[i].Indices[0]]), 0.1f), Color.White);
            }
            for (int i = 0; i < _vertDecs.Count; i+=3)
            {
                DebugShapeRenderer.AddTriangle(Vector3.Transform(_vertDecs[i].Position, bones[_vertDecs[i].Indices[0]]),
                                                Vector3.Transform(_vertDecs[i + 1].Position, bones[_vertDecs[i + 1].Indices[0]]),
                                                Vector3.Transform(_vertDecs[i + 2].Position, bones[_vertDecs[i + 2].Indices[0]]),
                                                Color.White);
            }

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //_graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertDecs.Count, 0, _indices.Count);
                _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, _indices.Count);
            }
        }
    }
}
