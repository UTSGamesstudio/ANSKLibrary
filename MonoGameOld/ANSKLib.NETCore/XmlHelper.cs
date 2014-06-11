using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ANSKLibrary
{
    public class XmlHelper
    {
        public static string Vector3ToXML(Vector3 v)
        {
            return v.X.ToString() + v.Y.ToString() + v.Z.ToString();
        }

        public static string ListIntToXML(List<int> list)
        {
            return ListToXMLSingleVal<int>(list);
        }

        public static string ListFloatToXML(List<float> list)
        {
            return ListToXMLSingleVal<float>(list);
        }

        public static string ListVector3ToXML(List<Vector3> list)
        {
            string temp = "";

            for (int i = 0; i < list.Count; i++)
            {
                temp += list[i].X.ToString() + "," + list[i].Y.ToString() + "," + list[i].Z.ToString();

                if (i != list.Count - 1)
                    temp += ",";
            }

            return temp;
        }

        public static string ListVector2ToXML(List<Vector2> list)
        {
            string temp = "";

            for (int i = 0; i < list.Count; i++)
            {
                temp += list[i].X.ToString() + "," + list[i].Y.ToString();

                if (i != list.Count - 1)
                    temp += ",";
            }

            return temp;
        }

        static public string MatrixToXML(Matrix matrix)
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

        private static string ListToXMLSingleVal<T>(List<T> list)
        {
            string temp = "";

            for (int i = 0; i < list.Count; i++)
            {
                temp += list[i].ToString();

                if (i != list.Count - 1)
                    temp += ",";
            }

            return temp;
        }
    }
}
