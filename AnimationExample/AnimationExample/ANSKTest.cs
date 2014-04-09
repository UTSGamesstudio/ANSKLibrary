using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectSneakyGame;
using ModelAnimationLibrary;

namespace AnimationExample
{
    public class ANSKTest : GameObject
    {
        public AnimatableModel _model;

        public ANSKTest(Game1 game, Vector3 pos, float speed, ANSKModelContent model)
            : base(game, pos, speed)
        {
            _model = new AnimatableModel(game, model, pos);

            //_model.ChangeAnimationSpeedTicks(-150000f);

            //Scale(0.3f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _model.Update(gameTime, _main);
        }

        public void FullBlend()
        {
            _model.ANSK.AdvancedAnimationControl.BlendShapesControl.ChangeBlendValue("OuterBlend", 1);
        }

        public void StartBleh()
        {
            _model.ResumeAnimation();
        }

        public void Draw(GameTime gameTime, Camera cam)
        {
            base.Draw(gameTime);

            _model.Draw(gameTime, _main, cam);
        }
    }
}
