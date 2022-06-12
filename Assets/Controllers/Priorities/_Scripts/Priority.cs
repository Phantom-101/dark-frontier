#nullable enable
using System;
using DarkFrontier.Items.Structures;

namespace DarkFrontier.Controllers.Priorities
{
    [Serializable]
    public class Priority
    {
        public virtual bool Tick(StructureComponent target)
        {
            return false;
        }
    }
}