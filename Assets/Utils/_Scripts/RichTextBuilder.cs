using System.Text;
using UnityEngine;

namespace DarkFrontier.Utils
{
    public class RichTextBuilder
    {
        private readonly StringBuilder _sb = new();

        public RichTextBuilder Clear()
        {
            _sb.Clear();
            return this;
        }

        public RichTextBuilder Text(string text)
        {
            _sb.Append(text);
            return this;
        }

        public RichTextBuilder StartBold()
        {
            _sb.Append("<b>");
            return this;
        }

        public RichTextBuilder EndBold()
        {
            _sb.Append("</b>");
            return this;
        }

        public RichTextBuilder StartItalics()
        {
            _sb.Append("<i>");
            return this;
        }

        public RichTextBuilder EndItalics()
        {
            _sb.Append("</i>");
            return this;
        }

        public RichTextBuilder StartSize(int size)
        {
            _sb.Append("<size=").Append(size.ToString()).Append(">");
            return this;
        }

        public RichTextBuilder EndSize()
        {
            _sb.Append("</size>");
            return this;
        }

        public RichTextBuilder StartColor(string color)
        {
            _sb.Append("<color=").Append(color).Append(">");
            return this;
        }

        public RichTextBuilder StartColor(Color color)
        {
            _sb.Append("<color=").Append(ToRGBHex(color)).Append(">");
            return this;
        }

        public RichTextBuilder EndColor()
        {
            _sb.Append("</color>");
            return this;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        private static string ToRGBHex(Color c)
        {
            return $"#{ToByte(c.r):X2}{ToByte(c.g):X2}{ToByte(c.b):X2}";
        }

        private static byte ToByte(float f)
        {
            return (byte)(Mathf.Clamp01(f) * 255);
        }
    }
}