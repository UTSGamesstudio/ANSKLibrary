using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectSneakyGame
{
    /// <summary>
    /// Extension of the GameComponent class.
    /// Includes set-up and functions for 3D matrix movement of GameComponent types.
    /// </summary>
    public class MovableComponent : GameComponent
    {
        protected float _speed;
        protected Vector3 _pos;
        protected Matrix _trans, _rot, _scale, _main;

        public float Speed { get { return _speed; } set { _speed = value; } }
        public Vector3 Pos { get { return _pos; } set { _pos = value; } }
        public Matrix TranslationM { get { return _trans; } set { _trans = value; } }
        public Matrix RotationM { get { return _trans; } set { _trans = value; } }
        public Matrix ScaleM { get { return _scale; } set { _scale = value; } }
        public Matrix TransformationM { get { return _main; } }

        public MovableComponent(Game game):base(game)
        {
            _pos = Vector3.One;
            _main = Matrix.Identity;
            _trans = Matrix.Identity;
            _rot = Matrix.Identity;
            _scale = Matrix.Identity;
            _speed = 0;
        }

        public MovableComponent(Game game, Vector3 pos)
            : base(game)
        {
            _pos = pos;
            _trans.Translation = _pos;
            _rot = Matrix.Identity;
            _scale = Matrix.Identity;
            _main = Matrix.Identity;
            _speed = 0;
        }

        public MovableComponent(Game game, Vector3 pos, float speed)
            : base(game)
        {
            _pos = pos;
            _trans.Translation = _pos;
            _rot = Matrix.Identity;
            _scale = Matrix.Identity;
            _main = Matrix.Identity;
            _speed = speed;
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
            Vector3 temp = new Vector3(x, y, z);
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

        public override void Update(GameTime gameTime)
        {
            _main = _scale * _rot * _trans;

            base.Update(gameTime);
        }
    }
}
