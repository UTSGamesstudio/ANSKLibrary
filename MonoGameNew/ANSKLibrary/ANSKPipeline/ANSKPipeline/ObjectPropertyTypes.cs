using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ANSKPipeline
{
    public interface IObjectPropertyType
    {
        Type GetType();
    }

    public class MeshPropertyType : IObjectPropertyType
    {
        private List<int> _polyVertIndex;
        private List<int> _edges;
        private List<Vector2> _uvs;
        private List<int> _uvIndex;
        private List<int> _materialIndex;

        public List<int> PolygonVertexIndex { get { return _polyVertIndex; } set { _polyVertIndex = value; } }
        public List<int> Edges { get { return _edges; } set { _edges = value; } }
        public List<Vector2> Uvs { get { return _uvs; } set { _uvs = value; } }
        public List<int> UvIndex { get { return _uvIndex; } set { _uvIndex = value; } }
        public List<int> MaterialIndex { get { return _materialIndex; } set { _materialIndex = value; } }

        public MeshPropertyType()
        {
            _polyVertIndex = new List<int>();
            _edges = new List<int>();
            _uvs = new List<Vector2>();
            _uvIndex = new List<int>();
            _materialIndex = new List<int>();
        }

        public Type GetType()
        {
            return typeof(MeshPropertyType);
        }
    }

    public class BlendShapePropertyType : IObjectPropertyType
    {
        // There is no information specific to Blend Shapes that we need to collect from
        // the fbx file. All the data can be stored in the default ObjectProperty.
        // Need this class so we can refer to the ObjectProperty as a BlendShapePropertyType.

        private bool _keyed;

        public bool Keyworded { get { return _keyed; } set { _keyed = value; } }

        public BlendShapePropertyType() { _keyed = false; }

        public Type GetType()
        {
            return typeof(BlendShapePropertyType);
        }
    }

    public class JointPropertyType : IObjectPropertyType
    {
        private List<float> _weights;

        public List<float> Weights { get { return _weights; } set { _weights = value; } }

        public JointPropertyType() { _weights = new List<float>(); }

        public Type GetType() { return typeof(JointPropertyType); }
    }

    public class LimbNodeNodeAttributeType : IObjectPropertyType
    {
        private bool _isSkeletonType;

        public bool IsSkeletonType { get { return _isSkeletonType; } set { _isSkeletonType = value; } }

        public LimbNodeNodeAttributeType() { _isSkeletonType = false; }

        public Type GetType() { return typeof(LimbNodeNodeAttributeType); }
    }

    public class LimbNodeJointType : IObjectPropertyType, IObjectProperties
    {
        private Vector3 _translation;
        private Vector3 _rotation;
        private Vector3 _scale;

        public Vector3 Translation { get { return _translation; } set { _translation = value; } }
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value; } }
        public Vector3 Scale { get { return _scale; } set { _scale = value; } }

        public LimbNodeJointType() { _translation = Vector3.Zero; _rotation = Vector3.Zero; _scale = Vector3.Zero; }

        public Type GetType() { return typeof(LimbNodeJointType); }
    }

    public class MaterialLambertPropertyType : IObjectPropertyType
    {
        public MaterialLambertPropertyType() { }

        public Type GetType() { return typeof(MaterialLambertPropertyType); }
    }

    public class MaterialBlinnPropertyType : IObjectPropertyType
    {
        private Vector3 _specColour;
        private Vector3 _specular;
        private double _shininessExp;
        private double _shininess;
        private double _reflectionFactor;
        private double _reflectivity;

        public Vector3 SpecularColour { get { return _specColour; } set { _specColour = value; } }
        public Vector3 Specular { get { return _specular; } set { _specular = value; } }
        public double Shininess { get { return _shininess; } set { _shininess = value; } }
        public double ShininessExponent { get { return _shininessExp; } set { _shininessExp = value; } }
        public double ReflectionFactor { get { return _reflectionFactor; } set { _reflectionFactor = value; } }
        public double Reflectivity { get { return _reflectivity; } set { _reflectivity = value; } }

        public MaterialBlinnPropertyType() { }

        public Type GetType() { return typeof(MaterialBlinnPropertyType); }
    }
}
