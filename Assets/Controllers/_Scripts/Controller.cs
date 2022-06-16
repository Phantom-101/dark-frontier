#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Controllers.Priorities;
using DarkFrontier.Items.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers.New
{
    [Serializable]
    public class Controller
    {
        [field: SerializeReference]
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