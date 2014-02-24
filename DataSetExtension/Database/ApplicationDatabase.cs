using System;
using System.Data;
using Dapper;

namespace DataSetExtension.Database
{
    public class ApplicationDatabase
    {
        public ApplicationDatabase(IDbConnection connection) 
        {
            Connection = connection;
        }
        
        protected IDbConnection Connection { get; set; }
        
        public void Compact()
        {
            Connection.Execute("vacuum;");
        }
    }
}