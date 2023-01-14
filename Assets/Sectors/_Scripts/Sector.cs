#nullable enable
using System.Collections.Generic;
using DarkFrontier.Objects;
using Framework.Persistence;

namespace DarkFrontier.Sectors
{
    public class Sector : PersistentObject
    {
        public PersistentTypeSet typeSet = null!;
        public List<SectorObject> objects = new();

        public void Add(SectorObject obj)
        {
            if (!objects.Contains(obj))
            {
                objects.Add(obj);
            }
        }

        public void Remove(SectorObject obj)
        {
            objects.Remove(obj);
        }
        
        public override PersistentData Save(PersistentData? data = null)
        {
            data ??= new PersistentData();
            base.Save(data);
            data.Add("objects", objects.FindAll(e => e.IsTopLevel()).ConvertAll(e => e.Save()));
            return data;
        }

        public override void Load(PersistentData data)
        {
            base.Load(data);
            typeSet.Load(data.GetList<PersistentData>("objects"), transform);
        }
    }
}