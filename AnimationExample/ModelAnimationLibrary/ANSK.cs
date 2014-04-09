using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModelAnimationLibrary
{
    public class ANSK
    {
        private SkinningData _skin;
        private AAC _aac;
        private ANSKModel _model;

        public SkinningData SkinningAndBasicAnims { get { return _skin; } }
        public AAC AdvancedAnimationControl { get { return _aac; } }

        public ANSK(ANSKModel model, Game game)
        {
            _aac = new AAC();

            //_skin = model.TagData.SkinData;

            /*if (data.BlendShapes.Count > 0)
            {
                _aac.LoadModel(model);
                _aac.LoadBlendShapes(data.BlendShapes, game);
            }*/

            _model = model;
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
