using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DarkFrontier.Utils
{
    public static class UIUtils
    {
        public static void SetMargin(this IStyle style, float left, float right, float top, float bottom)
        {
            style.marginLeft = left;
            style.marginRight = right;
            style.marginTop = top;
            style.marginBottom = bottom;
        }
        
        public static void SetPadding(this IStyle style, float left, float right, float top, float bottom)
        {
            style.paddingLeft = left;
            style.paddingRight = right;
            style.paddingTop = top;
            style.paddingBottom = bottom;
        }
        
        public static void AddTransition(this IStyle style, StylePropertyName name, TimeValue delay, TimeValue duration, EasingFunction easing)
        {
            if(style.transitionProperty.value == null)
            {
                style.transitionProperty = new StyleList<StylePropertyName>(new List<StylePropertyName>());
                style.transitionDelay = new StyleList<TimeValue>(new List<TimeValue>());
                style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue>());
                style.transitionTimingFunction = new StyleList<EasingFunction>(new List<EasingFunction>());
            }
            for(int i = 0, l = style.transitionProperty.value.Count; i < l; i++)
            {
                if(style.transitionProperty.value[i] == name)
                {
                    style.transitionDelay.value[i] = delay;
                    style.transitionDuration.value[i] = duration;
                    style.transitionTimingFunction.value[i] = easing;
                    return;
                }
            }   
            style.transitionProperty.value.Add(name);
            style.transitionDelay.value.Add(delay);
            style.transitionDuration.value.Add(duration);
            style.transitionTimingFunction.value.Add(easing);
        }

        public static void RemoveTransition(this IStyle style, StylePropertyName name)
        {
            if(style.transitionProperty.value == null)
            {
                style.transitionProperty = new StyleList<StylePropertyName>(new List<StylePropertyName>());
                style.transitionDelay = new StyleList<TimeValue>(new List<TimeValue>());
                style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue>());
                style.transitionTimingFunction = new StyleList<EasingFunction>(new List<EasingFunction>());
            }
            for(int i = 0, l = style.transitionProperty.value.Count; i < l; i++)
            {
                if(style.transitionProperty.value[i] == name)
                {
                    style.transitionProperty.value.RemoveAt(i);
                    style.transitionDelay.value.RemoveAt(i);
                    style.transitionDuration.value.RemoveAt(i);
                    style.transitionTimingFunction.value.RemoveAt(i);
                    return;
                }
            }
        }
    }
}