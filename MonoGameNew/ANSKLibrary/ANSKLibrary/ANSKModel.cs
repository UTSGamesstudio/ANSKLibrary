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

        private AAC _aac;
        private SkinningData _skin;
        private AnimationPlayer _player;
        private AnimationClip _currentClip;

        private Skeleton _skeleton;
        private List<Joint> _joints;
        private Effect _effect;
        private ANSKVertexDeclarationAnimatable[] _verticies;
        private VertexBuffer _vertBuffer;
        private IndexBuffer _indBuffer;
        private GraphicsDevice _gDevice;
        private Game _game;

        private BasicEffect _testEffect;

        // This class is unfinished, so code has been rolled back to the original render code so this example can actually work.
        // Code marked with 'RollForward' is new code that been commented out so the old code could be used.
        private MeshRenderer _mesh;
        private Geometry _geo;

        // RollForward
        public List<Vector3> Verticies { get { return _mesh.Vertices; } set { _mesh.Vertices = value; CreateDeclarationList(); } }
        public List<short> Indices { get { return _mesh.Indices; } }

        public AnimationPlayer Player { get { return _player; } }
        public AnimationClip AnimationClip { get { return _currentClip; } set { _currentClip = value; } }
        public AAC AAC { get { return _aac; } }
        public MeshRenderer MeshRenderer { get { return _mesh; } set { _mesh = value; } }
        public Geometry Geometry { get { return _geo; } set { _geo = value; } }

        public ANSKModel(ANSKModelContent content)
        {            
            _geo = new Geometry(content.Mesh);

            _skeleton = content.Joints;

            if (_skeleton != null)
            {
                _skeleton.Init();
                _joints = _skeleton.ToJointList();
            }
            if (content.Skin != null)
                _skin = content.Skin;

            _mesh = new MeshRenderer(content.Mesh, _joints);

            _aac = new AAC();
            _aac.LoadBlendShapes(content.BlendShapes, this);
        }


        public void PlayAnimation(string name)
        {
            if (_skin != null)
            {
                _currentClip = _skin.AnimationClips[name];

                _player.StartClip(_currentClip);
            }
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

            if (_skin != null)
                _player = new AnimationPlayer(_skin);

        }

        public void CreateDeclarationList()
        {
            _mesh.Refresh();
        }

        public void Refresh()
        {
            _mesh.Refresh();
        }

        private void SetGraphicsStatesFor3D()
        {
            _gDevice.RasterizerState = Rasterizer3DNormal;
            _gDevice.BlendState = Blend3DNormal;
            _gDevice.DepthStencilState = DepthStencil3DNormal;
            _gDevice.SamplerStates[0] = Sampler3DNormal;
        }

        public void Update(GameTime gameTime, Matrix transform)
        {
            _aac.Update(gameTime);
            if (_player != null)
                _player.Update(gameTime.ElapsedGameTime, true, transform);
        }

        public void Draw(GameTime gameTime, Matrix transform, Matrix view, Matrix proj)
        {
            Matrix[] bones = null;
            Matrix[] transforms = null;

            if (_player != null)
            {
                bones = _player.GetBoneTransforms();
                transforms = new Matrix[_skin.SkeletonHierarchy.Count];
                if (_skeleton != null)
                    transforms = _skeleton.CollectAbsoluteBoneTransforms();
            }

            SetGraphicsStatesFor3D();

            Matrix worldProj = transform * view * proj;

            SetGraphicsStatesFor3D();

            _mesh.Draw(gameTime, transform, view, proj, bones);
        }
    }
}