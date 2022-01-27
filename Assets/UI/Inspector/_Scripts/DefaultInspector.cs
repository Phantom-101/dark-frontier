using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Inspector
{
    public static class DefaultInspector
    {
        public static VisualElement Create(object obj)
        {
            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var inspector = new VisualElement
            {
                style =
                {
                    marginLeft = new StyleLength(new Length(8, LengthUnit.Pixel)),
                    marginRight = new StyleLength(new Length(8, LengthUnit.Pixel)),
                    marginTop = new StyleLength(new Length(8, LengthUnit.Pixel)),
                    marginBottom = new StyleLength(new Length(8, LengthUnit.Pixel)),
                    paddingLeft = new StyleLength(new Length(8, LengthUnit.Pixel)),
                    paddingRight = new StyleLength(new Length(8, LengthUnit.Pixel)),
                    paddingTop = new StyleLength(new Length(8, LengthUnit.Pixel)),
                    paddingBottom = new StyleLength(new Length(8, LengthUnit.Pixel)),
                    borderLeftColor = new StyleColor(new Color(0.75f, 0.75f, 0.75f)),
                    borderRightColor = new StyleColor(new Color(0.75f, 0.75f, 0.75f)),
                    borderTopColor = new StyleColor(new Color(0.75f, 0.75f, 0.75f)),
                    borderBottomColor = new StyleColor(new Color(0.75f, 0.75f, 0.75f)),
                    borderLeftWidth = new StyleFloat(2),
                    borderRightWidth = new StyleFloat(2),
                    borderTopWidth = new StyleFloat(2),
                    borderBottomWidth = new StyleFloat(2),
                    borderTopLeftRadius = new StyleLength(new Length(2, LengthUnit.Pixel)),
                    borderBottomLeftRadius = new StyleLength(new Length(2, LengthUnit.Pixel)),
                    borderTopRightRadius = new StyleLength(new Length(2, LengthUnit.Pixel)),
                    borderBottomRightRadius = new StyleLength(new Length(2, LengthUnit.Pixel))
                }
            };

            inspector.Add(new TextElement
            {
                text = $"{type}",
                style =
                {
                    fontSize = new StyleLength(new Length(16, LengthUnit.Pixel))
                }
            });
            
            for(int i = 0, l = fields.Length; i < l; i++)
            {
                var fieldType = fields[i].FieldType;
                if(fieldType == typeof(byte) ||
                   fieldType == typeof(short) ||
                   fieldType == typeof(int) ||
                   fieldType == typeof(long) ||
                   fieldType == typeof(float) ||
                   fieldType == typeof(double) ||
                   fieldType == typeof(decimal) ||
                   fieldType == typeof(string))
                {
                    inspector.Add(new TextElement
                    {
                        text = $"{fields[i].Name}: {fields[i].GetValue(obj)}",
                        style =
                        {
                            fontSize = new StyleLength(new Length(12, LengthUnit.Pixel))
                        }
                    });
                }
            }

            return inspector;
        }

        public static VisualElement Create(List<IInspectable> list)
        {
            var inspector = new VisualElement();

            for(int i = 0, l = list.Count; i < l; i++)
            {
                inspector.Add(list[i].CreateInspector());
            }

            return inspector;
        }
    }
}