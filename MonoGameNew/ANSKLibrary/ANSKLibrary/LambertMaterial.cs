using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ANSKLibrary
{
    public class LambertMaterial : Material
    {
        public LambertMaterial(Vector3 ambColour, double trans, Vector3 difColour, double difFactor, Vector3 diffLight, Vector3 emissive, double opacity, int shaderMode, string name)
            : base(ambColour, trans, difColour, difFactor, diffLight, emissive, opacity, shaderMode, name)
        {
        }

        public LambertMaterial() : base() { }
    }
}
