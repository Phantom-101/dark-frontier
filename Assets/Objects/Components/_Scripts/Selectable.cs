#nullable enable
using DarkFrontier.UI.Elements;
using DarkFrontier.Utils;
using Framework.Variables;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Objects.Components
{
    public class Selectable : ObjectComponent
    {
        public StructureVariable playerVariable = null!;
        public Detectable detectable = null!;
        public CameraVariable cameraVariable = null!;
        public UIDocument selectorDocument = null!;
        public VisualTreeAsset infoCard = null!;
        
        private Selector? _selector;

        protected virtual void Update()
        {
            if (_selector == null)
            {
                _selector = selectorDocument.rootVisualElement.Q<Selector>();
                _selector.RegisterCallback<ClickEvent>(OnClick);
            }
            if (PlayerExists() && ShouldShowSelector())
            {
                _selector.style.display = DisplayStyle.Flex;
                _selector.transform.position = GetSelectorPosition();
                _selector.IsSelected = playerVariable.value!.selected == this;
            }
            else
            {
                _selector.style.display = DisplayStyle.None;
            }
        }

        private bool PlayerExists()
        {
            return playerVariable.value != null;
        }
        
        protected bool IsNotPlayer()
        {
            return obj != playerVariable.value;
        }

        protected bool IsDetected()
        {
            return GameplayUtils.CanDetect(playerVariable.value!.detector, detectable);
        }
        
        protected static bool IsViewportPositionValid(Vector3 position)
        {
            return position.z > 0 && position.x is >= 0 and <= 1 && position.y is >= 0 and <= 1;
        }

        protected Vector3 GetViewportPosition()
        {
            return cameraVariable.value.WorldToViewportPoint(transform.position);
        }

        protected virtual bool ShouldShowSelector()
        {
            return IsNotPlayer() && IsDetected() && IsViewportPositionValid(GetViewportPosition());
        }

        protected virtual Vector3 GetSelectorPosition()
        {
            return RuntimePanelUtils.CameraTransformWorldToPanel(_selector!.panel, transform.position, cameraVariable.value);
        }
        
        public VisualElement NewInfoCard()
        {
            return infoCard.Instantiate();
        }

        private void OnClick(ClickEvent evt)
        {
            if (playerVariable.value != null)
            {
                playerVariable.value.selected = this;
            }
        }
    }
}