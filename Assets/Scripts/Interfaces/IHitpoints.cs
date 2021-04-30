public interface IHitpoints {
    StructureStat MaxHitpoints {
        get;
    }
    float Hitpoints {
        get;
    }
    event OnDestroyedEventHandler OnDestroyed;

    void TakeDamage (float amount, IInfo damager);
}

public delegate void OnDestroyedEventHandler (IHitpoints sender, IInfo destroyer);
