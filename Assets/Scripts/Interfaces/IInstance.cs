public interface IInstance<T> {
    IPrototype<T> Prototype {
        get;
    }

    void FromPrototype (IPrototype<T> prototype);
}
