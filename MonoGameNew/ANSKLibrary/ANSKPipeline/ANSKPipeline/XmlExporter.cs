using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using ANSKPipelineLibrary;

namespace ANSKPipeline
{
    public class XmlExporter
    {
        public static void Export(ANSKModelContent content, string modelName)
        {
            modelName = modelName.TrimEnd('t', 'x', 't', '.');

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter file = XmlWriter.Create(modelName + ".ansk", settings))
            {
                file.WriteStartDocument();
                file.WriteStartElement("ANSKModelContent");

                content.ExportToXML(file);

                file.WriteEndElement();
                file.WriteEndDocument();
            }
        }

        static public string MatrixToEntity(Matrix matrix)
        {
            string temp = matrix.M11.ToString() + "," +
                matrix.M12.ToString() + "," +
                matrix.M13.ToString() + "," +
                matrix.M14.ToString() + "," +
                matrix.M21.ToString() + "," +
                matrix.M22.ToString() + "," +
                matrix.M23.ToString() + "," +
                matrix.M24.ToString() + "," +
                matrix.M31.ToString() + "," +
                matrix.M32.ToString() + "," +
                matrix.M33.ToString() + "," +
                matrix.M34.ToString() + "," +
                matrix.M41.ToString() + "," +
                matrix.M42.ToString() + "," +
                matrix.M43.ToString() + "," +
                matrix.M44.ToString();

            return temp;
        }

        public static string Vector3ToEntity(Vector3 vector)
        {
            string temp = vector.X.ToString() + "," +
                vector.Y.ToString() + "," +
                vector.Z.ToString();

            return temp;
        }

        public static string Vector2ToEntity(Vector2 vector)
        {
            string temp = vector.X.ToString() + "," +
                vector.Y.ToString();

            return temp;
        }
    }
}
