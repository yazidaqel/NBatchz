namespace NbatchzInfrastructure;
public interface ItemWriter<O>
{
    Task write(List<O> output);
}
