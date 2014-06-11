using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ANSKLibrary;

namespace ANSKContentPipeline
{
    public class ANSKExporter
    {
        public static void XmlSerialiser(ANSKModelContent content, string name)
        {
            name = name.TrimEnd('t', 'x', 't', '.', 's', 'm', 'i', 'n', 'A');

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter file = XmlWriter.Create(name + ".ansk", settings))
            {
                file.WriteStartDocument();
                file.WriteStartElement("ModelContent");

                file.WriteEndElement();
                file.WriteEndDocument();
            }
        }
    }
}
