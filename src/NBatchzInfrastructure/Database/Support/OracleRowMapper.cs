using System;
using Oracle.ManagedDataAccess.Client;

namespace NBatchzInfrastructure.Database.Support
{
	public interface OracleRowMapper<T>
	{
		public Task<T> MapRow(OracleDataReader reader);
	}
}

