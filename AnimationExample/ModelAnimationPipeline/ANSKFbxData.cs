using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using ModelAnimationLibrary;

namespace ModelAnimationPipeline
{
    public class ANSKFbxData
    {
        private List<IObjectProperty> _properties;
        private List<ObjectConnection> _connections;

        public List<IObjectProperty> Properties { get { return _properties; } }
        public List<ObjectConnection> Connections { get { return _connections; } }

        public ANSKFbxData()
        {
            _properties = new List<IObjectProperty>();
            _connections = new List<ObjectConnection>();
        }

        public List<BlendShapeContent> CollectBlendShapes(bool keyworded)
        {
            List<BlendShapeContent> shapes = new List<BlendShapeContent>();

            for (int i = 0; i < _properties.Count; i++)
            {
                if (_properties[i].GetType() == typeof(GeometryProperty<BlendShapePropertyType>))
                {
                    GeometryProperty<BlendShapePropertyType> geo = _properties[i] as GeometryProperty<BlendShapePropertyType>;
                    if (!keyworded || keyworded && geo.PropertyType.Keyworded)
                        shapes.Add(new BlendShapeContent(geo.Verticies, geo.Indicies, geo.Normals, geo.Name, geo.PropertyType.Keyworded));
                }
            }

            return shapes;
        }

        public Skeleton CollectSkeleton()
        {
            //System.Diagnostics.Debugger.Launch();
            Skeleton skele = new Skeleton();

            List<Joint> joints = new List<Joint>();
            List<LimbNodeProperty<LimbNodeNodeAttributeType>> attrType = new List<LimbNodeProperty<LimbNodeNodeAttributeType>>();
            List<LimbNodeProperty<LimbNodeJointType>> jointTypes = new List<LimbNodeProperty<LimbNodeJointType>>();
            List<DeformerProperty<JointPropertyType>> deformTypes = new List<DeformerProperty<JointPropertyType>>();

            for (int i = 0; i < _properties.Count; i++)
            {
                IObjectProperty type = _properties[i];

                if (type.GetType() == typeof(LimbNodeProperty<LimbNodeJointType>))
                    jointTypes.Add((LimbNodeProperty<LimbNodeJointType>)type);
                else if (type.GetType() == typeof(LimbNodeProperty<LimbNodeNodeAttributeType>))
                    attrType.Add((LimbNodeProperty<LimbNodeNodeAttributeType>)type);
                else if (type.GetType() == typeof(DeformerProperty<JointPropertyType>))
                    deformTypes.Add((DeformerProperty<JointPropertyType>)type);
            }

            for (int i = 0; i < attrType.Count; i++)
            {
                for (int q = 0; q < jointTypes.Count; q++)
                {
                    ObjectConnection con = _connections.Find(obj => (attrType[i].ID == obj.Id1) && (jointTypes[q].ID == obj.Id2));

                    if (con != null)
                    {
                        if (!attrType[i].PropertyType.IsSkeletonType)
                            break;

                        for (int w = 0; w < deformTypes.Count; w++)
                        {
                            ObjectConnection con2 = _connections.Find(obj => (jointTypes[q].ID == obj.Id1) && (deformTypes[w].ID == obj.Id2));
                            
                            if (con2 != null)
                            {
                                //System.Diagnostics.Debugger.Launch();
                                int parentId = FindParentOfJoint(jointTypes[q].ID, jointTypes);
                                skele.AddJoint(new Joint(jointTypes[q].Name, jointTypes[q].ID, parentId, jointTypes[q].PropertyType.Translation,
                                    jointTypes[q].PropertyType.Rotation, jointTypes[q].PropertyType.Scale, deformTypes[w].Indicies, deformTypes[w].PropertyType.Weights));
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            skele.FinaliseJointTree();

            return skele;
        }

        private int FindParentOfJoint(int childId, List<LimbNodeProperty<LimbNodeJointType>> joints)
        {
            for (int i = 0; i < _connections.Count; i++)
            {
                for (int q = 0; q < joints.Count; q++)
                {
                    if (_connections[i].Id1 == childId && _connections[i].Id2 == joints[q].ID)
                        return _connections[i].Id2;
                }
            }

            // The reason why we do this is because this child is actually the root joint,
            // and there is no ObjectConnection comparing to joints that gives us the joint.
            return childId;
        }

        public List<Material> CollectMaterials()
        {
            List<Material> mats = new List<Material>();

            for (int i = 0; i < _properties.Count; i++)
            {
                if (_properties[i].GetType() == typeof(MaterialProperty<MaterialLambertPropertyType>))
                {
                    MaterialProperty<MaterialLambertPropertyType> lamb = (MaterialProperty<MaterialLambertPropertyType>)_properties[i];

                    mats.Add(new LambertMaterial(lamb.AmbientColour, lamb.Transparency, lamb.DiffuseColour, lamb.DiffuseFactor, lamb.DiffuseLight, lamb.Emissive, lamb.Opacity, (int)lamb.ShaderModel, lamb.Name));
                }
                else if (_properties[i].GetType() == typeof(MaterialProperty<MaterialBlinnPropertyType>))
                {
                    MaterialProperty<MaterialBlinnPropertyType> blin = (MaterialProperty<MaterialBlinnPropertyType>)_properties[i];

                    mats.Add(new BlinnMaterial(blin.AmbientColour, blin.Transparency, blin.DiffuseColour, blin.DiffuseFactor, blin.DiffuseLight, blin.Emissive, blin.Opacity, (int)blin.ShaderModel, blin.PropertyType.SpecularColour, blin.PropertyType.Specular, blin.PropertyType.ShininessExponent, blin.PropertyType.Shininess, blin.PropertyType.ReflectionFactor, blin.PropertyType.Reflectivity, blin.Name));
                }
            }

            return mats;
        }

        public void LoadFbx(string filePath)
        {
            //System.Diagnostics.Debugger.Launch();

            using (StreamReader reader = File.OpenText(filePath))
            {
                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    //if (line.Contains(ANSKFbxFileKeywords.ObjectPropertiesHeading))
                        //ObjectPropertiesEnter(reader);
                    //if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyBlendShape))
                        //AnalyseGeometryProperty(reader, line);
                    if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyGeometry))
                        AnalyseGeometryProperty(reader, line);
                    else if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyConnections))
                        AnalyseObjectConnectionProperty(reader);
                }
            }
        }

