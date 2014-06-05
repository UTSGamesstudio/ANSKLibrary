using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ANSKPipelineLibrary
{
    public class MeshRenderer
    {
        public enum ModelRenderType { Normal, Animatable }

        private List<MeshPart> _meshParts;
        private MeshContent _meshData;
        private List<Joint> _joints;
        private ANSKVertexDeclarationAnimatable[] _vertsAnim;
        private ANSKVertexDeclarationModel[] _vertsModel;
        private short[] _indices;
        private ModelRenderType _renderType;

        public List<Vector3> Vertices { get { return _meshData.Verticies; } set { _meshData.Verticies = value; } }
        public ANSKVertexDeclarationAnimatable[] VertexDeclarationAnimatableArray { get { return _vertsAnim; } }
        public ANSKVertexDeclarationModel[] VertexDeclarationModelArray { get { return _vertsModel; } }
        public List<short> Indices { get { return _meshData.VertexIndicies; } set { _meshData.VertexIndicies = value; } }
        public short[] IndiceArray { get { return _indices; } }
        public Effect MeshEffect { set { for (int i = 0; i < _meshParts.Count; i++) _meshParts[i].MeshEffect = value; } }
        public GraphicsDevice MeshGraphicDevice { set { for (int i = 0; i < _meshParts.Count; i++) _meshParts[i].MeshGraphicsDevice = value; } }

        public MeshRenderer(MeshContent mesh, List<Joint> joints)
        {
            _meshParts = new List<MeshPart>();
            _meshData = mesh;
            _joints = joints;

            if (_joints == null)
                _renderType = ModelRenderType.Normal;
            else
                _renderType = ModelRenderType.Animatable;

            for (int i = 0; i < _meshData.Materials.Materials.Count; i++)
            {
                _meshParts.Add(new MeshPart(_meshData.Materials.Materials[i]));
            }

            //Refresh();
        }

        public void SetTexture(Texture2D texture)
        {
            foreach (MeshPart part in _meshParts)
                part.Texture = texture;
        }

        public void SetTexture(Texture2D texture, string materialName)
        {
            foreach (MeshPart part in _meshParts)
            {
                if (part.MeshMaterial.Name == "Material::" + materialName)
                    part.Texture = texture;
            }
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

                for (int q = 0; q < _meshData.VertexIndicies.Count; q++)
                {
                    if (_meshData.Materials.MaterialIndicieList[q] == i)
                    {
                        switch (_renderType)
                        {
                            case ModelRenderType.Animatable:
                                int4 ints = VertexToJointIndices(_meshData.VertexIndicies[q]);
                                _meshParts[_meshData.Materials.MaterialIndicieList[q]].AddVertex(_meshData.Verticies[_meshData.VertexIndicies[q]], _meshData.Uvs[_meshData.UvIndicies[_meshData.VertexIndicies[q]]],
                                                                                        _meshData.Normals[_meshData.VertexIndicies[q]], ints, VertexToJointsWeights(_meshData.VertexIndicies[q], ints), 
                                                                                        ints.Count, _meshData.VertexIndicies[q]);
                                break;
                            case ModelRenderType.Normal:
                                _meshParts[_meshData.Materials.MaterialIndicieList[q]].AddVertex(_meshData.Verticies[_meshData.VertexIndicies[q]], _meshData.Uvs[_meshData.UvIndicies[_meshData.VertexIndicies[q]]],
                                                                                        _meshData.Normals[_meshData.VertexIndicies[q]], _meshData.VertexIndicies[q]);
                                break;
                        }
                        
                    }
                }

                _meshParts[i].Finalise();
            }

            _indices = GetIndicieList();
            _vertsAnim = GetAnimVertices();
            _vertsModel = GetModelVertices();
        }

        public short[] GetIndicieList()
        {
            List<short> inds = new List<short>();

            for (int i = 0; i < _meshParts.Count; i++)
                inds.AddRange(_meshParts[i].CollectIndices());

            return inds.ToArray();
        }

        public ANSKVertexDeclarationAnimatable[] GetAnimVertices()
        {
            List<ANSKVertexDeclarationAnimatable> verts = new List<ANSKVertexDeclarationAnimatable>();

            for (int i = 0; i < _meshParts.Count; i++)
                verts.AddRange(_meshParts[i].CollectAnimVertices());

            return verts.ToArray();
        }

        public ANSKVertexDeclarationModel[] GetModelVertices()
        {
            List<ANSKVertexDeclarationModel> verts = new List<ANSKVertexDeclarationModel>();

            for (int i = 0; i < _meshParts.Count; i++)
                verts.AddRange(_meshParts[i].CollectModelVertices());

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

        public void Draw(GameTime gameTime, Matrix transform, Matrix view, Matrix proj, Matrix[] bones)
        {
            Matrix worldProj = transform * view * proj;

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i] = FixBoneMatrix(bones[i], _joints[i]);
            }

            for (int i = 0; i < _meshParts.Count; i++)
            {
                _meshParts[i].MeshEffect.Parameters["worldProj"].SetValue(worldProj);
                if (bones != null)
                    _meshParts[i].MeshEffect.Parameters["bones"].SetValue(bones);
                _meshParts[i].MeshEffect.Parameters["ambientIntensity"].SetValue(1f);
                _meshParts[i].MeshEffect.Parameters["world"].SetValue(transform);
                _meshParts[i].MeshEffect.Parameters["diffuseColour"].SetValue(_meshParts[i].MeshMaterial.DiffuseColour.ToVector4());
                _meshParts[i].MeshEffect.Parameters["diffuseFactor"].SetValue((float)_meshParts[i].MeshMaterial.DiffuseFactor);
                _meshParts[i].MeshEffect.Parameters["diffuseLight"].SetValue(Vector3.Forward);

                _meshParts[i].Draw(gameTime, bones);
            }
        }

        private Matrix FixBoneMatrix(Matrix bone, Joint joint)
        {
            bone = Matrix.Subtract(bone, joint.Transformation);
            return bone;
        }
    }
}
