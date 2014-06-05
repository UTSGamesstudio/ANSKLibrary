using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace ANSKPipelineLibrary
{
    public class MaterialContent
    {
        [ContentSerializer]
        public List<Material> Materials { get; set; }
        [ContentSerializer]
        public List<int> MaterialIndicieList { get; set; }

        public MaterialContent(List<Material> mats, List<int> matInd)
        {
            Materials = mats;
            MaterialIndicieList = matInd;
        }

        public MaterialContent() { }

        public void ExportToXML(XmlWriter writer)
        {
            for (int i = 0; i < Materials.Count; i++)
            {
                writer.WriteStartElement("Material");
                Materials[i].ExportToXML(writer);
                writer.WriteEndElement();
            }

            writer.WriteStartElement("MaterialIndices");
            for (int i = 0; i < MaterialIndicieList.Count; i++)
                writer.WriteElementString("Index", MaterialIndicieList[i].ToString());
            writer.WriteEndElement();
        }
    }
}