        private void ObjectPropertiesEnter(StreamReader reader)
        {
            string line = "";

            while (!(line = reader.ReadLine()).Contains('}'))
            {
                if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyObjects))
                    ObjectsEnter(reader);
            }
        }

        private void ObjectsEnter(StreamReader reader)
        {
            string line = "";
            int indent = 1;

            while (!(line = reader.ReadLine()).Contains('}') && indent > 0)
            {
                if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyGeometry))
                    AnalyseGeometryProperty(reader, line);

                if (line.Contains('{'))
                    indent++;
                else if (line.Contains('}'))
                    indent--;
            }
        }

        private void AnalyseObjectConnectionProperty(StreamReader reader)
        {
            string line;
            string name1, name2;
            int id1, id2;

            while (!(line = reader.ReadLine()).Contains('}'))
            {
                if (line == "\t")
                    continue;

                ScanObjectConnection(reader, line, out name1, out name2, out id1, out id2);

                _connections.Add(new ObjectConnection( id1, name1, id2, name2));
            }
        }

        private void ScanObjectConnection(StreamReader reader, string line, out string name1, out string name2, out int id1, out int id2)
        {
            name1 = "";
            name2 = "";
            id1 = -1;
            id2 = -1;

            string[] splitLine;

            splitLine = line.Split(',');
            name1 = splitLine[0].Remove(0, 2);
            name2 = splitLine[1].Remove(0, 1);

            line = reader.ReadLine();

            splitLine = line.Split(',');
            id1 = Int32.Parse(splitLine[1]);
            id2 = Int32.Parse(splitLine[2]);
        }

        private void AnalyseGeometryProperty(StreamReader reader, string line)
        {
            int id;
            string name;
            string type;
            IObjectProperty data;

            while (line != "}")
            {
                AnalyseStandardObjectHeading(line, out id, out name, out type);

                if (type != "")
                {
                    switch (type)
                    {
                        case "Mesh":
                            if (name.Contains("Geometry::"))
                            {
                                data = new GeometryProperty<MeshPropertyType>(id, name);
                                AnalyseGeometryMesh(reader, ref data);
                                _properties.Add(data);
                            }
                            else
                                ProceedToNextObject(reader);
                            //ProceedToNextObject(reader);
                            // Continue for when we need mesh data. Not needed now.
                            break;

                        case "Shape":
                            data = new GeometryProperty<BlendShapePropertyType>(id, name);
                            if (name.Contains("ansk"))
                                ((GeometryProperty<BlendShapePropertyType>)data).PropertyType.Keyworded = true;
                            AnalyseGeometryBlendShape(reader, ref data);
                            _properties.Add(data);
                            break;

                        case "Cluster":
                            data = new DeformerProperty<JointPropertyType>(id, name);
                            AnalyseGeometryDeformer(reader, ref data);
                            _properties.Add(data);
                            break;

                        case "LimbNode":
                            if (name.Contains("NodeAttribute::"))
                            {
                                data = new LimbNodeProperty<LimbNodeNodeAttributeType>(id, name);
                                AnalyseGeometryLimbNode(reader, ref data);
                                _properties.Add(data);
                            }
                            else if (name.Contains("Model::"))
                            {
                                data = new LimbNodeProperty<LimbNodeJointType>(id, name);
                                AnalyseGeometryLimbNode(reader, ref data);
                                _properties.Add(data);
                            }
                            else
                                ProceedToNextObject(reader);
                            break;

                        default:
                            ProceedToNextObject(reader);
                            break;
                    }
                }
                else
                {
                    if (name.Contains(ANSKFbxFileKeywords.ObjectNameMaterial))
                    {
                        if (name.Contains(ANSKFbxFileKeywords.ObjectNameMaterialLambert))
                        {
                            data = new MaterialProperty<MaterialLambertPropertyType>(id, name);
                            AnalyseMaterialLambert(reader, ref data);
                            _properties.Add(data);
                        }
                        else if (name.Contains(ANSKFbxFileKeywords.ObjectNameMaterialBlinn))
                        {
                            data = new MaterialProperty<MaterialBlinnPropertyType>(id, name);
                            AnalyseMaterialBlinn(reader, ref data);
                            _properties.Add(data);
                        }
                        else if (name.Contains(ANSKFbxFileKeywords.ObjectNameMaterialDefault))
                        {
                            data = new MaterialProperty<MaterialLambertPropertyType>(id, name);
                            AnalyseMaterialLambert(reader, ref data);
                            _properties.Add(data);
                        }
                    }
                    else
                        ProceedToNextObject(reader);
                }

                line = reader.ReadLine();
            }
        }

        private void ProceedToNextObject(StreamReader reader)
        {
            string line = "";
            while ((line = reader.ReadLine()) != "\t}") { }
        }

        private void AnalyseGeometryMesh(StreamReader reader, ref IObjectProperty data)
        {
            string line = "";
            int indent = 1;

            while ((line = reader.ReadLine()) != "\t}")// && indent > 0)
            {
                if (line.Contains('{'))
                    indent++;
                else if (line.Contains('}'))
                    indent--;

                if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyVertices))
                {
                    ((GeometryProperty<MeshPropertyType>)data).Verticies.AddRange(CollectVector3List(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.MeshPropertyPolygonVertexIndex))
                {
                    ((GeometryProperty<MeshPropertyType>)data).PropertyType.PolygonVertexIndex.AddRange(CollectIntList(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyEdges))
                {
                    ((GeometryProperty<MeshPropertyType>)data).PropertyType.Edges.AddRange(CollectIntList(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyNormals))
                {
                    ((GeometryProperty<MeshPropertyType>)data).Normals.AddRange(CollectVector3List(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.MeshPropertyUV))
                {
                    ((GeometryProperty<MeshPropertyType>)data).PropertyType.Uvs.AddRange(CollectVector2List(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.MeshPropertyUVIndex))
                {
                    ((GeometryProperty<MeshPropertyType>)data).PropertyType.UvIndex.AddRange(CollectIntList(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyMaterials))
                {
                    ((GeometryProperty<MeshPropertyType>)data).PropertyType.MaterialIndex.AddRange(CollectIntList(reader, GrabElementNumber(line)));
                }
            }
        }

        private void AnalyseGeometryBlendShape(StreamReader reader, ref IObjectProperty data)
        {
            string line = "";
            int indent = 1;

            while (!(line = reader.ReadLine()).Contains('}') && indent > 0)
            {
                if (line.Contains('{'))
                    indent++;
                else if (line.Contains('}'))
                    indent--;

                if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyIndexes))
                {
                    ((GeometryProperty<BlendShapePropertyType>)data).Indicies.AddRange(CollectIntList(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyVertices))
                {
                    ((GeometryProperty<BlendShapePropertyType>)data).Verticies.AddRange(CollectVector3List(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyNormals))
                {
                    ((GeometryProperty<BlendShapePropertyType>)data).Normals.AddRange(CollectVector3List(reader, GrabElementNumber(line)));
                    indent--;
                }
            }
        }

        private void AnalyseGeometryDeformer(StreamReader reader, ref IObjectProperty data)
        {
            string line = "";
            int indent = 1;

            while ((line = reader.ReadLine()) != "\t}")
            {
                if (line.Contains('{'))
                    indent++;
                else if (line.Contains('}'))
                    indent--;

                if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyIndexes))
                {
                    ((DeformerProperty<JointPropertyType>)data).Indicies.AddRange(CollectIntList(reader, GrabElementNumber(line)));
                    indent--;
                }
                else if (line.Contains(ANSKFbxFileKeywords.ObjectPropertyWeights))
                {
                    ((DeformerProperty<JointPropertyType>)data).PropertyType.Weights.AddRange(CollectFloatList(reader, GrabElementNumber(line)));
                    indent--;
                }
            }
        }

        private void AnalyseMaterialLambert(StreamReader reader, ref IObjectProperty data)
        {
            string line = "";

            while ((line = reader.ReadLine()) != "\t}")
            {
                if (line.Contains(ANSKFbxFileKeywords.MaterialShadingModel))
                    ScanMaterialShadingModel(line, ref data);
                else if (line.Contains(ANSKFbxFileKeywords.MaterialPropertiesHeading))
                    ScanMaterialLambertProperties(reader, ref data);
            }
        }

        private void AnalyseMaterialBlinn(StreamReader reader, ref IObjectProperty data)
        {
            string line = "";

            while ((line = reader.ReadLine()) != "\t}")
            {
                if (line.Contains(ANSKFbxFileKeywords.MaterialShadingModel))
                    ScanMaterialShadingModel(line, ref data);
                else if (line.Contains(ANSKFbxFileKeywords.MaterialPropertiesHeading))
                    ScanMaterialBlinnProperties(reader, ref data);
            }
        }

        private void ScanMaterialLambertProperties(StreamReader reader, ref IObjectProperty data)
        {
            string line;
            string pName = "", pType = ""; string[] extra;

            while ((line = reader.ReadLine()).Contains(ANSKFbxFileKeywords.ObjectPropertyStart))
            {
                ScanPropertyLine(line, out pName, out pType, out extra);

                switch (pName)
                {
                    case ANSKFbxFileKeywords.MaterialPropertyAmbientColour:
                        ((MaterialProperty<MaterialLambertPropertyType>)data).AmbientColour = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyDiffuseColour:
                        ((MaterialProperty<MaterialLambertPropertyType>)data).DiffuseColour = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyDiffuseFactor:
                        ((MaterialProperty<MaterialLambertPropertyType>)data).DiffuseFactor = PropertyExtraToDouble(extra);
                        break;
                    case ANSKFbxFileKeywords.MateiralPropertyDiffuseLight:
                        ((MaterialProperty<MaterialLambertPropertyType>)data).DiffuseLight = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyTransparency:
                        ((MaterialProperty<MaterialLambertPropertyType>)data).Transparency = PropertyExtraToDouble(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyEmissive:
                        ((MaterialProperty<MaterialLambertPropertyType>)data).Emissive = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyOpacity:
                        ((MaterialProperty<MaterialLambertPropertyType>)data).Opacity = PropertyExtraToDouble(extra);
                        break;
                }
            }
        }

        private void ScanMaterialBlinnProperties(StreamReader reader, ref IObjectProperty data)
        {
            string line;
            string pName = "", pType = ""; string[] extra;

            while ((line = reader.ReadLine()).Contains(ANSKFbxFileKeywords.ObjectPropertyStart))
            {
                ScanPropertyLine(line, out pName, out pType, out extra);

                switch (pName)
                {
                    case ANSKFbxFileKeywords.MaterialPropertyAmbientColour:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).AmbientColour = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyDiffuseColour:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).DiffuseColour = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyDiffuseFactor:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).DiffuseFactor = PropertyExtraToDouble(extra);
                        break;
                    case ANSKFbxFileKeywords.MateiralPropertyDiffuseLight:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).DiffuseLight = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyTransparency:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).Transparency = PropertyExtraToDouble(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertySpecularColour:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).PropertyType.SpecularColour = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyShininessExponent:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).PropertyType.ShininessExponent = PropertyExtraToDouble(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyReflectionFactor:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).PropertyType.ReflectionFactor = PropertyExtraToDouble(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyEmissive:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).Emissive = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertySpecular:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).PropertyType.Specular = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyShiniess:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).PropertyType.Shininess = PropertyExtraToDouble(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyOpacity:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).Opacity = PropertyExtraToDouble(extra);
                        break;
                    case ANSKFbxFileKeywords.MaterialPropertyReflectivity:
                        ((MaterialProperty<MaterialBlinnPropertyType>)data).PropertyType.Reflectivity = PropertyExtraToDouble(extra);
                        break;
                }
            }
        }

        private void ScanMaterialShadingModel(string line, ref IObjectProperty data)
        {
            string[] split = line.Split('"');

            switch (split[1])
            {
                case "lambert":
                    ((MaterialProperty<MaterialLambertPropertyType>)data).ShaderModel = MaterialProperty<MaterialLambertPropertyType>.Shader.Lambert;
                    break;
                case "phong":
                    ((MaterialProperty<MaterialBlinnPropertyType>)data).ShaderModel = MaterialProperty<MaterialBlinnPropertyType>.Shader.Phong;
                    break;
            }
        }

        private void AnalyseGeometryLimbNode(StreamReader reader, ref IObjectProperty data)
        {
            string line = "";
            int indent = 1;

            while ((line = reader.ReadLine()) != "\t}")
            {
                if (line.Contains('{'))
                    indent++;
                else if (line.Contains('}'))
                    indent--;

                if (data.GetType() == typeof(LimbNodeProperty<LimbNodeNodeAttributeType>))
                {
                    if (line.Contains(ANSKFbxFileKeywords.PropertiesTypeFlagsHeading))
                        ScanTypeFlags(line, ref data);
                }
                else if (data.GetType() == typeof(LimbNodeProperty<LimbNodeJointType>))
                {
                    if (line.Contains(ANSKFbxFileKeywords.LimbNodePropertiesHeading))
                        ScanLimbNodeJointTypeProperties(reader, ref data);
                }
            }
        }

        private void ScanTypeFlags(string line, ref IObjectProperty data)
        {
            string[] types = line.Split('"');

            if (types[1] == "Skeleton")
                ((LimbNodeProperty<LimbNodeNodeAttributeType>)data).PropertyType.IsSkeletonType = true;
        }

        private void ScanLimbNodeJointTypeProperties(StreamReader reader, ref IObjectProperty data)
        {
            string line;
            string pName = "", pType = ""; string[] extra;

            while ((line = reader.ReadLine()).Contains(ANSKFbxFileKeywords.ObjectPropertyStart))
            {
                pName = ""; pType = "";

                ScanPropertyLine(line, out pName, out pType, out extra);

                switch (pName)
                {
                    case ANSKFbxFileKeywords.PropertiesLocalTranslation:
                        ((LimbNodeProperty<LimbNodeJointType>)data).PropertyType.Translation = PropertyExtraToVector3(extra);
                        break;
                    case ANSKFbxFileKeywords.PropertiesLocalRotation:
                        ((LimbNodeProperty<LimbNodeJointType>)data).PropertyType.Rotation = PropertyExtraToVector3(extra);
                        break;
                }
            }
        }

        private void ScanPropertyLine(string line, out string name, out string type, out string[] extra)
        {
            string[] dataExtract = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> tempHolder = new List<string>();

            name = ""; type = "";

            name = dataExtract[0].Split('"')[1];
            type = dataExtract[1].Split('"')[1];

            for (int i = 2; i < dataExtract.Length; i++)
            {
                if (!dataExtract[i].Contains("\""))
                    tempHolder.Add(dataExtract[i]);
            }

            extra = tempHolder.ToArray();
        }

        private void AnalyseStandardObjectHeading(string line, out int id, out string name, out string type)
        {
            id = 0;
            name = "";
            type = "";

            string[] firstPart;
            string idText = "";

            firstPart = line.Split(',');

            for (int i = firstPart[0].Length - 1; firstPart[0][i] != ' '; i--)
                idText += firstPart[0][i];

            string tempNum = "";

            for (int i = idText.Length - 1; i >= 0; i--)
            {
                tempNum += idText[i];
            }

            id = int.Parse(tempNum);

            name = firstPart[1].Split('"')[1];

            type = firstPart[2].Split('"')[1];
        }

        private void AnalyseLimbNodeProperties(StreamReader reader, ref IObjectProperty data)
        {
            string line = "";
        }

        private Vector3[] CollectVector3List(StreamReader reader, int amount)
        {
            string line = "";
            string[] splits;
            char[] splitChars = new char[2]; splitChars[0] = ':'; splitChars[1] = ',';
            string rawVecs = "";
            int vAmount = amount / 3;
            Vector3[] temp = new Vector3[vAmount];

            while (!(line = reader.ReadLine()).Contains('}'))
                rawVecs += line;

            splits = rawVecs.Split(splitChars);

            for (int i = 0; i < vAmount; i++)
            {
                temp[i] = new Vector3(float.Parse(splits[i * 3 + 1]), float.Parse(splits[i * 3 + 2]), float.Parse(splits[i * 3 + 3]));
            }

            return temp;
        }

        private Vector2[] CollectVector2List(StreamReader reader, int amount)
        {
            string line = "";
            string[] splits;
            char[] splitChars = new char[2]; splitChars[0] = ':'; splitChars[1] = ',';
            string rawVecs = "";
            int vAmount = amount / 2;
            Vector2[] temp = new Vector2[vAmount];

            while (!(line = reader.ReadLine()).Contains('}'))
                rawVecs += line;

            splits = rawVecs.Split(splitChars);

            for (int i = 0; i < vAmount; i++)
            {
                temp[i] = new Vector2(float.Parse(splits[i * 2 + 1]), float.Parse(splits[i * 2 + 2]));
            }

            return temp;
        }

        private int[] CollectIntList(StreamReader reader, int amount)
        {
            string line = "";
            string[] splits;
            char[] splitChars = new char[2]; splitChars[0] = ':'; splitChars[1] = ',';
            string rawVecs = "";
            int[] temp = new int[amount];

            while (!(line = reader.ReadLine()).Contains('}'))
                rawVecs += line;

            splits = rawVecs.Split(splitChars);

            for (int i = 0; i < amount; i++)
            {
                temp[i] = int.Parse(splits[i + 1]);
            }

            return temp;
        }

        private float[] CollectFloatList(StreamReader reader, int amount)
        {
            string line = "";
            string[] splits;
            char[] splitChars = new char[2]; splitChars[0] = ':'; splitChars[1] = ',';
            string rawVecs = "";
            float[] temp = new float[amount];

            while (!(line = reader.ReadLine()).Contains('}'))
                rawVecs += line;

            splits = rawVecs.Split(splitChars);

            for (int i = 0; i < amount; i++)
            {
                temp[i] = float.Parse(splits[i + 1]);
            }

            return temp;
        }

        private int GrabElementNumber(string line)
        {
            int amount = -1;
            char[] splitters = new char[3];
            splitters[0] = ':';
            splitters[1] = '*';
            splitters[1] = '{';

            string[] split;

            split = line.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

            split = split[1].Split('*');

            if (!int.TryParse(split[1], out amount))
                throw new Exception("Error parsing element number");

            return amount;
        }

        private Vector3 PropertyExtraToVector3(string[] extra)
        {
            return new Vector3(float.Parse(extra[0]), float.Parse(extra[1]), float.Parse(extra[2]));
        }

        private double PropertyExtraToDouble(string[] extra)
        {
            return double.Parse(extra[0]);
        }
    }

    public class ANSKFbxFileKeywords
    {
        public const string ObjectPropertiesHeading = "Object properties";
        public const string ObjectPropertyObjects = "Objects: ";
        public const string ObjectPropertyGeometry = "Geometry: ";
        public const string ObjectPropertyConnections = "Connections: ";
        public const string ObjectPropertyIndexes = "Indexes: ";
        public const string ObjectPropertyVertices = "Vertices: ";
        public const string ObjectPropertyNormals = "Normals: ";
        public const string ObjectPropertyWeights = "Weights: ";
        public const string ObjectPropertyBlendShape = "\"Shape\"";
        public const string ObjectPropertyMaterials = "Materials: ";
        public const string MeshPropertyPolygonVertexIndex = "PolygonVertexIndex: ";
        public const string ObjectPropertyEdges = "Edges: ";
        public const string MeshPropertyUV = "\tUV: ";
        public const string MeshPropertyUVIndex = "UVIndex: ";
        public const string LimbNodePropertiesHeading = "Properties";
        public const string ObjectPropertyStart = "P:";
        public const string PropertiesLocalTranslation = "Lcl Translation";
        public const string PropertiesLocalRotation = "Lcl Rotation";
        public const string PropertiesTypeFlagsHeading = "TypeFlags: ";
        public const string ObjectNameMaterial = "Material::";
        public const string ObjectNameMaterialLambert = "lambert";
        public const string ObjectNameMaterialBlinn = "blinn";
        public const string ObjectNameMaterialDefault = "Default_Material";
        public const string MaterialShadingModel = "ShadingModel: ";
        public const string MaterialPropertiesHeading = "Properties";
        public const string MaterialPropertyAmbientColour = "AmbientColor";
        public const string MaterialPropertyDiffuseColour = "DiffuseColor";
        public const string MaterialPropertyDiffuseFactor = "DiffuseFactor";
        public const string MateiralPropertyDiffuseLight = "Diffuse";
        public const string MaterialPropertyTransparency = "TransparencyFactor";
        public const string MaterialPropertyEmissive = "Emissive";
        public const string MaterialPropertyOpacity = "Opacity";
        public const string MaterialPropertySpecularColour = "SpecularColor";
        public const string MaterialPropertySpecular = "Specular";
        public const string MaterialPropertyShininessExponent = "ShininessExponent";
        public const string MaterialPropertyShiniess = "Shininess";
        public const string MaterialPropertyReflectionFactor = "ReflectionFactor";
        public const string MaterialPropertyReflectivity = "Reflectivity";
    }
}
