using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ANSKPipelineLibrary
{
    public class BlendShapeComplex
    {
        private BlendShape _shape;
        private int _min, _max, current, _toValue;
        private float _timePassed, _timeToWait;
        private bool _blend;
        private List<BlendShapeVert> _verts;
        private ANSKModel _model;

        public BlendShape Shape { get { return _shape; } }

        public BlendShapeComplex(BlendShape shape, ANSKModel model)
        {
            _shape = shape;
            _min = 0;
            _max = 1;
            _toValue = 0;
            _timePassed = 0.0f;
            _timeToWait = 0.0f;
            _blend = false;
            _verts = new List<BlendShapeVert>();
            ExtractBlendShapeVerts(model);
            _model = model;
        }

        public void Blend(int newVal)
        {
            current = newVal;

            for (int i = 0; i < _verts.Count; i++)
                _verts[i].Blend(current);

            EditModelVertexes();
        }

        private void EditModelVertexes()
        {
            for (int i = 0; i < _verts.Count; i++)
            {
                _model.Verticies[_verts[i].Indice] += _verts[i].BlendTo;
            }

            _model.CreateDeclarationList();
        }

        public void Update(GameTime g)
        {
            if (_blend)
            {

            }
        }

        private void ExtractBlendShapeVerts(ANSKModel model)
        {
            List<short> modelInds = model.Indices;
            List<short> check = new List<short>();

            for (int i = 0; i < modelInds.Count; i++)
            {
                for (int q = 0; q < _shape.Indices.Count; q++)
                {
                    if (_shape.Indices[q] == modelInds[i] && !check.Exists(temp => temp == _shape.Indices[q]))
                    {
                        _verts.Add(new BlendShapeVert(modelInds[i], _shape.Vertices[q]));
                        check.Add(modelInds[i]);
                    }
                }
            }
        }
    }
}
