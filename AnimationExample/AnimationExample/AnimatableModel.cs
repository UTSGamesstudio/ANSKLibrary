using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelAnimationLibrary;
using ProjectSneakyGame;

namespace AnimationExample
{
    public class AnimatableModel
    {
        private Game1 _game;
        private ANSKModel _model;
        private AnimationPlayer _player;
        //private SkinningData _skinData;
        private ANSK _ansk;
        private AnimationClip _currentClip;
        protected List<BoundingSphere> _spheres;
        protected List<BoundingSphere> _headSpheres;
        protected BoundingSphere _parentBoneSphere;
        protected Texture2D _modelTexture;
        protected Vector3 _hurtColour;
        protected float _hurtDamp, _alpha;
        protected float _alertDamp;
        protected float _collisionSphereRadius;
        protected float _parentCollisionSphereRadius;

        public int psindex = 0;
        public int vsindex = 0;

        public ANSKModel Model { get { return _model; } }
        public AnimationClip CurrentClip { get { return _currentClip; } }
        public bool IsPaused { get { return _player.IsPaused; } }
        public List<BoundingSphere> CollisionSpheres { get { return _spheres; } }
        public List<BoundingSphere> HeadCollisionSpheres { get { return _headSpheres; } }
        public BoundingSphere ParentBoneCollisionSphere { get { return _parentBoneSphere; } }
        public Texture2D ModelTexture { get { return _modelTexture; } set { _modelTexture = value; } }
        public Vector3 HurtColour { get { return _hurtColour; } set { _hurtColour = value; } }
        public float HurtDamp { get { return _hurtDamp; } set { if (value < 0) _hurtDamp = 0; else if (value > 1) _hurtDamp = 1; else _hurtDamp = value; } }
        public float Alpha { get { return _alpha; } set { if (value < 0) _alpha = 0; else if (value > 1) _alpha = 1; else _alpha = value; } }
        public bool Static { get { return _player.Static; } set { _player.Static = value; } }
        public float CollisonSphereRadius { get { return _collisionSphereRadius; } set { _collisionSphereRadius = value; } }
        public float ParentBoneCollisionSphereRadius { get { return _parentCollisionSphereRadius; } set { _parentCollisionSphereRadius = value; } }
        public bool EndOfAnimation { get { return _player.EndOfClip; } }
        public float AlertDamp { get { return _alertDamp; } set { _alertDamp = value; } }
        public ANSK ANSK { get { return _ansk; } }

        public AnimatableModel(Game1 game, ANSKModelContent model, Vector3 pos)
        {
            _game = game;
            _model = new ANSKModel(model);
            _modelTexture = null;

            //_model = ModelRegistry.GetModel("TestNormalBlendOppCorners2");

            _ansk = new ANSK( _model, game);
            //_skinData = _model.Tag as ANSKTagData;

            //if (_skinData == null)
            if (_ansk.SkinningAndBasicAnims == null)
                throw new InvalidOperationException("The model " + _model.ToString() + " does not contain the data needed for animations.");

            //_player = new AnimationPlayer(_skinData);
            _player = new AnimationPlayer(_ansk.SkinningAndBasicAnims);

            _currentClip = null;

            _player.Looped = false;

            _collisionSphereRadius = 0.1f;
            _parentCollisionSphereRadius = 0.6f;

            GenerateBoundingSpheres(Matrix.Identity);
        }

        public void PlayAnimation(string name)
        {
            //_currentClip = _skinData.AnimationClips[name];
            _currentClip = _ansk.SkinningAndBasicAnims.AnimationClips[name];

            _player.StartClip(_currentClip);

            _player.Looped = true;
        }

        public void PlayAnimation(string name, bool looped)
        {
            //_currentClip = _skinData.AnimationClips[name];
            _currentClip = _ansk.SkinningAndBasicAnims.AnimationClips[name];

            _player.StartClip(_currentClip, looped);
        }

        public void PauseAnimation()
        {
            _player.Pause();
        }

        public void ResumeAnimation()
        {
            _player.Resume();
        }

        public void ChangeAnimationSpeedTicks(float speed)
        {
            _player.Speed += speed;
        }

        public void ChangeAnimationSpeedSeconds(float speed)
        {
            _player.Speed += speed * 100000;
        }

        public void ResetAnimationSpeed()
        {
            _player.Speed = 0;
        }

        private void GenerateBoundingSpheres(Matrix transform)
        {
            _spheres = new List<BoundingSphere>();
            _headSpheres = new List<BoundingSphere>();

            Matrix[] bones = _player.GetWorldTransforms();

            _parentBoneSphere = new BoundingSphere(bones[0].Translation, _parentCollisionSphereRadius);

            for (int i = 0; i < bones.Length; i++)
            {
                Vector3 bPos = bones[i].Translation;
                _spheres.Add(new BoundingSphere(bPos, _collisionSphereRadius));
            }

            bones = _player.GetHeadWorldTransforms();

            for (int i = 0; i < bones.Length; i++)
            {
                Vector3 bPos = bones[i].Translation;
                _headSpheres.Add(new BoundingSphere(bPos, _collisionSphereRadius));
            }
        }

        public void Update(GameTime gameTime, Matrix transform)
        {
            _player.Update(gameTime.ElapsedGameTime, true, transform);

            _ansk.Update(gameTime);

            GenerateBoundingSpheres(transform);
        }

        public void Draw(GameTime gameTime, Matrix transform, Camera c)
        {
            /*Matrix[] bones = _player.GetSkinTransforms();
            Matrix[] transforms = new Matrix[_model.Bones.Count];

            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                Matrix worldProj = transforms[mesh.ParentBone.Index] * c.View * c.Projection;

                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["worldProj"].SetValue(worldProj);
                    //effect.Parameters["modelTexture"].SetValue(_modelTexture);
                    effect.Parameters["bones"].SetValue(bones);
                    effect.Parameters["vsArrayIndex"].SetValue(vsindex);
                    effect.Parameters["psArrayIndex"].SetValue(psindex);

                    effect.CurrentTechnique.Passes[0].Apply();
                }

                mesh.Draw();
            }*/
        }

        /*static public implicit operator Model(AnimatableModel model)
        {
            return model._model;
        }*/
    }
}
