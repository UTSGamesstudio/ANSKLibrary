using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModelAnimationLibrary
{
    //[StructLayout(LayoutKind.Explicit, Pack = 64)] 
    public struct ANSKVertexDeclaration : IVertexType
    {
        //[FieldOffset(0)]
        public Vector3 Position;
        //public Vector4 Color;
        //[FieldOffset(12)]
        public Vector2 Uv;
        //[FieldOffset(20)]
        public Vector3 Normal;
        public int4 Indices;
        public float4 Weights;
        public int BoneCount;

        //public static readonly VertexDeclaration VertexDeclaration;

        public static readonly int SizeInBytes = (sizeof(float) * (3 + 2 + 3 + 4)) + (sizeof(int) * 5);
        //public static readonly int SizeInBytes = (sizeof(float) * (3 + 4));

        public ANSKVertexDeclaration(Vector3 pos, Vector2 uv, Vector3 normal, int4 indices, float4 weights, int boneCount)
        {
            Position = pos;
            Uv = uv;
            Normal = normal;
            Indices = indices;
            Weights = weights;
            BoneCount = boneCount;
        }

        /*public ANSKVertexDeclaration(Vector3 pos, Color color)
        {
            Position = pos;
            Color = color.ToVector4();
        }*/

        /*public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position,0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
        };*/

         VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return MyVertexDeclaration; }
        }

        public readonly static VertexDeclaration MyVertexDeclaration = new VertexDeclaration
        (
            new VertexElement[]
            {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position,0),
            //new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector4, VertexElementUsage.Color, 0)
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float) * 5, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(sizeof(float) * 8, VertexElementFormat.Short4, VertexElementUsage.BlendIndices, 0),
                new VertexElement(sizeof(float) * 8 + sizeof(int) * 4, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
                new VertexElement(sizeof(float) * 12 + sizeof(int) * 4, VertexElementFormat.Single, VertexElementUsage.BlendIndices, 1)
            }
        );

        /*VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        static ANSKVertexDeclaration()
        {
            VertexElement[] elements = new VertexElement[]
            {*/
                /*new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position,0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(20, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(36, VertexElementFormat.Short4, VertexElementUsage.BlendIndices, 0),
                new VertexElement(40, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0)*/
                /*new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position,0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float) * 5, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(sizeof(float) * 8, VertexElementFormat.Short4, VertexElementUsage.BlendIndices, 0),
                new VertexElement(sizeof(float) * 8 + sizeof(int) * 4, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
                new VertexElement(sizeof(float) * 12 + sizeof(int) * 4, VertexElementFormat.Single, VertexElementUsage.BlendIndices, 1)*/
                /*new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position,0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
            };
            VertexDeclaration = new VertexDeclaration(elements);
        }*/
    }
}
