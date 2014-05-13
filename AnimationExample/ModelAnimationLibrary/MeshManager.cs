using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModelAnimationLibrary
{
    public class MeshManager
    {
        private List<MeshPart> _meshParts;
        private MeshContent _meshData;
        private List<Joint> _joints;
        private ANSKVertexDeclaration[] _verts;
        private short[] _indices;

        public List<Vector3> Vertices { get { return _meshData.Verticies; } set { _meshData.Verticies = value; } }
        public ANSKVertexDeclaration[] VertexDeclarationArray { get { return _verts; } }
        public List<short> Indices { get { return _meshData.VertexIndicies; } set { _meshData.VertexIndicies = value; } }
        public short[] IndiceArray { get { return _indices; } }

        public MeshManager(MeshContent mesh, List<Joint> joints)
        {
            _meshParts = new List<MeshPart>();
            _meshData = mesh;
            _joints = joints;

            for (int i = 0; i < _meshData.Materials.Materials.Count; i++)
            {
                _meshParts.Add(new MeshPart(_meshData.Materials.Materials[i]));
            }

            Refresh();
        }

        private int4 VertexToJointIndices(int vertIndex)
        {
            int4 ints = new int4();
            ints.Init();

            for (int i = 0; i < _joints.Count; i++)
            {
                if (_joints[i].IsIndicePartOfThisJoint(vertIndex))
                    ints.AddInt(i);
            }

            return ints;
        }

        private float4 VertexToJointsWeights(int vertIndex, int4 ints)
        {
            float4 floats = new float4();
            floats.Init();

            for (int i = 0; i < ints.Count; i++)
            {
                floats.AddFloat(_joints[ints[i]].GetWeight(vertIndex));
            }

            return floats;
        }

        public void Refresh()
        {
            for (int i = 0; i < _meshData.Materials.Materials.Count; i++)
            {
                _meshParts[i].Reset();

                for (int q = 0; q < _meshData.Verticies.Count; q++)
                {
                    if (_meshData.Materials.MaterialIndicieList[q] == i)
                    {
                        int4 ints = VertexToJointIndices(q);
                        _meshParts[_meshData.Materials.MaterialIndicieList[q]].AddVertex(_meshData.Verticies[q], _meshData.Uvs[_meshData.UvIndicies[q]], _meshData.Normals[q],
                                                                                        ints, VertexToJointsWeights(q, ints), ints.Count, _meshData.VertexIndicies[q]);
                    }
                }
            }

            _indices = GetIndicieList();
            _verts = GetVertices();
        }

        public short[] GetIndicieList()
        {
            List<short> inds = new List<short>();

            for (int i = 0; i < _meshParts.Count; i++)
                inds.AddRange(_meshParts[i].CollectIndices());

            return inds.ToArray();
        }

        public ANSKVertexDeclaration[] GetVertices()
        {
            List<ANSKVertexDeclaration> verts = new List<ANSKVertexDeclaration>();

            for (int i = 0; i < _meshParts.Count; i++)
                verts.AddRange(_meshParts[i].CollectVertices());

            return verts.ToArray();
        }

        public void CenterModelToOrigin()
        {
            // This works by getting a box around the mesh and getting the midpoint between the min and max corners.
            // If the midpoint is not (0,0) then you subtract that midpoint on every point, as that will will make the midpoint of the box (0,0).

            Vector3 min = Vector3.Zero;
            Vector3 max = Vector3.Zero;
            Vector3 mid = Vector3.Zero;

            for (int i = 0; i < _meshData.Verticies.Count; i++)
            {
                min = Vector3.Min(min, _meshData.Verticies[i]);
                max = Vector3.Max(max, _meshData.Verticies[i]);
            }

            mid = ((max - min) * 0.5f) + min;

            for (int i = 0; i < _meshData.Verticies.Count; i++)
            {
                Vector3 temp = _meshData.Verticies[i];
                temp.X -= mid.X;
                temp.Y -= mid.Y;
                temp.Z -= mid.Z;
                _meshData.Verticies[i] = temp;
            }

            Refresh();
        }
    }
}
