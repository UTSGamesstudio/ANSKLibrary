using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectSneakyGame
{
    public delegate void EndCutsceneFunction();
    public delegate bool UpdateCinematicCutscene();

    /// <summary>
    /// Base class for cameras.
    /// Includes set-up and functions for basic 3D camera use.
    /// </summary>
    public class Camera : MovableComponent
    {
        protected Vector3 _targetPos;           // The target vector in which the camera looks.
        protected Vector3 _up;                  // The up vector of the camera.
        protected GameObject _target;           // The specified GameObject that the camera will look at.
        protected Matrix _view;                 // The view matrix.
        protected Matrix _projection;           // The projection matrix.
        protected Game _game;                   // The reference to Game1.
        protected bool _targetSpecified;        // Toggle for whether the camera will look freely, or focus to a target.
        protected Vector3 _posOffset;           // An offset of the camera's look at when looking at a target. Used when you want the camera to look slightly off the target.
        protected bool _inCutscene;             // Boolean for when the camera is busy looking at a cutscene. Useful for stopping enemy and player updates when this is true, as player might have lost camera focus.
        protected CutsceneData _cutData;
        protected bool _controlOverride;

        public GameObject Target { get { return _target; } set { _target = value; } }
        public Vector3 TargetPos { get { return _targetPos; } set { _targetPos = value; } }
        public Vector3 Up { get { return _up; } set { _up = value; } }
        public Matrix View { get { return _view; } set { _view = value; } }
        public Matrix Projection { get { return _projection; } set { _projection = value; } }
        public Vector3 Side { get { return  Vector3.Cross(_up, _targetPos); } }
        public bool TargetSpecified { get { return _targetSpecified; } set { _targetSpecified = value; } }
        public Vector3 PositionOffset { get { return _posOffset; } set { _posOffset = value; } }
        public bool InCutscene { get { return _inCutscene; } }
        public bool ControlOverride { get { return _controlOverride; } set { _controlOverride = value; } }

        public Camera(Game game)
            : base(game)
        {
            _game = game;
            _targetSpecified = false;
            _target = null;
            _posOffset = Vector3.Zero;
            _inCutscene = false;
            _cutData = null;
            ResetCamera();
            _controlOverride = false;
        }

        public Camera(Game game, Vector3 pos, Vector3 targetPos, Vector3 up) : base(game, pos)
        {
            _game = game;
            _targetPos = targetPos;
            _up = up;
            _target = null;
            _targetSpecified = false;
            _posOffset = Vector3.Zero;
            _inCutscene = false;
            _cutData = null;
            SetProjection();
            _controlOverride = false;
        }

        /// <summary>
        /// Resetting of the matrixes.
        /// </summary>
        protected virtual void SetMatrixes()
        {
            _pos = new Vector3(0, 0, 50);
            _targetPos = new Vector3();
            _up = Vector3.Up;
        }

        /// <summary>
        /// The creation of the view and projection matrix.
        /// </summary>
        protected virtual void SetProjection()
        {
            if (_targetSpecified && _target != null)
                _targetPos = _target.Pos;

            _view = Matrix.CreateLookAt(_pos, _targetPos, _up);
            _projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 
                (float)_game.Window.ClientBounds.Width / 
                (float)_game.Window.ClientBounds.Height, 
                1, 100);
        }

        public void ResetCamera()
        {
            SetMatrixes();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_targetSpecified)
                _targetPos = _target.Pos + _posOffset;
            UpdateViewMatrix(gameTime);
        }

        /// <summary>
        /// Re-calculate the view matrix.
        /// </summary>
        protected virtual void UpdateViewMatrix(GameTime g)
        {
            if (_cutData != null)
            {
                if (_cutData.Update(g))
                {
                    _target = _cutData.OriginalTarget;
                    _pos = _cutData.OriginalPosition;
                    _posOffset = _cutData.OriginalOffset;
                    _cutData.EndFunction();
                    _cutData = null;
                    _inCutscene = false;
                }
                else
                    _view = Matrix.CreateLookAt(_pos + _posOffset, _cutData.Target.TranslationM.Translation, Up); //_cutData.Target.TranslationM.Translation, Up);
            }
            else if (_targetSpecified && _target != null)
            {
                _targetPos.Normalize();
                _view = Matrix.CreateLookAt(_pos + _posOffset, _target.TransformationM.Translation, Up);
            }
            else
            {
                _targetPos.Normalize();
                _view = Matrix.CreateLookAt(_pos + _posOffset, _targetPos + _pos, Up);
            }
        }

        /// <summary>
        /// Move the camera object in the target direction.
        /// </summary>
        /// <param name="g"></param>
        public override void MoveForward(GameTime g)
        {
            Vector3 temp = _targetPos;
            temp.Y = 0;
            temp.Normalize();
            temp *= (_speed * (float)g.ElapsedGameTime.TotalSeconds);
            _pos += temp;
        }

        /// <summary>
        /// Move the camera object away from the target direction.
        /// </summary>
        /// <param name="g"></param>
        public override void MoveBackward(GameTime g)
        {
            Vector3 temp = _targetPos;
            temp.Y = 0;
            temp.Normalize();
            temp *= (_speed * (float)g.ElapsedGameTime.TotalSeconds);
            _pos -= temp;
        }

        /// <summary>
        /// Move the camera object in the up direction.
        /// </summary>
        /// <param name="g"></param>
        public override void MoveUp(GameTime g)
        {
            _pos += _up * (_speed * (float)g.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Move the camera object away from the up direction.
        /// </summary>
        /// <param name="g"></param>
        public override void MoveDown(GameTime g)
        {
            _pos -= _up * (_speed * (float)g.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Strafe the camera object towards the left.
        /// </summary>
        /// <param name="g"></param>
        public override void MoveLeft(GameTime g)
        {
            Vector3 temp = Side;
            temp.Normalize();
            temp.Y = 0;
            temp *= (_speed * (float)g.ElapsedGameTime.TotalSeconds);
            _pos += temp;
        }

        /// <summary>
        /// Strafe the camera object towards the right.
        /// </summary>
        /// <param name="g"></param>
        public override void MoveRight(GameTime g)
        {
            Vector3 temp = Side;
            temp.Normalize();
            temp.Y = 0;
            temp *= (_speed * (float)g.ElapsedGameTime.TotalSeconds);
            _pos -= temp;
        }

        /// <summary>
        /// Move the camera object in a specified direction.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="direction"></param>
        public override void Move(GameTime g, Vector3 direction)
        {
            _pos += direction * (_speed * (float)g.ElapsedGameTime.TotalSeconds);
        }

        public virtual void Yaw(float angle)
        {
            _targetPos = Vector3.Transform(_targetPos, Matrix.CreateFromAxisAngle(Up, angle));
        }

        /// <summary>
        /// Rotate the camera object along it's up direction.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="angle"></param>
        public virtual void Yaw(GameTime g, float angle)
        {
           _targetPos = Vector3.Transform(_targetPos, Matrix.CreateFromAxisAngle(Up, (float)g.ElapsedGameTime.TotalSeconds * angle));
        }

        public virtual void Roll(float angle)
        {
            _targetPos = Vector3.Transform(_targetPos, Matrix.CreateFromAxisAngle(_targetPos, angle));
        }

        /// <summary>
        /// Rotate the camera object along the target direction.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="angle"></param>
        public virtual void Roll(GameTime g, float angle)
        {
            _targetPos = Vector3.Transform(_targetPos, Matrix.CreateFromAxisAngle(_targetPos, (float)g.ElapsedGameTime.TotalSeconds * angle));
        }

        public virtual void Pitch(float angle)
        {
            _targetPos = Vector3.Transform(_targetPos, Matrix.CreateFromAxisAngle(Side, angle));
        }

        /// <summary>
        /// Rotate the camera object along the side direction.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="angle"></param>
        public virtual void Pitch(GameTime g, float angle)
        {
            _targetPos = Vector3.Transform(_targetPos, Matrix.CreateFromAxisAngle(Side, (float)g.ElapsedGameTime.TotalSeconds * angle));
        }

        public void RunCutsceneLookAtTimed(GameObject obj, float time, Vector3 offset, EndCutsceneFunction func)
        {
            _cutData = new CutsceneData(obj, time, _target, _pos, _posOffset, func);
            _inCutscene = true;
            _target = obj;
            _posOffset = offset;
            _targetSpecified = true;
        }
    }

    public class CinematicCutsceneContent
    {
        private Vector3 _origPosOffset, _origPos;
        private float _timePassed, _time;
        private GameObject _target, _origTarget;


        public CinematicCutsceneContent()
        {

        }
    }

    public class CutsceneData
    {
        private GameObject _target, _origTarget;
        private float _time, _timePassed;
        private Vector3 _origPos, _origOff;
        private EndCutsceneFunction _func;

        public GameObject Target { get { return _target; } }
        public GameObject OriginalTarget { get { return _origTarget; } }
        public Vector3 OriginalPosition { get { return _origPos; } }
        public Vector3 OriginalOffset { get { return _origOff; } }
        public EndCutsceneFunction EndFunction { get { return _func; } }

        public CutsceneData(GameObject obj, float time, GameObject origTarget, Vector3 origPos, Vector3 origOffset, EndCutsceneFunction func)
        {
            _target = obj;
            _time = time;
            _timePassed = 0;
            _origTarget = origTarget;
            _origPos = origPos;
            _origOff = origOffset;
            _func = func;
        }

        public bool Update(GameTime g)
        {
            _timePassed += (float)g.ElapsedGameTime.TotalSeconds;

            return (_timePassed > _time);
        }
    }
}
