#nullable enable
using DarkFrontier.Items.Structures;
using DarkFrontier.Utils;
using UnityEngine;

namespace DarkFrontier.UI.Indicators.Interactions
{
    public class AttackInteraction : IInteraction
    {
        public string Text { get; }

        public AttackInteraction(StructureComponent? other, bool attacking, float damage)
        {
            Text = new RichTextBuilder()
                .StartSize(20)
                .StartColor(attacking ? new Color(0, 1, 1) : new Color(1, 0.5f, 0.5f))
                .Text(Mathf.RoundToInt(damage).ToString())
                .EndColor()
                .StartColor(Color.white)
                .Text(attacking ? " to " : " from ")
                .Text(other == null ? "Unknown" : other.Instance?.Name ?? "Unknown")
                .EndColor()
                .EndSize()
                .ToString();
        }
    }
}