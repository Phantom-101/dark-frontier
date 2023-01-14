using UnityEngine;

namespace DarkFrontier.Items.Equipment.Weapons.Lasers
{
    public interface ILaserEndpointProvider
    {
        Vector3 Position { get; }
    }
}