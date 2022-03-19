namespace DarkFrontier.Data.Pipelines
{
    public interface IPipelineInput<in T>
    {
        void Process(T input);
    }
}