using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ModelAnimationLibrary
{
    public class ANSKModel
    {
        private static BlendState Blend3DNormal = BlendState.AlphaBlend;                     // Default 3D GraphicsDevice blend state.
        private static RasterizerState Rasterizer3DNormal = RasterizerState.CullNone;        // Default 3D GraphicsDevice rasterizer state.
        private static SamplerState Sampler3DNormal = SamplerState.LinearWrap;               // Default 3D GraphicsDevice sampler state.
        private static DepthStencilState DepthStencil3DNormal = DepthStencilState.Default;   // Default 3D GraphicsDevice depth stencil state.

        private AAC _aac;
        private SkinningData _skin;
        private AnimationPlayer _player;
        private AnimationClip _currentClip;

        // Only keep these here to show the model for examples.
        // Will be replaced with MeshManager.
        /*private List<Vector3> _verts;
        private List<short> _indicies;
        private List<Vector2> _uvs;
        private List<int> _uvIndicies;
        private List<int> _edges;
        private List<Vector3> _normals;
        private List<Material> _materials;
        private List<int> _materialIndices;*/

        private Skeleton _skeleton;
        private List<Joint> _joints;
        private Effect _effect;
        private ANSKVertexDeclaration[] _verticies;
        private VertexBuffer _vertBuffer;
        private IndexBuffer _indBuffer;
        private GraphicsDevice _gDevice;
        private Game _game;

        private BasicEffect _testEffect;

        // This class is unfinished, so code has been rolled back to the original render code so this example can actually work.
        // Code marked with 'RollForward' is new code that been commented out so the old code could be used.
        private MeshManager _mesh;

        // RollForward
        public List<Vector3> Verticies { get { return _mesh.Vertices; } set { _mesh.Vertices = value; CreateDeclarationList(); } }
        public List<short> Indices { get { return _mesh.Indices; } }
        //public List<Vector3> Verticies { get { return _verts; } set { _verts = value; CreateDeclarationList(); } }
        //public List<short> Indices { get { return _indicies; } }

        public AnimationPlayer Player { get { return _player; } }
        public AnimationClip AnimationClip { get { return _currentClip; } set { _currentClip = value; } }
        public AAC AAC { get { return _aac; } }
        public MeshManager MeshManager { get { return _mesh; } set { _mesh = value; } }

        public ANSKModel(ANSKModelContent content)
        {            
            /*_verts = content.Mesh.Verticies;
            _indicies = content.Mesh.VertexIndicies;
            _uvs = content.Mesh.Uvs;
            _uvIndicies = content.Mesh.UvIndicies;
            _edges = content.Mesh.Edges;
            _normals = content.Mesh.Normals;
            _materials = content.Mesh.Materials.Materials;
            _materialIndices = content.Mesh.Materials.MaterialIndicieList;*/
             
            _skeleton = content.Joints;
            _joints = _skeleton.ToJointList();
            _skin = content.Skin;

            // RollForward
            _mesh = new MeshManager(content.Mesh, _joints);

            _skeleton.Init();

            //_verticies = new ANSKVertexDeclaration[_verts.Count];

            _aac = new AAC();
            _aac.LoadBlendShapes(content.BlendShapes, this);
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
            _mesh.MeshEffect = effect;
            _mesh.MeshGraphicDevice = device;
            _mesh.Refresh();
            _testEffect = new BasicEffect(device);
            /*try
            {
                // RollForward
                _vertBuffer = new VertexBuffer(_game.GraphicsDevice, typeof(ANSKVertexDeclaration), _mesh.Vertices.Count, BufferUsage.WriteOnly);
                //_vertBuffer = new VertexBuffer(_game.GraphicsDevice, typeof(ANSKVertexDeclaration), _verts.Count, BufferUsage.WriteOnly);
                _indBuffer = new IndexBuffer(_game.GraphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * _mesh.IndiceArray.Length, BufferUsage.WriteOnly);
                //_indBuffer = new IndexBuffer(_game.GraphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * _indicies.Count, BufferUsage.WriteOnly);
            }
            catch (Exception e)
            {
                throw new Exception("Error declaring vertex and index buffers.", e);
            }
            */
            _player = new AnimationPlayer(_skin);

            // RollForward
            //_indBuffer.SetData<short>(_mesh.IndiceArray);
            //_indBuffer.SetData<short>(_indicies.ToArray());
            //_gDevice.Indices = _indBuffer;
            //_game.GraphicsDevice.Indices = _indBuffer;
            //CreateDeclarationList();
        }

        public void CreateDeclarationList()
        {
            // RollForward 
            // Delete this for loop
            /*for (int i = 0; i < _verts.Count; i++)
            {
                int4 ints = VertexToJointIndices(i);
                float4 weights = VertexToJointsWeights(i, ints);

                _verticies[i] = new ANSKVertexDeclaration(_verts[i], _materials[_materialIndices[i]].DiffuseColour.ToVector4(), _uvs[i], _normals[i], ints, weights, ints.Count);
            }*/

            // RollForward
            _mesh.Refresh();

            //_game.GraphicsDevice.SetVertexBuffer(null);

            // RollForward
            //_vertBuffer.SetData<ANSKVertexDeclaration>(_mesh.VertexDeclarationArray);
            //_vertBuffer.SetData<ANSKVertexDeclaration>(_verticies);

            //_game.GraphicsDevice.SetVertexBuffer(_vertBuffer);
        }

        // RollForward
        // Delete everything in this region.
        #region OnlyInUseForExample
        /*private int4 VertexToJointIndices(int vertIndex)
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

        public void CenterModelToOrigin()
        {
            // This works by getting a box around the mesh and getting the midpoint between the min and max corners.
            // If the midpoint is not (0,0) then you subtract that midpoint on every point, as that will will make the midpoint of the box (0,0).

            Vector3 min = Vector3.Zero;
            Vector3 max = Vector3.Zero;
            Vector3 mid = Vector3.Zero;

            for (int i = 0; i < _verts.Count; i++)
            {
                min = Vector3.Min(min, _verts[i]);
                max = Vector3.Max(max, _verts[i]);
            }

            mid = ((max - min) * 0.5f) + min;

            for (int i = 0; i < _verts.Count; i++)
            {
                Vector3 temp = _verts[i];
                temp.X -= mid.X;
                temp.Y -= mid.Y;
                temp.Z -= mid.Z;
                _verts[i] = temp;
            }

            CreateDeclarationList();
        }*/
        #endregion

        private void SetGraphicsStatesFor3D()
        {
            //_gDevice.RasterizerState = Rasterizer3DNormal;
            //_gDevice.BlendState = Blend3DNormal;
            //_gDevice.DepthStencilState = DepthStencil3DNormal;
            //_gDevice.SamplerStates[0] = Sampler3DNormal;
            _gDevice.RasterizerState = Rasterizer3DNormal;
            _gDevice.BlendState = Blend3DNormal;
            _gDevice.DepthStencilState = DepthStencil3DNormal;
            _gDevice.SamplerStates[0] = Sampler3DNormal;
        }

        public void Update(GameTime gameTime, Matrix transform)
        {
            _aac.Update(gameTime);
            _player.Update(gameTime.ElapsedGameTime, true, transform);
        }

        public void Draw(GameTime gameTime, Matrix transform, Matrix view, Matrix proj)
        {
            Matrix[] bones = _player.GetSkinTransforms();
            Matrix[] transforms = new Matrix[_skin.SkeletonHierarchy.Count];

            transforms = _skeleton.CollectAbsoluteBoneTransforms();

            SetGraphicsStatesFor3D();

            Matrix worldProj = transform * view * proj;

            //_effect.CurrentTechnique = _effect.Techniques["ANSKLambert"];
            //_effect.Parameters["worldProj"].SetValue(worldProj);
            //_effect.Parameters["bones"].SetValue(bones);
            //_effect.Parameters["vsArrayIndex"].SetValue(0);
            //_effect.Parameters["psArrayIndex"].SetValue(1);
            //_effect.Parameters["ambientIntensity"].SetValue(1f);
            //_effect.Parameters["world"].SetValue(transform);
            //_effect.Parameters["diffuseColour"].SetValue(_materials[0].DiffuseColour.ToVector4());
            //_effect.Parameters["diffuseFactor"].SetValue((float)_materials[0].DiffuseFactor);
            //_effect.Parameters["diffuseLight"].SetValue(_materials[0].DiffuseLight);
            //_effect.Parameters["diffuseLight"].SetValue(Vector3.Forward);
            //_effect.Parameters["specColour"].SetValue(Color.Green.ToVector4());
            //_effect.Parameters["specPos"].SetValue(Vector3.Backward * 3);

            /*List<Vector3> temp = new List<Vector3>();
            for (int i = 0; i < _verticies.Length; i++)
            {
                //temp.Add(Vector3.Transform(_verticies[i].Position, bones[_verticies[i].Indices[0]]));// * bones[_verticies[i].Indices[0]].Translation * _verticies[i].Weights[0]);
                temp.Add(_verticies[i].Normal + Vector3.Transform(_verticies[i].Position, transform));
                Vector3 normal = temp[i];
                normal.Normalize();
                //temp[i] = Vector3.Transform(temp[i], worldProj);
                //DebugShapeRenderer.AddBoundingSphere(new BoundingSphere(temp[i], 1), Color.White);
                DebugShapeRenderer.AddLine(temp[i], temp[i] + normal, Color.White);
                //DebugShapeRenderer.AddBoundingSphere(new BoundingSphere(_verticies[i].Position, 1), Color.White);
            }*/

            /*foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                _game.GraphicsDevice.SetVertexBuffer(_vertBuffer);
                _game.GraphicsDevice.Indices = _indBuffer;

                pass.Apply();

                _game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _verts.Count, 0, _indicies.Count);
            }*/

            SetGraphicsStatesFor3D();
            _mesh.Draw(gameTime, transform, view, proj, bones);

            /*_testEffect.World = transform;
            _testEffect.View = view;
            _testEffect.Projection = proj;
            _testEffect.VertexColorEnabled = true;
            _testEffect.EnableDefaultLighting();

            foreach (EffectPass pass in _testEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _gDevice.DrawUserPrimitives<ANSKVertexDeclaration>(PrimitiveType.TriangleList, _mesh.VertexDeclarationArray, 0, _mesh.VertexDeclarationArray.Length / 3);
            }*/
        }
    }
}