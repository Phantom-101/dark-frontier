using System.Collections.Generic;
using DarkFrontier.Utils;
using UnityEngine;

#nullable enable
namespace DarkFrontier.UI.Indicators.Selectors
{
    public class SelectorSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform? _root;

        [field: SerializeReference]
        public ISelectable? Selected { get; private set; }

        private readonly HashSet<ISelectable> _selectables = new HashSet<ISelectable>();
        private readonly List<Selector> _selectors = new List<Selector>();

        private readonly List<Selector> _subSelectors = new List<Selector>();

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            var behaviours = FindObjectsOfType<MonoBehaviour>();
            for(int i = 0, l = behaviours.Length; i < l; i++)
            {
                if (behaviours[i] is ISelectable selectable)
                {
                    if (selectable.Parent == null)
                    {
                        //if (selectable.IsDetected(null))
                        //{
                            
                        //}
                        if(!_selectables.Contains(selectable))
                        {
                            var selector = Spawn(selectable);
                            if(selector != null)
                            {
                                _selectables.Add(selectable);
                                _selectors.Add(selector);
                            }
                        }
                    }
                }
            }

            for(int i = 0, l = _subSelectors.Count; i < l; i++)
            {
                AddressableUtils.Destroy(_subSelectors[i].gameObject);
            }

            _subSelectors.Clear();

            if(Selected != null)
            {
                for(int i = 0, li = Selected.Children.Length; i < li; i++)
                {
                    var selector = Spawn(Selected.Children[i]);
                    if (selector != null)
                    {
                        _subSelectors.Add(selector);
                    }
                }
            }
        }

        private void Update()
        {
            for(int i = 0, l = _selectors.Count; i < l; i++)
            {
                _selectors[i].Tick(UnityEngine.Time.deltaTime);
            }
            for(int i = 0, l = _subSelectors.Count; i < l; i++)
            {
                _subSelectors[i].Tick(UnityEngine.Time.deltaTime);
            }
        }

        private Selector? Spawn(ISelectable selectable)
        {
            var selector = selectable.CreateSelector(_root);

            if (selector != null)
            {
                selector.Initialize();
            }

            return selector;
        }
    }
}
#nullable restore