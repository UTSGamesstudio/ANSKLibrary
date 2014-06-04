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

        private List<ANSKVertexDeclarationAnimatable> _vertDecsAnim;
        private List<ANSKVertexDeclarationModel> _vertDecsModel;
        private Material _material;
        private List<short> _indices;
        private Effect _effect;
        private VertexBuffer _vertBuffer;
        private IndexBuffer _indBuffer;
        private GraphicsDevice _graphics;
        private bool _usable;
        private Texture2D _texture;
        public MeshRenderer.ModelRenderType _renderType;

        public Effect MeshEffect { get { return _effect; } set { _effect = value; } }
        public Material MeshMaterial { get { return _material; } }
        public GraphicsDevice MeshGraphicsDevice { get { return _graphics; } set { _graphics = value; } }
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }

        public MeshPart(Material material)
        {
            _vertDecsAnim = new List<ANSKVertexDeclarationAnimatable>();
            _vertDecsModel = new List<ANSKVertexDeclarationModel>();
            _indices = new List<short>();
            _material = material;
            _usable = false;
            _texture = null;
        }

        public void AddVertex(Vector3 vertex, Vector2 uv, Vector3 normal, int4 indices, float4 weights, int boneCount, short indice)
        {
            _usable = true;

            _vertDecsAnim.Add(new ANSKVertexDeclarationAnimatable(vertex, _material.DiffuseColour.ToVector4(), uv, normal, indices, weights, boneCount));
            _indices.Add(indice);

            _renderType = MeshRenderer.ModelRenderType.Animatable;
        }

        public void AddVertex(Vector3 vertex, Vector2 uv, Vector3 normal, short indice)
        {
            _usable = true;

            _vertDecsModel.Add(new ANSKVertexDeclarationModel(vertex, _material.DiffuseColour.ToVector4(), uv, normal));
            
            _indices.Add(indice);

            _renderType = MeshRenderer.ModelRenderType.Normal;
        }

        public void Reset()
        {
            _usable = false;
            _vertDecsAnim.Clear();
            _indices.Clear();
        }

        public void Finalise()
        {
            if (_usable)
            {
                switch (_renderType)
                {
                    case MeshRenderer.ModelRenderType.Normal:
                        _vertBuffer = new VertexBuffer(_graphics, typeof(ANSKVertexDeclarationModel), _vertDecsModel.Count, BufferUsage.None);
                        _vertBuffer.SetData<ANSKVertexDeclarationModel>(_vertDecsModel.ToArray());
                        break;

                    case MeshRenderer.ModelRenderType.Animatable:
                        _vertBuffer = new VertexBuffer(_graphics, typeof(ANSKVertexDeclarationAnimatable), _vertDecsAnim.Count, BufferUsage.None);
                        _vertBuffer.SetData<ANSKVertexDeclarationAnimatable>(_vertDecsAnim.ToArray());
                        break;
                }

                
                _indBuffer = new IndexBuffer(_graphics, typeof(short), _indices.Count, BufferUsage.None);
                _indBuffer.SetData<short>(_indices.ToArray());
            }
        }

        public List<short> CollectIndices()
        {
            return _indices;
        }

        public List<ANSKVertexDeclarationAnimatable> CollectAnimVertices()
        {
            return _vertDecsAnim;
        }

        public List<ANSKVertexDeclarationModel> CollectModelVertices()
        {
            return _vertDecsModel;
        }

        public void Draw(GameTime gameTime, Matrix[] bones)
        {
            ANSKVertexDeclarationAnimatable[] skinnedAnim = _vertDecsAnim.ToArray();

            if (!_usable) return;

            if (_material.Name.Contains(Material.NameLambert))
                _effect.Parameters["psArrayIndex"].SetValue(PIXELSHADERLAMBERT);
            else if (_material.Name.Contains(Material.NameBlinn))
            {
                _effect.Parameters["specColour"].SetValue(((BlinnMaterial)_material).SpecularColour.ToVector4());
                _effect.Parameters["specPos"].SetValue(((BlinnMaterial)_material).Specular);
                _effect.Parameters["psArrayIndex"].SetValue(PIXELSHADERBLINN);
            }

            if (_texture != null)
            {
                _effect.Parameters["usingTexture"].SetValue(true);
                _effect.Parameters["modelTexture"].SetValue(_texture);
            }
            else
                _effect.Parameters["usingTexture"].SetValue(false);

            _effect.Parameters["vsArrayIndex"].SetValue(0);

            if (_renderType == MeshRenderer.ModelRenderType.Animatable)
                _effect.CurrentTechnique = _effect.Techniques["Anim"];
                //_effect.CurrentTechnique = _effect.Techniques["Normal"];
            else
                _effect.CurrentTechnique = _effect.Techniques["Normal"];

            /*if (_renderType == MeshRenderer.ModelRenderType.Animatable)
            {
                for (int i = 0; i < _vertDecsAnim.Count; i++)
                {
                    skinnedAnim[i].Position = Vector3.Transform(_vertDecsAnim[i].Position, bones[_vertDecsAnim[i].Indices[0]]);
                }

                _vertBuffer.SetData<ANSKVertexDeclarationAnimatable>(skinnedAnim);
            }*/
                

            _graphics.SetVertexBuffer(_vertBuffer);
            _graphics.Indices = _indBuffer;

            /*for (int i = 0; i < _vertDecsAnim.Count; i++)
            {
                DebugShapeRenderer.AddBoundingSphere(new BoundingSphere(Vector3.Transform(_vertDecsAnim[i].Position, bones[_vertDecsAnim[i].Indices[0]]), 0.1f), Color.White);
            }
            for (int i = 0; i < _vertDecsAnim.Count; i+=3)
            {
                DebugShapeRenderer.AddTriangle(Vector3.Transform(_vertDecsAnim[i].Position, bones[_vertDecsAnim[i].Indices[0]]),
                                                Vector3.Transform(_vertDecsAnim[i + 1].Position, bones[_vertDecsAnim[i + 1].Indices[0]]),
                                                Vector3.Transform(_vertDecsAnim[i + 2].Position, bones[_vertDecsAnim[i + 2].Indices[0]]),
                                                Color.White);
            }*/

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, _indices.Count);
            }
        }
    }
}
