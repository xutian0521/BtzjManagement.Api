using SqlSugar;
using System;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 单位汇缴主表
    /// </summary>
    [SugarTable("OPERATION_GJJ_ENTER", "单位汇缴主表")]
    public class D_OPERATION_GJJ_ENTER
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "OPERATION_GJJ_ENTER_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 记账凭证号
        /// </summary>
        [SugarColumn( OracleSequenceName = "OPERATION_GJJ_ENTER_SEQ")]
        public string JZPZH { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string ZHAIYAO { get; set; }

        /// <summary>
        /// 科目编号
        /// </summary>
        public string KMBH { get; set; }

        /// <summary>
        /// 借方发生额
        /// </summary>
        public decimal JFFSE { get; set; }

        /// <summary>
        /// 贷方发生额
        /// </summary>
        public decimal DFFSE { get; set; }

        /// <summary>
        /// 附件单据数
        /// </summary>
        public decimal FJDJS { get; set; }

        /// <summary>
        /// 记账日期
        /// </summary>
        public DateTime JZRQ { get; set; }

        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }

        /// <summary>
        /// 发生日期
        /// </summary>
        public DateTime FSRQ { get; set; }

        /// <summary>
        /// 汇缴月份
        /// </summary>
        public int HJYF { get; set; }

        /// <summary>
        /// 汇缴年份
        /// </summary>
        public int I_HJ_YEAR { get; set; }

        /// <summary>
        /// 汇缴月份
        /// </summary>
        public int I_HJ_MONTH { get; set; }

        /// <summary>
        /// 汇缴方式
        /// </summary>
        public int I_HJFS { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string S_USERNAME { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal DC_HJJE { get; set; }
    }

}
