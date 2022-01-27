using DarkFrontier.Items.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace DarkFrontier.Positioning.Sectors
{
    public class SectorComponent : MonoBehaviour, ISelectable
    {
        [SerializeReference]
        public SectorInstance? instance;

        public ISelectable? Parent => null;

        public ISelectable[] Children => Array.Empty<ISelectable>();

        public string Name => instance?.Name ?? "";

        public string Description => instance?.Description ?? "";

        public Vector3 Position => instance?.Position ?? Vector3.zero;

        private void Start()
        {
            SectorSpawner.Create(this);
        }

        public bool IsDetected(StructureComponent structure) => true;

        public Selector? CreateSelector(Transform? root)
        {
            if(instance == null)
            {
                return null;
            }

            var selector = Addressables.InstantiateAsync(instance.SelectorPrefabAddress, root).WaitForCompletion().GetComponent<BasicSelector>();
            selector.selectable = this;

            return selector;
        }
    }
}
