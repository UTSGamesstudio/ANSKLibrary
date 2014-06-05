using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ANSKLibrary
{
    public struct ANSKVertexDeclarationModel : IVertexType
    {
        public Vector3 Position;
        public Vector4 Colour;
        public Vector2 Uv;
        public Vector3 Normal;

        public static readonly int SizeInBytes = (sizeof(float) * (3 + 4 + 2 + 3));

        public ANSKVertexDeclarationModel(Vector3 pos, Vector4 colour, Vector2 uv, Vector3 normal)
        {
            Position = pos;
            Colour = colour;
            Uv = uv;
            Normal = normal;
        }

         VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return MyVertexDeclaration; }
        }

        public readonly static VertexDeclaration MyVertexDeclaration = new VertexDeclaration
        (
            new VertexElement[]
            {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position,0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 7, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float) * 9, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
            }
        );
    }
}
