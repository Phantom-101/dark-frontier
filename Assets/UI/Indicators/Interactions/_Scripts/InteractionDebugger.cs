using System.Collections;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Structures;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Indicators.Interactions
{
    public class InteractionDebugger : MonoBehaviour
    {
        public UIDocument document;
        public string other;

        private InteractionList _list;
        
        private void Start()
        {
            _list = document.rootVisualElement.Q<InteractionList>();
            StartCoroutine(SendInteraction());
        }

        private IEnumerator SendInteraction()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 2));
            var structure = Singletons.Get<IdRegistry>().Get<StructureComponent>(other);
            if(structure != null)
            {
                _list.AddInteraction(new AttackInteraction(structure, Random.Range(0, 2) == 1, Random.Range(0.0f, 100)));
            }
            StartCoroutine(SendInteraction());
        }
    }
}