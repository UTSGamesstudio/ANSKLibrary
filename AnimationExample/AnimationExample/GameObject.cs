using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AnimationExample;

namespace ProjectSneakyGame
{
    /// <summary>
    /// Base class of all objects that will be 'physically' in the game.
    /// </summary>
    public class GameObject : DrawableMovableComponent
    {
        public enum GameObjectVertexShader { Basic = 0 }
        public enum GameObjectPixelShader 
        { 
            Basic = 0, 
            BasicDamage,
            BasicTransparent, 
            BasicDamageTransparent, 
            BasicTransparentGrayscale,
            BasicDamageTransparentGrayscale, 
            NPCScientistBasic, 
            NPCScientistBasicDamage, 
            NPCGuardBasic,
            NPCGuardBasicDamage,
            NPCGuardAlert
        }
        public enum GameObjectType { Player, Scientist, Guard }

        public static int _refCount = 0;

        public static int CollectRefCount() { _refCount++; return _refCount-1; }
        public static void Reset() { _refCount = 0; }

        protected BasicEffect _effect;
        protected int _ref;
        protected float _cooldownTime, _cooldownTimePast;
        protected bool _hurt;
        protected GameObjectType _type;
        protected Vector3 _hideStartPos;
        protected Vector3 _hidingPos;
        protected int _health;
        protected bool _dead, _dying;
        protected bool _canAttack, _attacking;
        protected float _attackTime, _attackTimePast;

        public int ReferenceNumber { get { return _ref; } }
        public GameObjectType Type { get { return _type; } set { _type = value; } }
        public int Health { get { return _health; } set { _health = value; } }
        public bool Dead { get { return _dead; } }
        public bool CanAttack { get { return !_dying && _canAttack; } }

        public GameObject(Game1 game)
            : base(game)
        {
            _game = game;
            _ref = CollectRefCount();
            _cooldownTime = 0;
            _cooldownTimePast = 0;
            _hurt = false;
            _health = 4;
        }

        public GameObject(Game1 game, Vector3 pos)
            : base(game, pos)
        {
            _game = game;
            _ref = CollectRefCount();
            _cooldownTime = 0;
            _cooldownTimePast = 0;
            _hurt = false;
            _health = 4;
        }

        public GameObject(Game1 game, Vector3 pos, float speed)
            : base(game, pos, speed)
        {
            _game = game;
            _ref = CollectRefCount();
            _cooldownTime = 0;
            _cooldownTimePast = 0;
            _hurt = false;
            _health = 4;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public virtual bool IsColliding(BoundingBox box)
        {
            return _bBox.BBox.Intersects(box);
        }

        public virtual bool IsColliding(BoundingSphere sphere)
        {
            return _bBox.BBox.Intersects(sphere);
        }

        public virtual bool IsColliding(List<BoundingSphere> spheres)
        {
            for (int i = 0; i < spheres.Count; i++)
            {
                if (_bBox.BBox.Intersects(spheres[i]))
                    return true;
            }

            return false;
        }

        public virtual bool IsCollidingHead(BoundingBox box)
        {
            return false;
        }

        public virtual bool IsCollidingHead(BoundingSphere sphere)
        {
            return false;
        }

        public virtual bool IsCollidingHead(List<BoundingSphere> spheres)
        {
            return false;
        }

        public virtual void Damage(int damage) { }
        public virtual void Undamage() { }

        public virtual void Die() { }


        public virtual BoundingSphere GetBoundingSphere() { return new BoundingSphere(); }
    }
}
