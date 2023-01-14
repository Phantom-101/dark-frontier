#nullable enable
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Weapons.Lasers
{
    public class RelativeLaserEndpointProvider : ILaserEndpointProvider
    {
        public Vector3 Position => _transform == null ? _offset : _transform.TransformPoint(_offset);

        private readonly Transform? _transform;
        private readonly Vector3 _offset;

        public RelativeLaserEndpointProvider(Transform? transform, Vector3 offset)
        {
            _transform = transform;
            _offset = offset;
        }
    }
}