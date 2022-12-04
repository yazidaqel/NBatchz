namespace NbatchzInfrastructure;
public interface ItemWriter<O>
{
    Task write(O output);
}
