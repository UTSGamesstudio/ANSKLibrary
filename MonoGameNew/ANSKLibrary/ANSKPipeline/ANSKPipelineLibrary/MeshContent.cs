using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ANSKPipelineLibrary
{
    public class MeshContent
    {
        [ContentSerializer]
        public List<Vector3> Verticies { get; set; }
        [ContentSerializer]
        public List<short> VertexIndicies { get; set; }
        [ContentSerializer]
        public List<Vector2> Uvs { get; set; }
        [ContentSerializer]
        public List<int> UvIndicies { get; set; }
        [ContentSerializer]
        public List<int> Edges { get; set; }
        [ContentSerializer]
        public List<Vector3> Normals { get; set; }
        [ContentSerializer]
        public MaterialContent Materials { get; set; }

        public MeshContent(List<Vector3> verts, List<short> vertInd, List<Vector2> uvs, List<int> uvInd, List<int> edges, List<Vector3> normals, MaterialContent materialContent)
        {
            Verticies = verts;
            VertexIndicies = vertInd;
            Uvs = uvs;
            UvIndicies = uvInd;
            Edges = edges;
            Normals = normals;
            Materials = materialContent;
        }

        public MeshContent() { }

        public void ExportToXML(XmlWriter writer)
        {
            writer.WriteStartElement("Vertices");
            for (int i = 0; i < Verticies.Count; i++)
                writer.WriteElementString("Vert", XmlExporter.Vector3ToEntity(Verticies[i]));
            writer.WriteEndElement();

            writer.WriteStartElement("VertexIndices");
            for (int i = 0; i < VertexIndicies.Count; i++)
                writer.WriteElementString("VertIndex", VertexIndicies[i].ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Uvs");
            for (int i = 0; i < Uvs.Count; i++)
                writer.WriteElementString("Uv", XmlExporter.Vector2ToEntity(Uvs[i]));
            writer.WriteEndElement();

            writer.WriteStartElement("UvIndices");
            for (int i = 0; i < UvIndicies.Count; i++)
                writer.WriteElementString("UvIndex", UvIndicies[i].ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Edges");
            for (int i = 0; i < Edges.Count; i++)
                writer.WriteElementString("Edge", Edges[i].ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Normals");
            for (int i = 0; i < Normals.Count; i++)
                writer.WriteElementString("Normal", XmlExporter.Vector3ToEntity(Normals[i]));
            writer.WriteEndElement();

            writer.WriteStartElement("MaterialContent");
            Materials.ExportToXML(writer);
            writer.WriteEndElement();
        }
    }
}
