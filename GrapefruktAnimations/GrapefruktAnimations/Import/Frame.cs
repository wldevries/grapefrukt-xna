using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace FlashAnimations.Import
{
    public class Frame
    {
        public Frame()
        {
            // Set default values
            this.scaleX = 1;
            this.scaleY = 1;
            this.alpha = 1;
        }

        [XmlAttribute("x")]
        public float x { get; set; }
        [XmlAttribute("y")]
        public float y { get; set; }

        public Vector2 Position => new Vector2(x, y);

        [XmlAttribute("scaleX")]
        public float scaleX { get; set; }
        [XmlAttribute("scaleY")]
        public float scaleY { get; set; }

        public Vector2 Scale => new Vector2(scaleX, scaleY);

        [XmlAttribute("alpha")]
        public float alpha { get; set; }
        [XmlAttribute("rotation")]
        public float rotation { get; set; }
        [XmlAttribute("index")]
        public int index { get; set; }

        public static float LerpValue(
            Frame frame1, Frame frame2, float weight, Func<Frame, float> selector)
        {
            if (frame1 == null && frame2 == null)
                return default(float);
            else if (frame1 == null)
                return selector(frame2);
            else if (frame2 == null)
                return selector(frame1);
            else
                return MathHelper.Lerp(selector(frame1), selector(frame2), weight);
        }

        public static Vector2 LerpValue(
            Frame frame1, Frame frame2, float weight, Func<Frame, Vector2> selector)
        {
            if (frame1 == null && frame2 == null)
                return default(Vector2);
            else if (frame1 == null)
                return selector(frame2);
            else if (frame2 == null)
                return selector(frame1);
            else
                return Vector2.Lerp(selector(frame1), selector(frame2), weight);
        }
    }
}
