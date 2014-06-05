using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ANSKPipelineLibrary
{
    public class Material
    {
        public enum Shader { Lambert = 0, Phong }
        public const string NameBlinn = "blinn";
        public const string NameLambert = "lambert";

        [ContentSerializer]
        public string Name { get; set; }
        [ContentSerializer]
        public Color AmbientColour { get; set; }
        [ContentSerializer]
        public double Transparency { get; set; }
        [ContentSerializer]
        public Color DiffuseColour { get; set; }
        [ContentSerializer]
        public double DiffuseFactor { get; set; }
        [ContentSerializer]
        public Vector3 DiffuseLight { get; set; }
        [ContentSerializer]
        public Vector3 Emissive { get; set; }
        [ContentSerializer]
        public double Opacity { get; set; }
        [ContentSerializer]
        public Shader ShaderMode { get; set; }

        public Material() { }

        public Material(Vector3 ambColour, double trans, Vector3 difColour, double difFactor, Vector3 diffuseLight, Vector3 emissive, double opacity, int shaderMode, string name)
        {
            AmbientColour = new Color(ambColour.X, ambColour.Y, ambColour.Z, (float)opacity);
            Transparency = trans;
            DiffuseColour = new Color(difColour.X, difColour.Y, difColour.Z, (float)opacity);
            DiffuseFactor = difFactor;
            DiffuseLight = diffuseLight;
            Emissive = emissive;
            Opacity = opacity;
            ShaderMode = (Shader)shaderMode;
            Name = name;
        }

        public virtual void ExportToXML(XmlWriter writer)
        {
            writer.WriteStartElement("Name");
            writer.WriteAttributeString("Name", Name);
            writer.WriteEndElement();

            writer.WriteStartElement("AmbientColour");
            writer.WriteAttributeString("Colour", XmlExporter.Vector3ToEntity(AmbientColour.ToVector3()));
            writer.WriteEndElement();

            writer.WriteStartElement("Transparency");
            writer.WriteAttributeString("Transparency", Transparency.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("DiffuseColour");
            writer.WriteAttributeString("Colour", XmlExporter.Vector3ToEntity(DiffuseColour.ToVector3()));
            writer.WriteEndElement();

            writer.WriteStartElement("DiffuseFactor");
            writer.WriteAttributeString("Factor", DiffuseFactor.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("DiffuseLight");
            writer.WriteAttributeString("Light", XmlExporter.Vector3ToEntity(DiffuseLight));
            writer.WriteEndElement();

            writer.WriteStartElement("Emissive");
            writer.WriteAttributeString("Emissive", XmlExporter.Vector3ToEntity(Emissive));
            writer.WriteEndElement();

            writer.WriteStartElement("Opacity");
            writer.WriteAttributeString("Opacity", Opacity.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("ShaderMode");
            writer.WriteAttributeString("Mode", XmlExporter.EnumToEntity(ShaderMode));
            writer.WriteEndElement();
        }
    }
}
