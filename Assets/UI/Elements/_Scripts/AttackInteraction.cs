using DarkFrontier.Objects;
using DarkFrontier.Utils;
using UnityEngine;

namespace DarkFrontier.UI.Elements
{
    public class AttackInteraction : Interaction
    {
        public AttackInteraction(Font font, Structure other, bool attacking, float damage, float margin = 8) : base(font, new RichTextBuilder().StartSize(20).StartColor(attacking ? new Color(0, 1, 1) : new Color(1, 0.5f, 0.5f)).Text(Mathf.RoundToInt(damage).ToString()).EndColor().StartColor(Color.white).Text(attacking ? " to " : " from ").Text(other.name).EndColor().EndSize().ToString(), margin)
        {
        }
    }
}