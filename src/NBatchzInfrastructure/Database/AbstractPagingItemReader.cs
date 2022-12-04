namespace NbatchzInfrastructure.Database;


using System.Collections;

public abstract class AbstractPagingItemReader<T> : ItemStreamReader<T>
{

    private int _pageSize = 100;
    private int _current = 0;
    private int _page = 0;
    private int _currentItemCount = 0;
    private int _maxItemCount = Int32.MaxValue;
    protected IList<T> _results = new List<T>();
    private bool _isInitialized;

    private T? DoRead()
    {

        if (_current > _pageSize)
        {
            DoReadPage();
            _page++;
            if (_current >= _pageSize)
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
            return default(T);
        }

    }

    public Task<T?> Read()
    {
        return Task.Run(() =>
        {
            if (_currentItemCount >= _maxItemCount)
            {
                return default(T);
            }

            _currentItemCount++;

            T item = DoRead();

            if (item != null && item.GetType().IsAssignableFrom(typeof(ItemCountAware)))
            {
                ((ItemCountAware)item).SetItemCount(_currentItemCount);
            }

            return item;
        });


    }

    protected abstract void DoReadPage();
    protected abstract void DoJumpToPage(int itemIndex);
    public abstract Task open();
    public abstract Task update();
    public abstract Task close();
}