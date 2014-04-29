using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelAnimationLibrary;
using Microsoft.Xna.Framework;
using ProjectSneakyGame;

namespace AnimationExample
{
    public class ANSKModelContainer : GameObject
    {
        private ANSKModel _model;

        public ANSKModelContainer(ANSKModel model, Game1 game, Vector3 pos) : base(game, pos)
        {
            _model = model;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _model.Update(gameTime, _main);
        }

        public void Draw(GameTime gameTime, Camera cam)
        {
            _model.Draw(gameTime, _main, cam.View, cam.Projection);

            base.Draw(gameTime);
        }
    }
}
