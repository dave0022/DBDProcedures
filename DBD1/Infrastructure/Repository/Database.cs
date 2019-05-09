using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;

namespace Infrastructure.Repository
{
	public class DB : IDisposable
	{
		
        private string cs;
        private IDbConnection DbConnection;
        public void Dispose()
        {
            DbConnection.Dispose();
        }
        
		public IEnumerable<t> ExcetueSP<t>(string name, DynamicParameters parameters = null)
		{
			var result = DbConnection.Query<t>(name, parameters, commandType: CommandType.StoredProcedure);
			return result;
        }

        
        public DB()
        {
            DbConnection = new SqlConnection(cs);
            cs = "Server=10.176.111.31; Database=CS2016B_8_Company;" +
                "Persist Security Info=False;User ID=CS2016B_8;" +
                "Password=CS2016B_8;" +
                "MultipleActiveResultSets = False; Encrypt = False;";

        }
        public string GetCS()
        {
            return cs;
        }
    }
}
