using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.ViewModel
{
    /// <summary>
    /// 表数据量统计
    /// </summary>
    public class v_TableDataCount
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表数据量
        /// </summary>
        public int TableDataNumber { get; set; }
    }
}
