using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 个人账户信息
    /// </summary>
    [SugarTable("CUSTOMER_ACCTINFO")]
    public class D_CUSTOMER_ACCTINFO
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "CUSTOMER_ACCTINFO_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CUSTID { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 个人账号
        /// </summary>
        public string GRZH { get; set; }
        /// <summary>
        /// 个人账户状态
        /// </summary>
        public string GRZHZT { get; set; }
        /// <summary>
        /// 开户日期
        /// </summary>
        public DateTime? KHRQ { get; set; }
        /// <summary>
        /// 封存日期
        /// </summary>
        public DateTime? LOCK_DATE { get; set; }
        /// <summary>
        /// 封存原因
        /// </summary>
        public string LOCK_REASON { get; set; }
        /// <summary>
        /// 启封日期
        /// </summary>
        public DateTime? UNLOCK_DATE { get; set; }
        /// <summary>
        /// 销户日期
        /// </summary>
        public DateTime? XHRQ { get; set; }
        /// <summary>
        /// 销户原因
        /// </summary>
        public string XHYY { get; set; }
        /// <summary>
        /// 账户类型 0：按月 1:一次性
        /// </summary>
        public string ACCT_TYPE { get; set; }
        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }
        /// <summary>
        /// 个人缴存比例
        /// </summary>
        public decimal GRJCBL { get; set; }
        /// <summary>
        /// 个人缴存基数
        /// </summary>
        public decimal GRJCJS { get; set; }
        /// <summary>
        /// 单位月缴存额
        /// </summary>
        public decimal DWYJCE { get; set; }
        /// <summary>
        /// 个人月缴存额
        /// </summary>
        public decimal GRYJCE { get; set; }
        /// <summary>
        /// 月汇缴额
        /// </summary>
        public decimal MONTHPAYAMT { get; set; }
        /// <summary>
        /// 末次汇缴月
        /// </summary>
        public DateTime? LASTPAYMONTH { get; set; }
        /// <summary>
        /// 起缴日期
        /// </summary>
        public DateTime? QJRQ { get; set; }
        /// <summary>
        /// 一次性补贴金额
        /// </summary>
        public decimal YCXBTJE { get; set; }
        /// <summary>
        ///  一次性缴存状态 0：未缴交 1：生成汇缴业务未记账 2：已复核
        /// </summary>
        public string YCX_CHECK_FLAG { get; set; }
        /// <summary>
        /// 个人账户余额
        /// </summary>
        public decimal GRZHYE { get; set; }
        /// <summary>
        /// 个人账户上年结转余额
        /// </summary>
        public decimal GRZHSNJZYE { get; set; }
        /// <summary>
        /// 个人账户上年结转日期
        /// </summary>
        public DateTime? GRZHSNJZRQ { get; set; }
        /// <summary>
        /// 个人账户当年归集余额
        /// </summary>
        public decimal GRZHDNGJYE { get; set; }
        /// <summary>
        /// 个人存款账户号码
        /// </summary>
        public string GRCKZHHM { get; set; }
        /// <summary>
        /// 个人存款账户开户名称
        /// </summary>
        public string GRCKZHKHMC { get; set; }
        /// <summary>
        /// 个人存款账户开户银行代码
        /// </summary>
        public string GRCKZHKHYHDM { get; set; }
        /// <summary>
        /// 核定标志
        /// </summary>
        public string CHECK_FLAG { get; set; }
        /// <summary>
        /// 日期戳
        /// </summary>
        public DateTime? LASTDEALDATE { get; set; }
        /// <summary>
        /// 城市网点
        /// </summary>
        public string CITY_CENTNO { get; set; }
    }
}