#nullable enable
using System.Collections.Generic;
using DarkFrontier.Objects.Components;
using DarkFrontier.Sectors;
using Framework.Persistence;

namespace DarkFrontier.Objects
{
    public class SectorObject : PersistentObject
    {
        public Sector Sector
        {
            get => _sector;
            set
            {
                if (_sector == value)
                {
                    return;
                }
                _sector.Remove(this);
                _sector = value;
                _sector.Add(this);
            }
        }

        private Sector _sector = null!;

        public List<ObjectComponent> components = new();

        protected virtual void OnEnable()
        {
            _sector = GetComponentInParent<Sector>();
            _sector.Add(this);
        }

        protected virtual void OnDisable()
        {
            _sector.Remove(this);
        }

        public virtual bool IsTopLevel()
        {
            return true;
        }

        public void Add(ObjectComponent comp)
        {
            if (!components.Contains(comp))
            {
                components.Add(comp);
            }
        }

        public void Remove(ObjectComponent comp)
        {
            components.Remove(comp);
        }
    }
}