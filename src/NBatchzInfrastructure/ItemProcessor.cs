namespace NbatchzInfrastructure;
public interface ItemProcessor<I,O>
{
    Task<O> process(I input);
}
