using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BtzjManagement.Api.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SybaseService
    {
        /// <summary>
        /// 获取原sybase数据库的表及其数据量
        /// </summary>
        /// <returns>表数据量列表</returns>
        public async Task<List<v_TableDataCount>> AccquireSybaseTableDataCount()
        {
            List<v_TableDataCount> result = new List<v_TableDataCount>();
            using (var conn = SybaseConnector.Conn())
            {
                var sql = @" select name TableName from sysobjects where type = 'U' ";//获取用户表名
                var tableNameList = (await conn.QueryAsync<v_TableDataCount>(sql)).OrderBy(x => x.TableName);

                StringBuilder sqlCount = new StringBuilder();
                bool first = true;
                foreach (var table in tableNameList)
                {
                    if (first)
                    {
                        sqlCount.Append($" select '{table.TableName}' TableName,count(1) TableDataNumber from {table.TableName} ");
                    }
                    else
                    {
                        sqlCount.Append($" union all ");
                        sqlCount.Append($" select '{table.TableName}' TableName,count(1) TableDataNumber from {table.TableName} ");
                    }

                    first = false;
                }

                if (sqlCount.Length > 0)
                {
                    result = (await conn.QueryAsync<v_TableDataCount>(sqlCount.ToString())).OrderBy(x => x.TableName).ToList();
                }
            }
            return result;
        }
       
    }
}
