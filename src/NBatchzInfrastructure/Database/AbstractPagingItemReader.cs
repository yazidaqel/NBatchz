namespace NbatchzInfrastructure.Database;


using System.Collections;
using NBatchzInfrastructure;

public abstract class AbstractPagingItemReader<T> : ItemStreamReader<T>
{

    protected int PageSize { get; set; } = 100;
    private int _current = 0;
    protected int Page { get; set; } = 0;
    public int CurrentItemCount { get; set; } = 0;
    private int _maxItemCount = Int32.MaxValue;
    protected IList<T> _results = new List<T>();
    private bool _isInitialized;
    protected string? ConnectionString { get; set; }

    private T? DoRead()
    {

        if (_current > PageSize)
        {
            DoReadPageAsync();
            Page++;
            if (_current >= PageSize)
            {
                _current = 0;
            }
        }

        int next = _current++;
        if (next < _results.Count)
        {
            return _results[next];
        }
        else
        {
            return default;
        }

    }

    public Task<T?> Read()
    {
        return Task.Run(() =>
        {
            if (CurrentItemCount >= _maxItemCount)
            {
                return default;
            }

            CurrentItemCount++;

            T? item = DoRead();

            if (item != null && item.GetType().IsAssignableFrom(typeof(ItemCountAware)))
            {
                ((ItemCountAware)item).SetItemCount(CurrentItemCount);
            }

            return item;
        });


    }

    protected abstract Task DoReadPageAsync();
    protected abstract void DoJumpToPage(int itemIndex);
    public abstract Task open();
    public abstract Task close();
}