using NbatchzInfrastructure;
using Oracle.ManagedDataAccess.Client;


namespace NbatchzInfrastructure.Database.Support;

public class OraclePagingItemReader<T> : AbstractPagingItemReader<T>
{

    private int _fetchSize = 100;

    private OracleConnection _oracleConnection;
    public override Task close()
    {
        return Task.Run(async () =>
        {
            _oracleConnection = new OracleConnection("");
            await _oracleConnection.DisposeAsync();
            await _oracleConnection.CloseAsync();
        });
    }

    public override Task open()
    {
        return Task.Run(async () =>
        {
            _oracleConnection = new OracleConnection("");
            await _oracleConnection.OpenAsync();
        });

    }

    public override Task update()
    {
        throw new NotImplementedException();
    }

    protected override void DoJumpToPage(int itemIndex)
    {
        throw new NotImplementedException();
    }

    protected override void DoReadPage()
    {

        if(_oracleConnection == null)
            return;
        
        using (OracleCommand oracleCommand = _oracleConnection.CreateCommand()){
            oracleCommand.CommandText = "";
            oracleCommand.FetchSize = _fetchSize;

            OracleDataReader oracleDataReader = oracleCommand.

        }

    }
}
