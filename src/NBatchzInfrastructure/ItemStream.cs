using NBatchzInfrastructure;

namespace NBatchzInfrastructure;
public interface ItemStream
{
    public Task open();

    public Task close();
}
