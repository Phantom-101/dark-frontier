namespace DarkFrontier.Items.Equipment.Weapons.Lasers
{
    public interface ILaserProviders
    {
        ILaserEndpointProvider Endpoint1Provider { get; }
        
        ILaserEndpointProvider Endpoint2Provider { get; }
        
        ILaserAlphaProvider AlphaProvider { get; }
        
        ILaserWidthProvider WidthProvider { get; }
    }
}