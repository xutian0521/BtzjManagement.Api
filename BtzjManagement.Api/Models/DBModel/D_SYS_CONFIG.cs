using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    [SugarTable("SYS_CONFIG")]
    public class D_SYS_CONFIG
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SYS_CONFIG_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 计算方法
        /// </summary>
        public string CALC_METHOD { get; set; }
        /// <summary>
        /// 最小
        /// </summary>
        public decimal MIN_JJ { get; set; }
        /// <summary>
        /// 上年结转利息
        /// </summary>
        public decimal SNJZ_LIXI { get; set; }
        /// <summary>
        /// 今年汇缴利息
        /// </summary>
        public decimal JNHJ_LIXI { get; set; }
        /// <summary>
        /// 系统开始时间
        /// </summary>
        public DateTime DT_CREATE { get; set; }
        /// <summary>
        /// 系统账户日期
        /// </summary>
        public DateTime DT_SYSTEM { get; set; }
        /// <summary>
        /// 系统标志
        /// </summary>
        public string SYS_FLAG { get; set; }
        /// <summary>
        /// 贷款月利率
        /// </summary>
        public decimal DKYLL { get; set; }
        /// <summary>
        /// 城市网点
        /// </summary>
        public string CITY_CENTNO { get; set; }
    }
}
