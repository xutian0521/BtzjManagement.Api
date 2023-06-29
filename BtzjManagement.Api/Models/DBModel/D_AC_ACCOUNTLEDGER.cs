using SqlSugar;
using System;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 总账信息表
    /// </summary>
    [SugarTable("AC_ACCOUNTLEDGER")]
    public class D_AC_ACCOUNTLEDGER
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "AC_ACCOUNTLEDGER_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 科目编号
        /// </summary>
        public string KMBH { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string KMMC { get; set; }

        /// <summary>
        /// 期初余额
        /// </summary>
        public decimal QCYE { get; set; }

        /// <summary>
        /// 期初余额方向
        /// </summary>
        public string QCYEFX { get; set; }

        /// <summary>
        /// 借方发生额
        /// </summary>
        public decimal JFFSE { get; set; }

        /// <summary>
        /// 贷方发生额
        /// </summary>
        public decimal DFFSE { get; set; }

        /// <summary>
        /// 期末余额
        /// </summary>
        public decimal QMYE { get; set; }

        /// <summary>
        /// 期末余额方向
        /// </summary>
        public string QMYEFX { get; set; }

        /// <summary>
        /// 记账日期
        /// </summary>
        public DateTime JZRQ { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string ZHAIYAO { get; set; }
    }

}
