using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ANSKPipelineLibrary
{
    public class BlinnMaterial : Material
    {
        [ContentSerializer]
        public Color SpecularColour { get; set; }
        [ContentSerializer]
        public Vector3 Specular { get; set; }
        [ContentSerializer]
        public double ShininessExponent { get; set; }
        [ContentSerializer]
        public double Shininess { get; set; }
        [ContentSerializer]
        public double ReflectionFactor { get; set; }
        [ContentSerializer]
        public double Reflectivity { get; set; }

        public BlinnMaterial(Vector3 ambColour, double trans, Vector3 difColour, double difFactor, Vector3 diffLight, Vector3 emissive, double opacity, int shaderMode, Vector3 specColor, Vector3 specular, double shinExp, double shin, double refFac, double reflec, string name)
            : base(ambColour, trans, difColour, difFactor, diffLight, emissive, opacity, shaderMode, name)
        {
            SpecularColour = new Color(specColor.X, specColor.Y, specColor.Z, (float)opacity);
            Specular = specular;
            ShininessExponent = shinExp;
            Shininess = shin;
            ReflectionFactor = refFac;
            Reflectivity = reflec;
        }

        public BlinnMaterial() : base() { }

        public override void ExportToXML(System.Xml.XmlWriter writer)
        {
            base.ExportToXML(writer);

            writer.WriteStartElement("SpecularColour");
            writer.WriteAttributeString("Colour", XmlExporter.Vector3ToEntity(SpecularColour.ToVector3()));
            writer.WriteEndElement();

            writer.WriteStartElement("Specular");
            writer.WriteAttributeString("Specular", XmlExporter.Vector3ToEntity(Specular));
            writer.WriteEndElement();

            writer.WriteStartElement("ShininessExponent");
            writer.WriteAttributeString("Exponent", ShininessExponent.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Shininess");
            writer.WriteAttributeString("Shininess", Shininess.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("ReflectionFactor");
            writer.WriteAttributeString("Factor", ReflectionFactor.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Reflectivity");
            writer.WriteAttributeString("Reflectivity", Reflectivity.ToString());
            writer.WriteEndElement();
        }
    }
}
