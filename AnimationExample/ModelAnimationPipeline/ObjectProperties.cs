using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ModelAnimationPipeline
{
    #region interfaces
    public interface IObjectProperty
    {
        int ID { get; set; }
        string Name { get; set; }

        Type GetType();
    }

    public interface IObjectProperties
    {
        Vector3 Translation { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Scale { get; set; }
    }
    #endregion

    #region ObjectProperties
    public class ObjectProperty : IObjectProperty
    {
        protected int _id;
        protected string _name;

        public int ID { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }

        public ObjectProperty()
        {
            _id = -1;
            _name = "";
        }

        public ObjectProperty(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }

    public class ObjectProperty<T> :IObjectProperty where T : IObjectPropertyType, new()
    {
        protected int _id;
        protected string _name;
        protected T _subClass;

        public int ID { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public T PropertyType { get { return _subClass; } set { _subClass = value; } }

        public ObjectProperty()
        {
            _id = -1;
            _name = "";
            _subClass = new T();
        }

        public ObjectProperty(int id, string name)
        {
            _id = id;
            _name = name;
            _subClass = new T();
        }
    }
    #endregion

    #region GeometryProperties
    public class GeometryProperty<T> :ObjectProperty<T> where T :IObjectPropertyType, new()
    {
        private List<Vector3> _verticies;
        private List<int> _indicies;
        private List<Vector3> _normals;

        public List<Vector3> Verticies { get { return _verticies; } set { _verticies = value; } }
        public List<int> Indicies { get { return _indicies; } set { _indicies = value; } }
        public List<Vector3> Normals { get { return _normals; } set { _normals = value; } }

        public GeometryProperty()
            : base()
        {
            _verticies = new List<Vector3>();
            _indicies = new List<int>();
            _normals = new List<Vector3>();
        }

        public GeometryProperty(int id, string name)
            : base(id, name)
        {
            _verticies = new List<Vector3>();
            _indicies = new List<int>();
            _normals = new List<Vector3>();
        }

        public Type GetType()
        {
            return typeof(GeometryProperty<T>);
        }
    }

    public class GeometryProperty : ObjectProperty
    {
        private List<Vector3> _verticies;
        private List<Vector3> _indicies;
        private List<Vector3> _normals;

        public List<Vector3> Verticies { get { return _verticies; } set { _verticies = value; } }
        public List<Vector3> Indicies { get { return _indicies; } set { _indicies = value; } }
        public List<Vector3> Normals { get { return _normals; } set { _normals = value; } }

        public GeometryProperty()
            : base()
        {
            _verticies = new List<Vector3>();
            _indicies = new List<Vector3>();
            _normals = new List<Vector3>();
        }

        public GeometryProperty(int id, string name)
            : base(id, name)
        {
            _verticies = new List<Vector3>();
            _indicies = new List<Vector3>();
            _normals = new List<Vector3>();
        }

        public Type GetType()
        {
            return typeof(GeometryProperty);
        }
    }
    #endregion

    #region DeformerPrperty
    public class DeformerProperty : ObjectProperty
    {
        private List<int> _indicies;

        public List<int> Indicies { get { return _indicies; } set { _indicies = value; } }

        public DeformerProperty()
            : base()
        {
            _indicies = new List<int>();
        }

        public DeformerProperty(int id, string name)
            : base(id, name)
        {
            _indicies = new List<int>();
        }

        public Type GetType()
        {
            return typeof(DeformerProperty);
        }
    }

    public class DeformerProperty<T> : ObjectProperty<T> where T : IObjectPropertyType, new()
    {
        private List<int> _indicies;

        public List<int> Indicies { get { return _indicies; } set { _indicies = value; } }

        public DeformerProperty()
            : base()
        {
            _indicies = new List<int>();
        }

        public DeformerProperty(int id, string name)
            : base(id, name)
        {
            _indicies = new List<int>();
        }

        public Type GetType()
        {
            return typeof(DeformerProperty);
        }
    }
    #endregion

    #region LimbNodeProperty
    public class LimbNodeProperty : ObjectProperty
    {
        public LimbNodeProperty(int id, string name)
            : base(id, name) { }

        public Type GetType() { return typeof(LimbNodeProperty); }
    }

    public class LimbNodeProperty<T> : ObjectProperty<T> where T : IObjectPropertyType, new()
    {
        public LimbNodeProperty()
            : base() { }

        public LimbNodeProperty(int id, string name)
            : base(id, name) { }

        public Type GetType() { return typeof(LimbNodeProperty<T>); }
    }
    #endregion

    #region MaterialProperty
    public class MaterialProperty : ObjectProperty
    {
        public enum Shader { Lambert = 0, Phong }

        private Vector3 _colour;
        private double _transparency;
        private Vector3 _ambColour;
        private double _diffuseFactor;
        private Vector3 _diffuseColour;
        private Vector3 _emissive;
        private double _opacity;
        private Shader _shader;

        public Vector3 Colour { get { return _colour; } set { _colour = value; } }
        public double Transparency { get { return _transparency; } set { _transparency = value; } }
        public Vector3 AmbientColour { get { return _ambColour; } set { _ambColour = value; } }
        public Vector3 DiffuseColour { get { return _diffuseColour; } set { _diffuseColour = value; } }
        public double DiffuseFactor { get { return _diffuseFactor; } set { _diffuseFactor = value; } }
        public Vector3 Emissive { get { return _emissive; } set { _emissive = value; } }
        public double Opacity { get { return _opacity; } set { _opacity = value; } }
        public Shader ShaderModel { get { return _shader; } set { _shader = value ; } }

        public MaterialProperty(int id, string name) : base(id, name) { }

        public Type GetType() { return typeof(MaterialProperty); }
    }

    public class MaterialProperty<T> : ObjectProperty<T> where T : IObjectPropertyType, new()
    {
        public enum Shader { Lambert = 0, Phong }

        private Vector3 _colour;
        private double _transparency;
        private Vector3 _ambColour;
        private double _diffuseFactor;
        private Vector3 _diffuseColour;
        private Vector3 _diffLight;
        private Vector3 _emissive;
        private double _opacity;
        private Shader _shader;

        public Vector3 Colour { get { return _colour; } set { _colour = value; } }
        public double Transparency { get { return _transparency; } set { _transparency = value; } }
        public Vector3 AmbientColour { get { return _ambColour; } set { _ambColour = value; } }
        public Vector3 DiffuseColour { get { return _diffuseColour; } set { _diffuseColour = value; } }
        public double DiffuseFactor { get { return _diffuseFactor; } set { _diffuseFactor = value; } }
        public Vector3 DiffuseLight { get { return _diffLight; } set { _diffLight = value; } }
        public Vector3 Emissive { get { return _emissive; } set { _emissive = value; } }
        public double Opacity { get { return _opacity; } set { _opacity = value; } }
        public Shader ShaderModel { get { return _shader; } set { _shader = value; } }

        public MaterialProperty(int id, string name) : base(id, name) { }

        public Type GetType() { return typeof(MaterialProperty<T>); }
    }
#endregion
}
