using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;

namespace ANSKLibrary
{
    public class XmlImporter
    {
        public static ANSKModelContent LoadANSK(string name)
        {
            // MeshContent files.
            MeshContent mesh = null;
            Skeleton skele = null;
            SkinningData skin = null;
            List<BlendShapeContent> bShapes = null;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create("Content/" + name + "Anims.ansk", settings))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "MeshContent":
                                mesh = XMLToMeshContent(reader);
                                break;

                            case "Skeleton":
                                skele = XMLToSkeleton(reader);
                                break;

                            case "Skin":
                                skin = XMLToSkinningData(reader);
                                break;

                            case "BlendShapeContent":
                                bShapes.Add(XMLToBlendShapeContent(reader));
                                break;
                        }
                    }
                }
            }

            return new ANSKModelContent(mesh.Verticies, mesh.VertexIndicies, mesh.Uvs, mesh.UvIndicies, mesh.Edges, mesh.Normals, skele, skin,bShapes,mesh.Materials);
        }

        static private MeshContent XMLToMeshContent(XmlReader reader)
        {
            List<Vector3> verts = new List<Vector3>();
            List<short> vertInd = new List<short>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> uvInd = new List<int>();
            List<int> edges = new List<int>();
            List<Vector3> normals = new List<Vector3>();
            MaterialContent mat = null;

            reader.Read();

            while (reader.Name != "MeshContent")
            {
                switch (reader.Name)
                {
                    case "Vert":
                        reader.Read();
                        verts.Add(XmlImporter.XmlToVector3(reader.Value));
                        reader.Read();
                        break;

                    case "VertIndex":
                        reader.Read();
                        vertInd.Add(short.Parse(reader.Value));
                        reader.Read();
                        break;

                    case "Uv":
                        reader.Read();
                        uvs.Add(XmlImporter.XmlToVector2(reader.Value));
                        reader.Read();
                        break;

                    case "UvIndex":
                        reader.Read();
                        uvInd.Add(int.Parse(reader.Value));
                        reader.Read();
                        break;

                    case "Edge":
                        reader.Read();
                        edges.Add(int.Parse(reader.Value));
                        reader.Read();
                        break;

                    case "Normal":
                        reader.Read();
                        normals.Add(XmlImporter.XmlToVector3(reader.Value));
                        reader.Read();
                        break;

                    case "Material":
                        reader.Read();
                        mat = XMLToMaterialContent(reader);
                        break;
                }

                reader.Read();
            }

            return new MeshContent(verts, vertInd, uvs, uvInd, edges, normals, mat);
        }

        private static MaterialContent XMLToMaterialContent(XmlReader reader)
        {
            List<Material> mats = new List<Material>();
            List<int> ind = new List<int>();

            reader.Read();

            while (reader.Name != "MaterialContent")
            {
                switch (reader.Name)
                {
                    case "Material":
                        reader.Read();
                        mats.Add(XMLToMaterial(reader));
                        break;

                    case "Index":
                        reader.Read();
                        ind.Add(int.Parse(reader.Value));
                        reader.Read();
                        break;
                }

            reader.Read();
            }

            return new MaterialContent(mats, ind);
        }

        private static Material XMLToMaterial(XmlReader reader)
        {
            string name = "";
            Vector3 ambCol = Vector3.Zero;
            float trans = -1;
            Vector3 diffCol = Vector3.Zero;
            float diffFact = -1;
            Vector3 diffLight = Vector3.Zero;
            Vector3 emmis = Vector3.Zero;
            float opac = -1;
            int shader = -1;
            Vector3 specCol = Vector3.Zero;
            Vector3 spec = Vector3.Zero;
            float shinyExp = -1;
            float shiny = -1;
            float reflFac = -1;
            float refl = -1;

            reader.Read();

            while (reader.Name != "Material")
            {
                switch (reader.Name)
                {
                    case "Name":
                        name = reader.GetAttribute("Name");
                        break;

                    case "AmbientColour":
                        ambCol = XmlToVector3(reader.GetAttribute("Colour"));
                        break;

                    case "Transparency":
                        trans = float.Parse(reader.GetAttribute("Transparency"));
                        break;

                    case "DiffuseColour":
                        diffCol = XmlToVector3(reader.GetAttribute("Colour"));
                        break;

                    case "DiffuseFactor":
                        diffFact = float.Parse(reader.GetAttribute("Factor"));
                        break;

                    case "DiffuseLight":
                        diffLight = XmlToVector3(reader.GetAttribute("Light"));
                        break;

                    case "Emissive":
                        emmis = XmlToVector3(reader.GetAttribute("Emissive"));
                        break;

                    case "Opacity":
                        opac = float.Parse(reader.GetAttribute("Opacity"));
                        break;

                    case "ShaderMode":
                        shader = int.Parse(reader.GetAttribute("Mode"));
                        break;

                    case "SpecularColour":
                        specCol = XmlToVector3(reader.GetAttribute("Colour"));
                        break;

                    case "Specular":
                        spec = XmlToVector3(reader.GetAttribute("Specular"));
                        break;

                    case "ShininessExponent":
                        shinyExp = float.Parse(reader.GetAttribute("Exponent"));
                        break;

                    case "Shininess":
                        shiny = float.Parse(reader.GetAttribute("Shininess"));
                        break;
                    
                    case "ReflectionFactor":
                        reflFac = float.Parse(reader.GetAttribute("Factor"));
                        break;

                    case "Reflectivity":
                        refl = float.Parse(reader.GetAttribute("Reflectivity"));
                        break;
                }

                reader.Read();
            }

            Material mat = null;

            if (shader == 0)
                mat = new LambertMaterial(ambCol, trans, diffCol, diffFact, diffLight, emmis, opac, shader, name);
            else if (shader == 1)
                mat = new BlinnMaterial(ambCol, trans, diffCol, diffFact, diffLight, emmis, opac, shader, specCol, spec, shinyExp, shiny, reflFac, refl, name);

            return mat;
        }

        private static Skeleton XMLToSkeleton(XmlReader reader)
        {
            Skeleton skele = new Skeleton();

            reader.Read();

            while (reader.Name != "Skeleton")
            {
                switch (reader.Name)
                {
                    case "Joint":
                        skele.AddJoint(XMLToJoint(reader));
                        break;
                }

                reader.Read();
            }

            return skele;
        }

        private static Joint XMLToJoint(XmlReader reader)
        {
            Joint j = null;
            string name = "";
            int id = -1;
            int parentId = -1;
            Matrix trans = Matrix.Identity;
            Matrix rot = Matrix.Identity;
            Matrix scale = Matrix.Identity;
            List<int> inds = new List<int>();
            List<float> weights = new List<float>();

            reader.Read();

            while (reader.Name != "Joint")
            {
                switch (reader.Name)
                {
                    case "BasicInfo":
                        reader.Read();
                        name = reader.GetAttribute("Name");
                        id = int.Parse(reader.GetAttribute("Id"));
                        parentId = int.Parse(reader.GetAttribute("ParentId"));
                        trans = XMLToMatrix(reader.GetAttribute("Translation"));
                        rot = XMLToMatrix(reader.GetAttribute("Rotation"));
                        scale = XMLToMatrix(reader.GetAttribute("Scale"));
                        break;

                    case "Indice":
                        reader.Read();
                        inds.Add(int.Parse(reader.ReadElementContentAsString()));
                        break;

                    case "Weight":
                        reader.Read();
                        weights.Add(float.Parse(reader.ReadElementContentAsString()));
                        break;
                }

                reader.Read();
            }

            return new Joint(name, id, parentId, trans.Translation, rot.Translation, scale.Translation, inds, weights);
        }

        private static SkinningData XMLToSkinningData(XmlReader reader)
        {
            Dictionary<string, AnimationClip> anims = new Dictionary<string,AnimationClip>();
            List<AnimationClip> animList = new List<AnimationClip>();
            List<Matrix> bPose = null;
            List<Matrix> iBPose = null;
            List<BoneData> bData = null;

            reader.Read();

            while (reader.Name != "Skin")
            {
                switch (reader.Name)
                {
                    case "AnimationClip":
                        reader.Read();
                        animList.Add(XMLToAnimationClipXML(reader));
                        break;

                    case "BindPose":
                        reader.Read();
                        bPose = XMLToBindPose(reader);
                        break;

                    case "InverseBindPose":
                        reader.Read();
                        iBPose = XMLToInverseBindPose(reader);
                        break;

                    case "SkeletonHierarchy":
                        reader.Read();
                        bData = XMLToSkeletonHierarchy(reader);
                        break;
                }

                reader.Read();
            }

            for (int i = 0; i < animList.Count; i++)
                anims.Add(animList[i].Name, animList[i]);

            return new SkinningData(anims, bPose, iBPose, bData, new List<BoneData>());
        }

        static private AnimationClip XMLToAnimationClipXML(XmlReader reader)
        {
            string name = "";
            TimeSpan timespan;
            List<Keyframe> frames = new List<Keyframe>();

            reader.Read();
            while (reader.Name != "AnimationClip")
            {
                switch (reader.Name)
                {
                    case "Name":
                        reader.Read();
                        name = reader.ReadContentAsString();
                        break;

                    case "Timespan":
                        reader.Read();
                        timespan = new TimeSpan(reader.ReadContentAsLong());
                        break;

                    case "Keyframes":
                        reader.Read();
                        frames = XMLToKeyframesXML(reader);
                        break;
                }

                reader.Read();
            }

            return new AnimationClip(timespan, frames, name);
        }

        static private List<Keyframe> XMLToKeyframesXML(XmlReader reader)
        {
            List<Keyframe> keyframes = new List<Keyframe>();
            int curBone = 0;
            TimeSpan curTime;
            Matrix curTrans = Matrix.Identity;

            while (reader.Name != "Keyframes")
            {
                switch (reader.Name)
                {
                    case "Keyframe":
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            curBone = 0;
                            curTime = TimeSpan.Zero;
                            curTrans = Matrix.Identity;
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            keyframes.Add(new Keyframe(curBone, curTime, curTrans));
                        }
                        break;

                    case "Bone":
                        reader.Read();
                        curBone = reader.ReadContentAsInt();
                        break;

                    case "Time":
                        reader.Read();
                        curTime = new TimeSpan(reader.ReadContentAsLong());
                        break;

                    case "Transform":
                        reader.Read();
                        curTrans = XMLToMatrix(reader.ReadContentAsString());
                        break;
                }

                reader.Read();
            }

            return keyframes;
        }

        static private List<Matrix> XMLToBindPose(XmlReader reader)
        {
            List<Matrix> bones = new List<Matrix>();

            reader.Read();
            while (reader.Name != "BindPose")
            {
                switch (reader.Name)
                {
                    case "Bone":
                        reader.Read();
                        bones.Add(XMLToMatrix(reader.ReadContentAsString()));
                        break;
                }

                reader.Read();
            }

            return bones;
        }

        static private List<Matrix> XMLToInverseBindPose(XmlReader reader)
        {
            List<Matrix> bones = new List<Matrix>();

            reader.Read();
            while (reader.Name != "InverseBindPose")
            {
                switch (reader.Name)
                {
                    case "Bone":
                        reader.Read();
                        bones.Add(XMLToMatrix(reader.ReadContentAsString()));
                        break;
                }

                reader.Read();
            }

            return bones;
        }

        static private List<BoneData> XMLToSkeletonHierarchy(XmlReader reader)
        {
            List<BoneData> bones = new List<BoneData>();

            reader.Read();

            while (reader.Name != "SkeletonHierarchy")
            {
                switch (reader.Name)
                {
                    case "BoneData":
                        bones.Add(XMLToBoneData(reader));
                        break;
                }

                reader.Read();
            }

            return bones;
        }

        static private BoneData XMLToBoneData(XmlReader reader)
        {
            int numRef = 0;
            BoneData.SpecialBoneType? type = null;


            reader.Read();

            while (reader.Name != "BoneData")
            {
                switch (reader.Name)
                {
                    case "Ref":
                        reader.Read();
                        numRef = reader.ReadContentAsInt();
                        break;

                    case "Type":
                        if (reader.IsEmptyElement)
                            break;
                        reader.Read();
                        string temp = reader.ReadContentAsString();
                        if (temp == "Head")
                            type = BoneData.SpecialBoneType.Head;
                        break;
                }

                reader.Read();
            }

            return new BoneData(numRef, (BoneData.SpecialBoneType)type);
        }

        private static BlendShapeContent XMLToBlendShapeContent(XmlReader reader)
        {
            BlendShapeContent bShapes = null;
            List<Vector3> verts = new List<Vector3>();
            List<int> ind = new List<int>();
            List<Vector3> normals = new List<Vector3>();
            string name = "";
            bool keyed = false;

            reader.Read();

            while (reader.Name != "BlendShapeContent")
            {
                switch (reader.Name)
                {
                    case "Vertex":
                        reader.Read();
                        verts.Add(XmlToVector3(reader.ReadElementContentAsString()));
                        break;

                    case "Index":
                        reader.Read();
                        ind.Add(int.Parse(reader.ReadElementContentAsString()));
                        break;

                    case "Normal":
                        reader.Read();
                        normals.Add(XmlToVector3(reader.ReadElementContentAsString()));
                        break;

                    case "Name":
                        reader.Read();
                        name = reader.ReadElementContentAsString();
                        break;

                    case "Keyed":
                        reader.Read();
                        keyed = bool.Parse(reader.ReadElementContentAsString());
                        break;
                }

                reader.Read();
            }

            return new BlendShapeContent(verts, ind, normals, name, keyed);
        }

        public static Vector3 XmlToVector3(string data)
        {
            Vector3 temp;
            string[] split = data.Split(',');
            
            temp = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));

            return temp;
        }

        public static Vector2 XmlToVector2(string data)
        {
            Vector2 temp;
            string[] split = data.Split(',');

            temp = new Vector2(float.Parse(split[0]), float.Parse(split[1]));

            return temp;
        }

        public static Matrix XMLToMatrix(string entity)
        {
            string[] temp = entity.Split(',');

            return new Matrix(float.Parse(temp[0]),
                float.Parse(temp[1]),
                float.Parse(temp[2]),
                float.Parse(temp[3]),
                float.Parse(temp[4]),
                float.Parse(temp[5]),
                float.Parse(temp[6]),
                float.Parse(temp[7]),
                float.Parse(temp[8]),
                float.Parse(temp[9]),
                float.Parse(temp[10]),
                float.Parse(temp[11]),
                float.Parse(temp[12]),
                float.Parse(temp[13]),
                float.Parse(temp[14]),
                float.Parse(temp[15]));
        }
    }
}
