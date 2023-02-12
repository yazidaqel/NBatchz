using System;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;

namespace NBatchzInfrastructure.Database.Support
{
	public class OracleWriter<O> : ItemStreamWriter<O>
	{

        public string? Sql { get; set; }

        private OracleConnection? OracleConnection { get; set; }

        public OracleWriter()
		{
		}

        public Task close()
        {
            return Task.Run(() =>
            {

            });
        }

        public Task open()
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(Sql))
                    throw new ArgumentException("Sql query cannot be null");

                if (OracleConnection == null || !OracleConnection.IsAvailable)
                    throw new ArgumentException("Oracle connection cannot be null, neither not available");

                
            });
        }


        public Task write(List<O> output)
        {
            return Task.Run(() =>
            {

                if (output.Any())
                {
                    
                    OracleCommand oracleCommand = new OracleCommand()
                    {
                        CommandText = Sql,
                        Connection = OracleConnection


                    };

                    output.ForEach(async item =>
                    {
                        Dictionary<string, OracleParameter> oracleParameters = new Dictionary<string, OracleParameter>();

                        foreach (PropertyInfo prop in item.GetType().GetProperties())
                        {
                            OracleParameter oracleParameter = new OracleParameter()
                            {
                                Value = prop.GetValue(output, null)
                            };
                            oracleParameters.Add(prop.Name, oracleParameter);
                            oracleCommand.Parameters.Add(oracleParameter);

                        }


                        await oracleCommand.ExecuteNonQueryAsync();
                    });

                    
                }


            });
        }
    }
}

