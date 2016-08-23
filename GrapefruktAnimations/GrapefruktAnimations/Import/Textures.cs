using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FlashAnimations.Import
{
    public class Textures
    {
        [XmlElement("TextureSheet")]
        public TextureSheet[] Sheets { get; set; }
    }

    public class TextureSheet
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("Texture")]
        public TextureDef[] Textures { get; set; }
    }

    public class TextureDef
    {
        public TextureDef()
        {
            this.FrameCount = 1;
            this.Columns = 1;
        }

        [XmlAttribute("name")]
        public string name { get; set; }
        [XmlAttribute("width")]
        public int width { get; set; }
        [XmlAttribute("height")]
        public int height { get; set; }
        [XmlAttribute("path")]
        public string path { get; set; }
        [XmlAttribute("registrationPointX")]
        public float registrationPointX { get; set; }
        [XmlAttribute("registrationPointY")]
        public float registrationPointY { get; set; }
        [XmlAttribute("zIndex")]
        public int zIndex { get; set; }
        [XmlAttribute("frameCount")]
        public int FrameCount { get; set; }
        [XmlAttribute("frameWidth")]
        public int frameWidth { get; set; }
        [XmlAttribute("frameHeight")]
        public int frameHeight { get; set; }
        [XmlAttribute("columns")]
        public int Columns { get; set; }
    }
}
