using System.Collections.Generic;

namespace DarkFrontier.Items.Structures
{
    public class StructureRegistry
    {
        public List<StructureComponent> Registry { get; } = new();

        public void Register(StructureComponent structure) => Registry.Add(structure);

        public void Unregister(StructureComponent structure) => Registry.Remove(structure);

        public void Tick(float deltaTime)
        {
            for(int i = 0, l = Registry.Count; i < l; i++)
            {
                Registry[i].Tick(deltaTime);
            }
        }
        
        public void FixedTick(float deltaTime)
        {
            for(int i = 0, l = Registry.Count; i < l; i++)
            {
                Registry[i].FixedTick(deltaTime);
            }
        }
    }
}