using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ANSKContentPipeline
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

        public List<int> PolygonVertexIndex { get { return _polyVertIndex; } set { _polyVertIndex = value; } }
        public List<int> Edges { get { return _edges; } set { _edges = value; } }
        public List<Vector2> Uvs { get { return _uvs; } set { _uvs = value; } }
        public List<int> UvIndex { get { return _uvIndex; } set { _uvIndex = value; } }

        public MeshPropertyType()
        {
            _polyVertIndex = new List<int>();
            _edges = new List<int>();
            _uvs = new List<Vector2>();
            _uvIndex = new List<int>();
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
}
