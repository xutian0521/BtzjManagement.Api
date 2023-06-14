using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.QueryModel
{
    /// <summary>
    /// 按月汇缴暂存
    /// </summary>
    public class P_MonthHjCreate
    {
        /// <summary>
        /// 汇缴起始月
        /// </summary>
        public string monthStart { get; set; }
        /// <summary>
        /// 汇缴月数
        /// </summary>
        public int monthLength { get; set; }
        /// <summary>
        /// 汇缴金额
        /// </summary>
        public decimal payamt { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string dwzh { get; set; }
    }
}
