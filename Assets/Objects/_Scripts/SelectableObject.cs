#nullable enable
using DarkFrontier.Objects.Selectables;
using Framework.Channels;
using Framework.Variables;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Objects
{
    public class SelectableObject : SectorObject
    {
        public FloatChannel onUITickChannel = null!;
        public CameraVariable cameraVariable = null!;
        public UIDocument selectorDocument = null!;
        
        private Selector? _selector;

        protected override void OnEnable()
        {
            base.OnEnable();
            onUITickChannel.onEvent += OnUITick;
        }

        protected virtual void OnUITick(object sender, float deltaSeconds)
        {
            _selector ??= selectorDocument.rootVisualElement.Q<Selector>();
            if (ShouldShowSelector())
            {
                _selector.style.display = DisplayStyle.Flex;
                _selector.transform.position = GetSelectorPosition();
            }
            else
            {
                _selector.style.display = DisplayStyle.None;
            }
        }

        protected static bool IsViewportPositionValid(Vector3 position)
        {
            return position.z > 0 && position.x is >= 0 and <= 1 && position.y is >= 0 and <= 1;
        }

        protected virtual bool ShouldShowSelector()
        {
            return IsViewportPositionValid(cameraVariable.value.WorldToViewportPoint(transform.position));
        }

        protected virtual Vector3 GetSelectorPosition()
        {
            return RuntimePanelUtils.CameraTransformWorldToPanel(_selector!.panel, transform.position, cameraVariable.value);
        }
    }
}