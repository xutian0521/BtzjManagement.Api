using System;

namespace BtzjManagement.Api.Models.QueryModel
{
    public class P_Remittance
    {


        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }

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
        /// <summary>
        /// 缴存月数
        /// </summary>
        public int monthCount { get; set; }
        /// <summary>
        /// 附件数量
        /// </summary>
        public int I_FJ_COUNT { get; set; }
    }
}
