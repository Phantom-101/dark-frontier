#nullable enable
using System;
using DarkFrontier.Items.Structures;

namespace DarkFrontier.Controllers.Priorities
{
    [Serializable]
    public class CombatPriority : Priority
    {
        public float range;

        public float aggression;
        
        public override bool Tick(StructureComponent target)
        {
            return base.Tick(target);
        }
    }
}