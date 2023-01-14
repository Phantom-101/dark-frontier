using Framework.Persistence;
using UnityEngine;

namespace DarkFrontier.Objects.Components
{
    public abstract class ObjectComponent : MonoBehaviour
    {
        public SectorObject obj;
        
        protected virtual void OnEnable()
        {
            obj = GetComponent<SectorObject>();
            obj.Add(this);
        }

        protected virtual void OnDisable()
        {
            obj.Remove(this);
        }

        public virtual void Save(PersistentData data)
        {
        }

        public virtual void Load(PersistentData data)
        {
        }
    }
}