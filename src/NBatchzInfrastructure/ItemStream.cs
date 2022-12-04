namespace NbatchzInfrastructure;
public interface ItemStream
{
    public Task open();

    public Task update();

    public Task close();
}
