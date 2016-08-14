using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace FlashAnimations.Import
{
    public class FlashImporter
    {
        public Animations LoadAnimations(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Animations));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            var animations = xs.Deserialize(ms) as Animations;
            return animations;
        }
    }

    public class Animations
    {
        [XmlElement("Animation")]
        public Animation[] Animation { get; set; }
    }

    public class Animation
    {
        [XmlAttribute("name")]
        public string name { get; set; }
        [XmlAttribute("frameCount")]
        public int frameCount { get; set; }
        [XmlElement("Part")]
        public Part[] Parts { get; set; }
    }

    public class Part
    {
        [XmlAttribute("name")]
        public string name { get; set; }
        [XmlElement("Frame")]
        public Frame[] Frames { get; set; }
    }
}