namespace NbatchzInfrastructure;
public interface ItemReader<T>
{
    public Task<T?> Read();
}
