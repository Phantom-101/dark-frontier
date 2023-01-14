#nullable enable
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Weapons.Lasers
{
    public class LaserVisuals : MonoBehaviour
    {
        public Transform scaler = null!;
        public Renderer quad1 = null!;
        public Renderer quad2 = null!;
        
        private ILaserProviders _providers = null!;

        private void Awake()
        {
            enabled = false;
        }

        public void UseProviders(ILaserProviders providers)
        {
            _providers = providers;
            enabled = true;
        }

        private void Update()
        {
            OverrideTransform(scaler, _providers.Endpoint1Provider.Position, _providers.Endpoint2Provider.Position, _providers.WidthProvider.Width);
            var alpha = _providers.AlphaProvider.Alpha;
            OverrideAlpha(quad1.material, alpha);
            OverrideAlpha(quad2.material, alpha);
        }

        private static void OverrideTransform(Transform t, Vector3 pos1, Vector3 pos2, float width)
        {
            var dist = (pos1 - pos2).magnitude;
            t.position = pos1;
            t.LookAt(pos2);
            t.localScale = new Vector3(width, width, dist);
        }

        private static void OverrideAlpha(Material material, float value)
        {
            var color = material.color;
            color.a = value;
            material.color = color;
        }
    }
}
