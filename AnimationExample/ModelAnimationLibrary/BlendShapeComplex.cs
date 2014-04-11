using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ModelAnimationLibrary
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

            //for (int i = 0; i < _verts.Count; i++)
            //  _verts[i].ResetBlend();
        }

        private void EditModelVertexes()
        {
            /*Vector3[] store;

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
            
            }*/

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
            /*List<short> check = new List<short>();
            Vector3[] verts;
            short[] inds;

            for (int i = 0; i < model.Meshes.Count; i++)
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
