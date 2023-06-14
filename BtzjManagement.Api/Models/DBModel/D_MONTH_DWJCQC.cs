using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 按月汇缴清册表
    /// </summary>
    [SugarTable("MONTH_DWJCQC")]
    public class D_MONTH_DWJCQC
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "MONTH_DWJCQC_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }

        /// <summary>
        /// 汇缴月份
        /// </summary>
        public string PAYMTH { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZJHM { get; set; }

        /// <summary>
        /// 个人姓名
        /// </summary>
        public string XINGMING { get; set; }

        /// <summary>
        /// 个人账号
        /// </summary>
        public string GRZH { get; set; }

        /// <summary>
        /// 个人缴存基数
        /// </summary>
        public decimal GRJCJS { get; set; }

        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }

        /// <summary>
        /// 个人缴存比例
        /// </summary>
        public decimal GRJCBL { get; set; }

        /// <summary>
        /// 汇缴金额
        /// </summary>
        public decimal REMITPAYAMT { get; set; }

        /// <summary>
        /// 单位月缴存额
        /// </summary>
        public decimal DWYJCE { get; set; }

        /// <summary>
        /// 个人月缴存额
        /// </summary>
        public decimal GRYJCE { get; set; }
    }
}
