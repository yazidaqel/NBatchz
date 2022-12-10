using NBatchzInfrastructure.Database.Support;
using Oracle.ManagedDataAccess.Client;


namespace NbatchzInfrastructure.Database.Support
{
    public class OraclePagingItemReader<T> : AbstractPagingItemReader<T>
    {

        private int _fetchSize = 100;

        private OracleConnection? _oracleConnection;
        private readonly OracleRowMapper<T>? _rowMapper;

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

        protected override async Task DoReadPageAsync()
        {

            if (_oracleConnection == null)
                return;
            
            if (_rowMapper == null)
                return;
             
            using OracleCommand oracleCommand = _oracleConnection.CreateCommand();
            oracleCommand.CommandText = "";
            oracleCommand.FetchSize = oracleCommand.RowSize * _fetchSize;

            OracleDataReader oracleDataReader = oracleCommand.ExecuteReader();

            while (await oracleDataReader.ReadAsync())
            {
                _results.Add(await _rowMapper.MapRow(oracleDataReader));
            }



        }
    }
}


