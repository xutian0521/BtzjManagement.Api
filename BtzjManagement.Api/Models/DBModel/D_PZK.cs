using SqlSugar;
using System;

namespace BtzjManagement.Api.Models.DBModel
{
    [SugarTable("PZK", "单位汇缴_借款凭证库表")]
    public class D_PZK
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "PZK_SEQ")]
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
        /// 摘要
        /// </summary>
        public string S_ZY { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime DT_RQ { get; set; }

        /// <summary>
        /// 借款凭证号
        /// </summary>
        public string JZPZH { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int I_FLH { get; set; }

        /// <summary>
        /// 借方金额
        /// </summary>
        public decimal DC_JFJE { get; set; }

        /// <summary>
        /// 贷方金额
        /// </summary>
        public decimal DC_DFJE { get; set; }

        /// <summary>
        /// 制单
        /// </summary>
        public string S_ZD { get; set; }

        /// <summary>
        /// 复核
        /// </summary>
        public string S_FH { get; set; }

        /// <summary>
        /// 记账
        /// </summary>
        public string S_JZ { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int I_OP { get; set; }

        /// <summary>
        /// 复核标记
        /// </summary>
        public int I_RECHECKED { get; set; }

        /// <summary>
        /// 记账日期
        /// </summary>
        public DateTime JZRQ { get; set; }
    }

}
