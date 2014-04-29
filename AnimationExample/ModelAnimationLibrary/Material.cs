using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ModelAnimationLibrary
{
    public class Material
    {
        public enum Shader { Lambert = 0, Phong }

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
    }
}
