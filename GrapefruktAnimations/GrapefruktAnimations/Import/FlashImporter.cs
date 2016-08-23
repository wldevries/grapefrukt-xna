using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FlashAnimations.Import
{
    public static class FlashImporter
    {
        public static Animations LoadAnimations(string xml)
        {
            return Deserialize<Animations>(xml);
        }

        public static Textures LoadSheets(string xml)
        {
            return Deserialize<Textures>(xml);
        }

        public static T Deserialize<T>(string xml) where T : class
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var result = xs.Deserialize(ms) as T;
                return result;
            }
        }
    }
}