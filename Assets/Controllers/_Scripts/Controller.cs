#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Controllers.Priorities;
using DarkFrontier.Items.Structures;

namespace DarkFrontier.Controllers.New
{
    [Serializable]
    public class Controller
    {
        public List<Priority> Priorities { get; private set; } = new();
        
        public Controller()
        {
        }

        public Controller(List<Priority> priorities)
        {
            Priorities = priorities;
        }

        public void Tick(StructureComponent target)
        {
            for(int i = 0, l = Priorities.Count; i < l; i++)
            {
                if(Priorities[i].Tick(target))
                {
                    break;
                }
            }
        }
    }
}