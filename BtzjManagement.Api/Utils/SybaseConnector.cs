using AdoNetCore.AseClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Utils
{
    public class SybaseConnector
    {
        public static string connectionString { get; set; }

        private SybaseConnector()
        {
        }

        public static IDbConnection Conn()
        {
            return new AseConnection(connectionString);
        }
    }
}
