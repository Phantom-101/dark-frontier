using System;
using System.Collections.Generic;
using DarkFrontier.Items.Structures;
using UnityEngine;

namespace DarkFrontier.UI.Indicators.Selectors
{
    [Serializable]
    public class DetectableRegistry
    {
        [field: SerializeReference]
        public List<IDetectable> Detectables { get; private set; } = new();
    }
}