using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ANSKPipelineLibrary
{
    public class ANSKModelContent
    {
        [ContentSerializer]
        public MeshContent Mesh { get; set; }
        [ContentSerializer]
        public Skeleton Joints { get; set; }
        [ContentSerializer]
        public SkinningData Skin { get; set; }
        [ContentSerializer]
        public List<BlendShapeContent> BlendShapes { get; set; }

        public ANSKModelContent(List<Vector3> verts, List<short> vertInd, List<Vector2> uv, List<int> uvInd, List<int> edges, List<Vector3> normals, Skeleton joints, SkinningData skin, List<BlendShapeContent> bShapes, MaterialContent materials)
        {
            Mesh = new MeshContent(verts, vertInd, uv, uvInd, edges, normals, materials);
            Joints = joints;
            Skin = skin;
            BlendShapes = bShapes;
        }

        private ANSKModelContent()
        {

        }

        public void ExportToXML(XmlWriter writer)
        {
            if (Mesh != null)
            {
                writer.WriteStartElement("MeshContent");
                Mesh.ExportToXML(writer);
                writer.WriteEndElement();
            }

            if (Joints != null)
            {
                writer.WriteStartElement("Skeleton");
                Joints.ExportToXML(writer);
                writer.WriteEndElement();
            }

            if (Skin != null)
            {
                writer.WriteStartElement("Skin");
                Skin.ExportToXML(writer);
                writer.WriteEndElement();
            }

            if (BlendShapes != null)
            {
                for (int i = 0; i < BlendShapes.Count; i++)
                {
                    writer.WriteStartElement("BlendShapeContent");
                    BlendShapes[i].ExportToXML(writer);
                    writer.WriteEndElement();
                }
            }
        }
    }
}
