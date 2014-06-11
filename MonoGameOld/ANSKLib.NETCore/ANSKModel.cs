using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ANSKLibrary
{
    public class ANSKModel
    {
        private static BlendState Blend3DNormal = BlendState.AlphaBlend;                     // Default 3D GraphicsDevice blend state.
        private static RasterizerState Rasterizer3DNormal = RasterizerState.CullNone;        // Default 3D GraphicsDevice rasterizer state.
        private static SamplerState Sampler3DNormal = SamplerState.LinearWrap;               // Default 3D GraphicsDevice sampler state.
        private static DepthStencilState DepthStencil3DNormal = DepthStencilState.Default;   // Default 3D GraphicsDevice depth stencil state.

        private SkinningData _skin;
        private ANSKTagData _tagData;
        private AnimationPlayer _player;
        private AnimationClip _currentClip;
        private List<Vector3> _verts;
        private List<short> _indicies;
        private List<Vector2> _uvs;
        private List<int> _uvIndicies;
        private List<int> _edges;
        private List<Vector3> _normals;
        private Skeleton _skeleton;
        private List<Joint> _joints;
        private Effect _effect;
        private ANSKVertexDeclaration[] _verticies;
        private VertexPositionColor[] _poop;
        private VertexBuffer _vertBuffer;
        private IndexBuffer _indBuffer;
        private GraphicsDevice _gDevice;
        private Game _game;

        public List<Vector3> Verticies { get { return _verts; } set { _verts = value; CreateDeclarationList(); } }

        public AnimationPlayer Player { get { return _player; } }
        public AnimationClip AnimationClip { get { return _currentClip; } set { _currentClip = value; } }

        //public ANSKTagData TagData { get { return _tagData; } }

        public ANSKModel(ANSKModelContent content)
        {
            _tagData = content.TagData;
            _verts = content.Verticies;
            RemakeIndices(content.VertexIndicies);
            _uvs = content.Uvs;
            _uvIndicies = content.UvIndicies;
            _edges = content.Edges;
            _normals = content.Normals;
            _skeleton = content.Joints;
            _joints = _skeleton.ToJointList();
            _skin = content.TagData.SkinData;

            _skeleton.Init();
            // Find a way to load in the effect;

            _verticies = new ANSKVertexDeclaration[_verts.Count];
            _poop = new VertexPositionColor[_verts.Count];
        }

        private void RemakeIndices(List<int> inds)
        {
            _indicies = new List<short>();
            for (int i = 0; i < inds.Count; i++)
            {
                _indicies.Add(Convert.ToInt16(inds[i]));
            }
        }

        public void PlayAnimation(string name)
        {
            _currentClip = _skin.AnimationClips[name];

            _player.StartClip(_currentClip);
        }

        public void ManualInitialise(GraphicsDevice device, Effect effect, Game game)
        {
            _effect = effect;
            _gDevice = device;
            _game = game;
            DebugShapeRenderer.Initialize(_game.GraphicsDevice);

            try
            {
                //_vertBuffer = new VertexBuffer(_gDevice, typeof(ANSKVertexDeclaration), _verts.Count, BufferUsage.None);
                //_indBuffer = new IndexBuffer(_gDevice, IndexElementSize.SixteenBits, sizeof(int) * _indicies.Count, BufferUsage.None);
                _vertBuffer = new VertexBuffer(_game.GraphicsDevice, typeof(ANSKVertexDeclaration), _verts.Count, BufferUsage.WriteOnly);
                //_vertBuffer = new VertexBuffer(_game.GraphicsDevice, typeof(VertexPositionColor), _verts.Count, BufferUsage.WriteOnly);
                _indBuffer = new IndexBuffer(_game.GraphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * _indicies.Count, BufferUsage.WriteOnly);
            }
            catch (Exception e)
            {
                int poo = 0;
            }

            _player = new AnimationPlayer(_tagData.SkinData);

            _indBuffer.SetData<short>(_indicies.ToArray());
            //_gDevice.Indices = _indBuffer;
            _game.GraphicsDevice.Indices = _indBuffer;
            CreateDeclarationList();
        }

        public void CreateDeclarationList()
        {
            for (int i = 0; i < _verts.Count; i++)
            {
                // TODO -- This is commented as we do not have the logic that generates the
                // required indices and weights per vertex.

                int4 ints = VertexToJointIndices(i);

                float4 weights = VertexToJointsWeights(i, ints);

                _verticies[i] = new ANSKVertexDeclaration(_verts[i], _uvs[i], _normals[i], ints, weights, ints.Count);
                //_verticies[i] = new ANSKVertexDeclaration(_verts[i], Color.Red);
                _poop[i] = new VertexPositionColor(_verts[i], Color.Red);
            }

            _vertBuffer.SetData<ANSKVertexDeclaration>(_verticies.ToArray<ANSKVertexDeclaration>());
            //_vertBuffer.SetData<VertexPositionColor>(_poop.ToArray<VertexPositionColor>());
            _game.GraphicsDevice.SetVertexBuffer(_vertBuffer);
        }

        private int4 VertexToJointIndices(int vertIndex)
        {
            int4 ints = new int4();
            ints.Init();

            for (int i = 0; i < _joints.Count; i++)
            {
                if (_joints[i].IsIndicePartOfThisJoint(vertIndex))
                    ints.AddInt(i);
            }

            return ints;
        }

        private float4 VertexToJointsWeights(int vertIndex, int4 ints)
        {
            float4 floats = new float4();
            floats.Init();

            for (int i = 0; i < ints.Count; i++)
            {
                floats.AddFloat(_joints[ints[i]].GetWeight(vertIndex));
            }

            return floats;
        }

        private void SetGraphicsStatesFor3D()
        {
            //_gDevice.RasterizerState = Rasterizer3DNormal;
            //_gDevice.BlendState = Blend3DNormal;
            //_gDevice.DepthStencilState = DepthStencil3DNormal;
            //_gDevice.SamplerStates[0] = Sampler3DNormal;
            _game.GraphicsDevice.RasterizerState = Rasterizer3DNormal;
            _game.GraphicsDevice.BlendState = Blend3DNormal;
            _game.GraphicsDevice.DepthStencilState = DepthStencil3DNormal;
            _game.GraphicsDevice.SamplerStates[0] = Sampler3DNormal;
        }

        public void Update(GameTime gameTime, Matrix transform)
        {
            _player.Update(gameTime.ElapsedGameTime, true, transform);
        }

        public void Draw(GameTime gameTime, Matrix transform, Matrix view, Matrix proj)
        {
            Matrix[] bones = _player.GetSkinTransforms();
            Matrix[] transforms = new Matrix[_skin.SkeletonHierarchy.Count];

            transforms = _skeleton.CollectAbsoluteBoneTransforms();

            SetGraphicsStatesFor3D();

            Matrix worldProj = transform * view * proj;

            _effect.CurrentTechnique = _effect.Techniques["AnimatableModelTechnique"];
            _effect.Parameters["worldProj"].SetValue(worldProj);
            _effect.Parameters["bones"].SetValue(bones);
            _effect.Parameters["vsArrayIndex"].SetValue(0);
            _effect.Parameters["psArrayIndex"].SetValue(0);

            List<Vector3> temp = new List<Vector3>();
            for (int i = 0; i < _verticies.Length; i++)
            {
                temp.Add(Vector3.Transform(_verticies[i].Position, bones[_verticies[i].Indices[0]]));// * bones[_verticies[i].Indices[0]].Translation * _verticies[i].Weights[0]);
                //temp[i] = Vector3.Transform(temp[i], transforms[0]);
                DebugShapeRenderer.AddBoundingSphere(new BoundingSphere(temp[i], 1), Color.White);
                //DebugShapeRenderer.AddBoundingSphere(new BoundingSphere(_verticies[i].Position, 1), Color.White);
            }

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                //_vertBuffer.SetData<ANSKVertexDeclaration>(_verticies.ToArray<ANSKVertexDeclaration>());
                //_gDevice.SetVertexBuffer(_vertBuffer);
                _game.GraphicsDevice.SetVertexBuffer(_vertBuffer);
                _game.GraphicsDevice.Indices = _indBuffer;

                pass.Apply();

                //_gDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _verts.Count);
                //_gDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indicies.Count, 0, _indicies.Count / 3);
                _game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _verts.Count, 0, _indicies.Count);
                //_game.GraphicsDevice.DrawUserIndexedPrimitives<ANSKVertexDeclaration>(PrimitiveType.TriangleList, _verticies, 0, _verticies.Length, _indicies.ToArray(), 0, _verticies.Length);
                //_game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _poop, 0, _poop.Length, _indicies.ToArray(), 0, _poop.Length);
                DebugShapeRenderer.Draw(gameTime, view, proj);
            }
            // We use the graphics device Draw Indexed Primitives or something similar.
        }
    }
}