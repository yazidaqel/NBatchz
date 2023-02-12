namespace NBatchzInfrastructure;
public interface ItemReader<T>
{
    public Task<T?> Read();
}
