using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AnimationExample;

namespace ProjectSneakyGame
{
    /// <summary>
    /// Extension of the DrawableGameComponent class.
    /// Includes set-up and functions for 3D matrix movement of DrawableGameComponent types.
    /// </summary>
    public class DrawableMovableComponent : DrawableGameComponent
    {
        protected float _speed;
        protected Vector3 _pos;
        protected Matrix _trans, _rot, _scale, _main;
        protected Game1 _game;
        protected MovableBoundingBox _bBox;

        public float Speed { get { return _speed; } set { _speed = value; } }
        public Vector3 Pos { get { return _pos; } set { _pos = value; } }
        public Matrix TranslationM { get { return _trans; } set { _trans = value; } }
        public Matrix RotationM { get { return _trans; } set { _trans = value; } }
        public Matrix ScaleM { get { return _scale; } set { _scale = value; } }
        public Matrix TransformationM { get { return _main; } }
        public MovableBoundingBox BoundingBox { get { return _bBox; } set { _bBox = value; } }

        public DrawableMovableComponent(Game1 game):base(game)
        {
            _pos = Vector3.One;
            _main = Matrix.Identity;
            _trans = Matrix.Identity;
            _rot = Matrix.Identity;
            _scale = Matrix.Identity;
            _game = game;
            _speed = 0;
        }

        public DrawableMovableComponent(Game1 game, Vector3 pos)
            : base(game)
        {
            _pos = pos;
            //_trans.Translation = _pos;
            _trans = Matrix.CreateTranslation(pos);
            _rot = Matrix.Identity;
            _scale = Matrix.Identity;
            _main = Matrix.Identity;
            _speed = 0;
            _game = game;
        }

        public DrawableMovableComponent(Game1 game, Vector3 pos, float speed)
            : base(game)
        {
            _pos = pos;
            //_trans.Translation = _pos;
            _trans = Matrix.CreateTranslation(pos);
            _rot = Matrix.Identity;
            _scale = Matrix.Identity;
            _main = Matrix.Identity;
            _speed = speed;
            _game = game;
        }

        public virtual void Translate(Vector3 vec, GameTime g)
        {
            _trans *= Matrix.CreateTranslation(vec * (float)g.ElapsedGameTime.TotalSeconds);
            _pos += vec * (float)g.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Translate(Vector3 vec)
        {
            _trans *= Matrix.CreateTranslation(vec);
            _pos += vec;
        }

        public virtual void Translate(float x, float y, float z, GameTime g)
        {
            Vector3 temp = new Vector3(x,y,z);
            _trans *= Matrix.CreateTranslation(temp * (float)g.ElapsedGameTime.TotalSeconds);
            _pos += temp * (float)g.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Translate(float x, float y, float z)
        {
            Vector3 temp = new Vector3(x, y, z);
            _trans *= Matrix.CreateTranslation(temp);
            _pos += temp;
        }

        public virtual void RotateX(float rot, GameTime g)
        {
            _rot *= Matrix.CreateRotationX(rot * (float)g.ElapsedGameTime.TotalSeconds);
            _pos += _rot.Translation;
        }

        public virtual void RotateX(float rot)
        {
            _rot *= Matrix.CreateRotationX(rot);
            _pos += _rot.Translation;
        }

        public virtual void RotateY(float rot, GameTime g)
        {
            _rot *= Matrix.CreateRotationY(rot * (float)g.ElapsedGameTime.TotalSeconds);
            _pos += _rot.Translation;
        }

        public virtual void RotateY(float rot)
        {
            _rot *= Matrix.CreateRotationY(rot);
            _pos += _rot.Translation;
        }

        public virtual void RotateZ(float rot, GameTime g)
        {
            _rot *= Matrix.CreateRotationZ(rot * (float)g.ElapsedGameTime.TotalSeconds);
            _pos += _rot.Translation;
        }

        public virtual void RotateZ(float rot)
        {
            _rot *= Matrix.CreateRotationZ(rot);
            _pos += _rot.Translation;
        }

        public virtual void RotateOnAxis(Vector3 axis, float rot, GameTime g)
        {
            _rot *= Matrix.CreateFromAxisAngle(axis, rot * (float)g.ElapsedGameTime.TotalSeconds);
            _pos += _rot.Translation;
        }

        public virtual void RotateOnAxis(Vector3 axis, float rot)
        {
            _rot *= Matrix.CreateFromAxisAngle(axis, rot);
            _pos += _rot.Translation;
        }

        public virtual void Scale(float scale, GameTime g)
        {
            _scale *= Matrix.CreateScale(scale * (float)g.ElapsedGameTime.TotalSeconds);
        }

        public virtual void Scale(float scale)
        {
            _scale *= Matrix.CreateScale(scale);
        }

        public virtual void Scale(float x, float y, float z, GameTime g)
        {
            Vector3 temp = new Vector3(x, y, z);
            temp *= (float)g.ElapsedGameTime.TotalSeconds;
            _scale *= Matrix.CreateScale(temp);
        }

        public virtual void Scale(float x, float y, float z)
        {
            _scale *= Matrix.CreateScale(x, y, z);
        }

        public virtual void Scale(Vector3 vec, GameTime g)
        {
            _scale *= Matrix.CreateScale(vec * (float)g.ElapsedGameTime.TotalSeconds);
        }

        public virtual void Scale(Vector3 vec)
        {
            _scale *= Matrix.CreateScale(vec);
        }

        public virtual void MoveForward(GameTime g)
        {
            Translate(Vector3.Forward * _speed, g);
        }

        public virtual void MoveBackward(GameTime g)
        {
            Translate(Vector3.Backward * _speed, g);
        }

        public virtual void MoveUp(GameTime g)
        {
            Translate(Vector3.Up * _speed, g);
        }

        public virtual void MoveDown(GameTime g)
        {
            Translate(Vector3.Down * _speed, g);
        }

        public virtual void MoveLeft(GameTime g)
        {
            Translate(Vector3.Left * _speed, g);
        }

        public virtual void MoveRight(GameTime g)
        {
            Translate(Vector3.Right * _speed, g);
        }

        public virtual void Move(GameTime g, Vector3 dir)
        {
            Translate(dir * _speed, g);
        }

        /// <summary>
        /// Resets the GraphicsDevice states so that they are able to draw 3D objects.
        /// </summary>
        protected void SetGraphicsStatesFor3D()
        {
            _game.GraphicsDevice.RasterizerState = GraphicsDeviceStates.Rasterizer3DNormal;
            _game.GraphicsDevice.BlendState = GraphicsDeviceStates.Blend3DNormal;
            _game.GraphicsDevice.DepthStencilState = GraphicsDeviceStates.DepthStencil3DNormal;
            _game.GraphicsDevice.SamplerStates[0] = GraphicsDeviceStates.Sampler3DNormal;
        }

        public override void Update(GameTime gameTime)
        {
            _main = _scale * _rot * _trans;

            if (_bBox != null)
                _bBox.Transform(_trans.Translation);

            base.Update(gameTime);
        }

        static public implicit operator BoundingBox(DrawableMovableComponent obj)
        {
            return obj._bBox;
        }
    }
}
