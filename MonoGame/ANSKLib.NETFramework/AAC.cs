using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ANSKLibrary
{
    // Stands for "Advanced Animation Control"
    public class AAC
    {
        private BlendShapeManager _bManager;
        private ANSKModel _model;

        public BlendShapeManager BlendShapesControl { get { return _bManager; } }

        public AAC()
        {
            _bManager = null;
        }

        public void LoadModel(ANSKModel model)
        {
            _model = model;
        }

        public void LoadBlendShapes(List<BlendShapeContent> shapes, Game game)
        {
            _bManager = new BlendShapeManager(shapes, _model, game);
        }

        public void Update(GameTime g)
        {
            if (_bManager != null)
                _bManager.Update(g);
        }
    }

    public class BlendShapeManager
    {
        private List<BlendShapeComplex> _bShapes;
        private ANSKModel _model;

        public BlendShapeManager(List<BlendShapeContent> shapes, ANSKModel model, Game game)
        {
            /* _bShapes = new List<BlendShapeComplex>();
             for (int i = 0; i < shapes.Count; i++)
                 _bShapes.Add(new BlendShapeComplex(new BlendShape(shapes[i]), model, game));

             _model = model;*/
        }

        public void ChangeBlendValue(string name, int value)
        {
            /*if (value < 0 || value > 1)
                return;

            for (int i = 0; i < _bShapes.Count; i++)
            {
                if (_bShapes[i].Shape.Name.Contains(name))
                    _bShapes[i].Blend(value, _model);
            }*/
        }

        public void Update(GameTime g)
        {

        }

        public class BlendShapeComplex
        {
            private BlendShape _shape;
            private int _min, _max, current, _toValue;
            private float _timePassed, _timeToWait;
            private bool _blend;
            private List<BlendShapeVert> _verts;
            private Game _game;

            public BlendShape Shape { get { return _shape; } }

            public BlendShapeComplex(BlendShape shape, ANSKModel model, Game game)
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
                _game = game;
            }

            public void Blend(int newVal, Model model)
            {
                current = newVal;

                for (int i = 0; i < _verts.Count; i++)
                    _verts[i].Blend(current);

                EditModelVertexes(model);

                //for (int i = 0; i < _verts.Count; i++)
                //  _verts[i].ResetBlend();
            }

            private void EditModelVertexes(Model model)
            {
                Vector3[] store;

                for (int i = 0; i < _verts.Count; i++)
                {
                    _game.GraphicsDevice.SetVertexBuffer(null);
                    store = new Vector3[model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexBuffer.VertexCount];

                    VertexDeclaration vertexDec = model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexBuffer.VertexDeclaration;
                    var vertEle = vertexDec.GetVertexElements();

                    var posEle = vertEle.First(e => e.VertexElementUsage == VertexElementUsage.Position);
                    if (posEle.VertexElementFormat != VertexElementFormat.Vector3)
                        throw new NotSupportedException();

                    var positions = new Vector3[model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexBuffer.VertexCount];
                    model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexBuffer.GetData(model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexOffset * vertexDec.VertexStride + posEle.Offset, positions, 0, model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].NumVertices, vertexDec.VertexStride);
                    //model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexBuffer.GetData<Vector3>(positions);
                    positions[_verts[i].ArrayRef] += _verts[i].BlendTo;
                    //model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexBuffer.SetData<Vector3>(positions);
                    model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexBuffer.SetData(model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].VertexOffset * vertexDec.VertexStride + posEle.Offset, positions, 0, model.Meshes[_verts[i].ModelMeshRef].MeshParts[_verts[i].ModelMeshPartRef].NumVertices, vertexDec.VertexStride);

                }
            }

            public void Update(GameTime g)
            {
                if (_blend)
                {

                }
            }

            private void ExtractBlendShapeVerts(ANSKModel model)
            {
                List<short> check = new List<short>();
                Vector3[] verts;
                short[] inds;

                /*for (int i = 0; i < model.Meshes.Count; i++)
                {
                    for (int q = 0; q < model.Meshes[i].MeshParts.Count; q++)
                    {
                        //verts = new Vector3[model.Meshes[i].MeshParts[q].VertexBuffer.VertexCount];
                        //inds = new short[model.Meshes[i].MeshParts[q].IndexBuffer.IndexCount];

                        VertexDeclaration vertexDec = model.Meshes[i].MeshParts[q].VertexBuffer.VertexDeclaration;
                        var vertEle = vertexDec.GetVertexElements();

                        var posEle = vertEle.First(e => e.VertexElementUsage == VertexElementUsage.Position);
                        if (posEle.VertexElementFormat != VertexElementFormat.Vector3)
                            throw new NotSupportedException();

                        var positions = new Vector3[model.Meshes[i].MeshParts[q].NumVertices];
                        model.Meshes[i].MeshParts[q].VertexBuffer.GetData(model.Meshes[i].MeshParts[q].VertexOffset * vertexDec.VertexStride + posEle.Offset, positions, 0, model.Meshes[i].MeshParts[q].NumVertices, vertexDec.VertexStride);

                        var indexElementSize = (model.Meshes[i].MeshParts[q].IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits) ? 2 : 4;
                        if (indexElementSize != 2)
                        throw new NotSupportedException();

                        var indices = new short[model.Meshes[i].MeshParts[q].PrimitiveCount * 3];
                        model.Meshes[i].MeshParts[q].IndexBuffer.GetData(model.Meshes[i].MeshParts[q].StartIndex * 2, indices, 0, model.Meshes[i].MeshParts[q].PrimitiveCount * 3);

                        //model.Meshes[i].MeshParts[q].VertexBuffer.GetData<Vector3>(verts);
                        //model.Meshes[i].MeshParts[q].IndexBuffer.GetData<short>(inds);

                        for (int p = 0; p < _shape.Indices.Count; p++)
                        {
                            for (int w = 0; w < indices.Length; w++)
                            {
                                if (_shape.Indices[p] == indices[w] && !check.Exists(temp => temp == _shape.Indices[p]))
                                {
                                    _verts.Add(new BlendShapeVert(i, q, indices[w], p, _shape.Vertices[p]));
                                    check.Add(indices[w]);
                                }
                            }
                        }
                    }
                }*/
            }
        }
    }

    public class BlendShapeVert
    {
        private int _modelMeshRef;
        private int _modelMeshPartRef;
        private int _arrayref;
        private int _indice;
        private Vector3 _vert;
        private Vector3 _blendTo;
        private int _prevVal;

        public int ModelMeshRef { get { return _modelMeshRef; } }
        public int ModelMeshPartRef { get { return _modelMeshPartRef; } }
        public int ArrayRef { get { return _arrayref; } }
        public int Indice { get { return _indice; } }
        public Vector3 Vertex { get { return _vert; } }
        public Vector3 BlendTo { get { return _blendTo; } set { _blendTo = value; } }

        public BlendShapeVert(int modelMeshRef, int modelMeshPartRef, int arrayRef, int indice, Vector3 vert)
        {
            _modelMeshRef = modelMeshRef;
            _modelMeshPartRef = modelMeshPartRef;
            _arrayref = arrayRef;
            _indice = indice;
            _vert = vert;
            _blendTo = Vector3.Zero;
            _prevVal = 0;
        }

        public void Blend(int val)
        {
            float temp = val - _prevVal;

            if (temp == 0)
                _blendTo = Vector3.Zero;
            else
                _blendTo = new Vector3(_vert.X / temp, _vert.Y / temp, _vert.Z / temp);

            _prevVal = val;
        }

        public void ResetBlend()
        {
            _blendTo = Vector3.Zero;

            Blend(0);
        }
    }
}
