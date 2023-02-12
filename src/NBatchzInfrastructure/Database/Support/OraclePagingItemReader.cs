using System.Linq;
using NBatchzInfrastructure;
using NBatchzInfrastructure.Database;
using NBatchzInfrastructure.Database.Support;
using Oracle.ManagedDataAccess.Client;


namespace NbatchzInfrastructure.Database.Support
{
    public class OraclePagingItemReader<T> : AbstractPagingItemReader<T>
    {

        private static readonly int DEFAULT_FETCH_SIZE = 100;

        private static readonly string START_AFTER_VALUE = "start.after";

        private readonly int _fetchSize = DEFAULT_FETCH_SIZE;

        private PagingQueryProvider? PagingQueryProvider { get; set; }

        private OracleConnection? _oracleConnection;
        private OracleRowMapper<T>? RowMapper { get; set; }

        private string? _firstPageSql;
        private string? _remainingPagesSql;

        public override Task open()
        {
            return Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(ConnectionString))
                    throw new ArgumentException("Connection string cannot be null");

                if (PagingQueryProvider == null)
                    throw new ArgumentException("Paging query provider cannot be null");

                _oracleConnection = new OracleConnection(ConnectionString);
                await _oracleConnection.OpenAsync();

                this._firstPageSql = PagingQueryProvider.GenerateFirstPageQuery(PageSize);
                this._remainingPagesSql = PagingQueryProvider.GenerateRemainingPagesQuery(PageSize);
            });

        }


        public override Task close()
        {
            return Task.Run(async () =>
            {
                if (_oracleConnection != null)
                {
                    await _oracleConnection.DisposeAsync();
                    await _oracleConnection.CloseAsync();
                }
            });
        }


        protected override void DoJumpToPage(int itemIndex)
        {
            throw new NotImplementedException();
        }

        protected override async Task DoReadPageAsync()
        {

            _results.Clear();

            if (_oracleConnection == null)
                return;

            if (RowMapper == null)
                return;

            PagingOracleRowMapper rowCallback = new PagingOracleRowMapper(this);

            using OracleCommand oracleCommand = _oracleConnection.CreateCommand();

            if (Page == 0)
            {
                oracleCommand.CommandText = _firstPageSql;
            }
            else
            {
                oracleCommand.CommandText = _remainingPagesSql;
            }

            oracleCommand.FetchSize = oracleCommand.RowSize * _fetchSize;

            OracleDataReader oracleDataReader = oracleCommand.ExecuteReader();

            while (await oracleDataReader.ReadAsync())
            {
                _results.Add(await rowCallback.MapRow(oracleDataReader));
            }

        }

        private bool IsAtEndOfPage()
        {
            return CurrentItemCount % PageSize == 0;
        }

        class PagingOracleRowMapper : OracleRowMapper<T>
        {

            private readonly OraclePagingItemReader<T> _reader;

            public PagingOracleRowMapper(OraclePagingItemReader<T> reader)
            {
                this._reader = reader;
            }

            public Task<T> MapRow(OracleDataReader reader)
            {

                if (this._reader.RowMapper == null)
                    throw new ArgumentException("Row mapper cannot be null");

                return this._reader.RowMapper.MapRow(reader);
            }


        }
    }
}


