using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 单位账户信息表
    /// </summary>
    [SugarTable("CORPORATION_ACCTINFO")]
    public class D_CORPORATION_ACCTINFO
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "CORPORATION_ACCTINFO_SEQ")]
        public string ID { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CUSTID { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 缴至年月
        /// </summary>
        public string JZNY { get; set; }
        /// <summary>
        /// 下次应缴日期
        /// </summary>
        public DateTime? NEXTPAYMTH { get; set; }
        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }
        /// <summary>
        /// 个人缴存比例
        /// </summary>
        public decimal GRJCBL { get; set; }
        /// <summary>
        /// 单位职工人数
        /// </summary>
        public int DWZGRS { get; set; }
        /// <summary>
        /// 单位缴存人数
        /// </summary>
        public int DWJCRS { get; set; }
        /// <summary>
        /// 单位封存人数
        /// </summary>
        public int DWFCRS { get; set; }
        /// <summary>
        /// 工资总数
        /// </summary>
        public decimal FACTINCOME { get; set; }
        /// <summary>
        /// 月缴存总额
        /// </summary>
        public decimal MONTHPAYTOTALAMT { get; set; }
        /// <summary>
        /// 单位账户状态
        /// </summary>
        public string DWZHZT { get; set; }
        /// <summary>
        /// 单位账户余额
        /// </summary>
        public decimal DWZHYE { get; set; }
        /// <summary>
        /// 挂账户余额
        /// </summary>
        public decimal REGHANDBAL { get; set; }
        /// <summary>
        /// 单位销户日期 
        /// </summary>
        public DateTime? DWXHRQ { get; set; }
        /// <summary>
        /// 单位销户原因
        /// </summary>
        public string DWXHYY { get; set; }
        /// <summary>
        /// 补贴资金来源
        /// </summary>
        public string FROMFLAG { get; set; }
        /// <summary>
        /// 员工户已开立标
        /// </summary>
        public string OPENPERSALREADY { get; set; }
        /// <summary>
        /// 受托银行名称
        /// </summary>
        public string STYHMC { get; set; }
        /// <summary>
        /// 受托银行代码
        /// </summary>
        public string STYHDM { get; set; }
        /// <summary>
        /// 受托银行账号
        /// </summary>
        public string STYHZH { get; set; }
        /// <summary>
        /// 计算方法
        /// </summary>
        public string CALC_METHOD { get; set; }
        /// <summary>
        /// 城市网点编号
        /// </summary>
        public string CITY_CENTNO { get; set; }
    }
}
























