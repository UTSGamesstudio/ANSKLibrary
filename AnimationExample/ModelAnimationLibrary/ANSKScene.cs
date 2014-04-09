using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ModelAnimationLibrary
{
    public class ANSKScene
    {
        private Dictionary<string, ANSKModel> _models;
        private List<string> _looper;
        private Game _game;

        public ANSKScene(Game game)
        {
            _game = game;
            _looper = new List<string>();
        }

        public void LoadANSKModel(string name)
        {
            if (_models.ContainsKey(name))
                throw new Exception("Already loaded model " + name + " in the ANSKScene instance.");

            _models.Add(name, _game.Content.Load<ANSKModel>(name));
            _looper.Add(name);
        }

        public void LoadANSKModel(params string[] name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                LoadANSKModel(name[i]);
            }
        }

        public void RemoveANSKModel(string name)
        {
            _models.Remove(name);
            _looper.Remove(name);
        }

        public void RemoveAllANSKModel()
        {
            _models.Clear();
            _looper.Clear();
        }

        public void UpdateModels(GameTime gameTime)
        {
            for (int i = 0; i < _looper.Count; i++)
            {
                //_models[_looper[i]].Update(gameTime);
            }
        }

        public void DrawModels(GameTime gameTime)
        {
            for (int i = 0; i < _looper.Count; i++)
            {
                //_models[_looper[i]].Draw(gameTime);
            }
        }
    }
}
