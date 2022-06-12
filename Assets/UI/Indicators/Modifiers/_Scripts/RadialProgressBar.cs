#nullable enable
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Indicators.Modifiers
{
    public class RadialProgressBar : VisualElement
    {
        private const float _MaxAngle = 359.9999f;

        public new class UxmlFactory : UxmlFactory<RadialProgressBar, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlFloatAttributeDescription _value = new() { name = "value", defaultValue = 0 };
            private readonly UxmlFloatAttributeDescription _lineWidth = new() { name = "line-width", defaultValue = 1 };
            private readonly UxmlEnumAttributeDescription<LineCap> _lineCap = new() { name = "lineCap", defaultValue = LineCap.Round };
            private readonly UxmlEnumAttributeDescription<LineJoin> _lineJoin = new() { name = "lineJoin", defaultValue = LineJoin.Round };
            private readonly UxmlColorAttributeDescription _fillColor = new() { name = "fill-color", defaultValue = Color.white };
            private readonly UxmlColorAttributeDescription _strokeColor = new() { name = "stroke-color", defaultValue = Color.black };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var element = (RadialProgressBar)ve;
                element.Value = _value.GetValueFromBag(bag, cc);
                element.LineWidth = _lineWidth.GetValueFromBag(bag, cc);
                element.LineCap = _lineCap.GetValueFromBag(bag, cc);
                element.LineJoin = _lineJoin.GetValueFromBag(bag, cc);
                element.FillColor = _fillColor.GetValueFromBag(bag, cc);
                element.StrokeColor = _strokeColor.GetValueFromBag(bag, cc);
            }
        }

        public float Value { get; set; }
        public LineCap LineCap { get; set; }
        public LineJoin LineJoin { get; set; }
        public float LineWidth { get; set; }
        public Color FillColor { get; set; }
        public Color StrokeColor { get; set; }

        public RadialProgressBar()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            var paint2D = mgc.painter2D;
            paint2D.lineWidth = LineWidth;
            paint2D.lineCap = LineCap;
            paint2D.lineJoin = LineJoin;

            var width = mgc.visualElement.contentRect.width;
            var height = mgc.visualElement.contentRect.height;
            var radius = width / 2f;
            var center = new Vector2(width / 2, height / 2);
            var angle = Value * _MaxAngle;
            
            paint2D.fillColor = FillColor;
            paint2D.strokeColor = Color.clear;
            paint2D.BeginPath();
            paint2D.Arc(center, radius, 0, angle);
            paint2D.LineTo(center);
            paint2D.ClosePath();
            paint2D.Fill();
            
            if(LineWidth > 0)
            {
                paint2D.fillColor = Color.clear;
                paint2D.strokeColor = StrokeColor;
                paint2D.BeginPath();
                paint2D.Arc(center, radius, 0, angle);
                paint2D.Stroke();
            }
        }
    }
}