using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ANSKLibrary
{
    public class ANSKModelContent
    {
        [ContentSerializer]
        public List<Vector3> Verticies { get; set; }
        [ContentSerializer]
        public List<int> VertexIndicies { get; set; }
        [ContentSerializer]
        public List<Vector2> Uvs { get; set; }
        [ContentSerializer]
        public List<int> UvIndicies { get; set; }
        [ContentSerializer]
        public List<int> Edges { get; set; }
        [ContentSerializer]
        public List<Vector3> Normals { get; set; }
        [ContentSerializer]
        public Skeleton Joints { get; set; }
        [ContentSerializer]
        public ANSKTagData TagData { get; set; }

        public ANSKModelContent(List<Vector3> verts, List<int> vertInd, List<Vector2> uv, List<int> uvInd, List<int> edges, List<Vector3> normals, Skeleton joints, ANSKTagData tagData)
        {
            Verticies = verts;
            VertexIndicies = vertInd;
            Uvs = uv;
            UvIndicies = uvInd;
            Edges = edges;
            Normals = normals;
            Joints = joints;
            TagData = tagData;
        }

        private ANSKModelContent()
        {

        }

        public void ToXML(ref XmlWriter writer)
        {
            writer.WriteStartElement("Vertices");
            writer.WriteAttributeString("Count", Verticies.Count.ToString());
            writer.WriteElementString("VertList", XmlHelper.ListVector3ToXML(Verticies));
            writer.WriteEndElement();

            writer.WriteStartElement("Indices");
            writer.WriteAttributeString("Count", VertexIndicies.Count.ToString());
            writer.WriteElementString("IndiceList", XmlHelper.ListIntToXML(VertexIndicies));
            writer.WriteEndElement();

            writer.WriteStartElement("Uvs");
            writer.WriteAttributeString("Count", Uvs.Count.ToString());
            writer.WriteElementString("UvList", XmlHelper.ListVector2ToXML(Uvs));
            writer.WriteEndElement();

            writer.WriteStartElement("UvIndices");
            writer.WriteAttributeString("Count", UvIndicies.Count.ToString());
            writer.WriteElementString("UvIndiceList", XmlHelper.ListIntToXML(UvIndicies));
            writer.WriteEndElement();

            writer.WriteStartElement("Edges");
            writer.WriteAttributeString("Count", Edges.Count.ToString());
            writer.WriteElementString("EdgeList", XmlHelper.ListIntToXML(Edges));
            writer.WriteEndElement();

            writer.WriteStartElement("Normals");
            writer.WriteAttributeString("Count", Normals.Count.ToString());
            writer.WriteElementString("NormalList", XmlHelper.ListVector3ToXML(Normals));
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
