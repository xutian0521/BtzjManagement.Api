using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.SyBaseModel
{
    /// <summary>
    /// 个人公积金信息
    /// </summary>
    public class SD_grgjjxx
    {
        private string cbbj { get; set; }
        private decimal? czgjj { get; set; }
        /// <summary>
        /// 一次性补贴金额
        /// </summary>
        private decimal? dc_btje { get; set; }
        private decimal? dc_dqjs { get; set; }
        private decimal? dc_j_czgjj { get; set; }
        private decimal? dc_gj_dwgjj { get; set; }
        private decimal? dc_gj_grgjj { get; set; }
        private decimal? dc_gj_gze { get; set; }
        /// <summary>
        /// 上年金额
        /// </summary>
        private decimal? dc_snje { get; set; }
        /// <summary>
        /// 上年计息积数
        /// </summary>
        private decimal? dc_snjxjs { get; set; }
        /// <summary>
        /// 按月补贴截至时间
        /// </summary>
        private DateTime? dt_aybt_jiezhi { get; set; }
        /// <summary>
        /// 开户日期
        /// </summary>
        private DateTime? dt_kaihu { get; set; }
        /// <summary>
        /// 起缴日期
        /// </summary>
        private DateTime? dt_qjrq { get; set; }
        /// <summary>
        /// 上年结转日期
        /// </summary>
        private DateTime? dt_snjzrq { get; set; }
        /// <summary>
        /// 参加工作时间
        /// </summary>
        private DateTime? dt_work { get; set; }
        /// <summary>
        /// 按月缴存单位缴存额
        /// </summary>
        private decimal? dwgjj { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        private string dwzh { get; set; }
        /// <summary>
        /// 按月汇缴封存标记
        /// </summary>
        private string fcbj { get; set; }

        private string gr_password { get; set; }
        /// <summary>
        /// 按月汇缴个人缴存额
        /// </summary>
        private decimal? grgjj { get; set; }
        /// <summary>
        /// 个人账号
        /// </summary>
        private string grzh { get; set; }
        /// <summary>
        /// 工资额
        /// </summary>
        private decimal? gze { get; set; }
        /// <summary>
        /// 补贴方式
        /// </summary>
        private string i_btfs { get; set; }
        private string i_can_dk { get; set; }
        /// <summary>
        /// 封存原因
        /// </summary>
        private string i_fc_status { get; set; }
        private string i_gj_cbbj { get; set; }
        private string i_gj_fc_status { get; set; }
        private string i_gj_fcbj { get; set; }
        /// <summary>
        /// 一次性缴存状态 0：未缴交 1：生成汇缴业务未记账 2：已复核
        /// </summary>
        private string i_jj_flag { get; set; }
        /// <summary>
        /// 支取标记
        /// </summary>
        private string i_zqbj { get; set; }
        private string printedline { get; set; }
        private string s_global_id { get; set; }
        /// <summary>
        /// 受理人
        /// </summary>
        private string s_shouliren { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        private string s_yh_grzh { get; set; }
        /// <summary>
        /// 下次日期
        /// </summary>
        private DateTime? xcrq { get; set; }
        /// <summary>
        /// 个人账号余额
        /// </summary>
        public decimal grzhye { get; set; }




        public string cbbj_gbk { get { return Common.Cp850ToGBK(cbbj); } }
        public decimal? czgjj_gbk { get { return czgjj; } }
        public decimal? dc_btje_gbk { get { return dc_btje; } }
        public decimal? dc_dqjs_gbk { get { return dc_dqjs; } }
        public decimal? dc_j_czgjj_gbk { get { return dc_j_czgjj; } }
        public decimal? dc_gj_dwgjj_gbk { get { return dc_gj_dwgjj; } }
        public decimal? dc_gj_grgjj_gbk { get { return dc_gj_grgjj; } }
        public decimal? dc_gj_gze_gbk { get { return dc_gj_gze; } }
        public decimal? dc_snje_gbk { get { return dc_snje; } }
        public decimal? dc_snjxjs_gbk { get { return dc_snjxjs; } }
        public DateTime? dt_aybt_jiezhi_gbk { get { return dt_aybt_jiezhi; } }
        public DateTime? dt_kaihu_gbk { get { return dt_kaihu; } }
        public DateTime? dt_qjrq_gbk { get { return dt_qjrq; } }
        public DateTime? dt_snjzrq_gbk { get { return dt_snjzrq; } }
        public DateTime? dt_work_gbk { get { return dt_work; } }
        public decimal? dwgjj_gbk { get { return dwgjj; } }
        public string dwzh_gbk { get { return Common.Cp850ToGBK(dwzh); } }
        public string fcbj_gbk { get { return Common.Cp850ToGBK(fcbj); } }
        public string gr_password_gbk { get { return Common.Cp850ToGBK(gr_password); } }
        public decimal? grgjj_gbk { get { return grgjj; } }
        public string grzh_gbk { get { return Common.Cp850ToGBK(grzh); } }
        public decimal? gze_gbk { get { return gze; } }
        public string i_btfs_gbk { get { return Common.Cp850ToGBK(i_btfs); } }
        public string i_can_dk_gbk { get { return Common.Cp850ToGBK(i_can_dk); } }
        public string i_fc_status_gbk { get { return Common.Cp850ToGBK(i_fc_status); } }
        public string i_gj_cbbj_gbk { get { return Common.Cp850ToGBK(i_gj_cbbj); } }
        public string i_gj_fc_status_gbk { get { return Common.Cp850ToGBK(i_gj_fc_status); } }
        public string i_gj_fcbj_gbk { get { return Common.Cp850ToGBK(i_gj_fcbj); } }
        public string i_jj_flag_gbk { get { return Common.Cp850ToGBK(i_jj_flag); } }
        public string i_zqbj_gbk { get { return Common.Cp850ToGBK(i_zqbj); } }
        public string printedline_gbk { get { return Common.Cp850ToGBK(printedline); } }
        public string s_global_id_gbk { get { return Common.Cp850ToGBK(s_global_id); } }
        public string s_shouliren_gbk { get { return Common.Cp850ToGBK(s_shouliren); } }
        public string s_yh_grzh_gbk { get { return Common.Cp850ToGBK(s_yh_grzh); } }
        public DateTime? xcrq_gbk { get { return xcrq; } }
    }
}

//ID
//CUSTID 客户编号_1	VARCHAR2(120)
//DWZH 单位账号	VARCHAR2(20)
//GRZH 个人账号	VARCHAR2(20)
//GRZHZT 个人账户状态	VARCHAR2(2)
//KHRQ 开户日期	DATE

//LOCKDATE	封存日期
//UNLOCKDATE	启封日期
//XHRQ 销户日期	DATE
//XHYY 销户原因	VARCHAR2(2)
//ACCTTYPE 账户类型
//DWJCBL	单位缴存率1
//GRJCBL	个人缴存比例
//GRJCJS 个人缴存基数
//DWYJCE	单位月缴存额
//GRYJCE	个人月缴存额
//MONTHPAYAMT	月汇缴额
//LASTPAYMONTH	末次汇缴月
//GRZHYE 个人账户余额	NUMBER(18,2)

//BANKTYPE 银行类型_1	VARCHAR2(4)
//BANKID 银行机构编号	VARCHAR2(9)
//GRCKZHHM 个人存款账户号码	VARCHAR2(30)


//CHECKFLAG 核定标志

//GRZHSNJZYE 个人账户上年结转余额	NUMBER(18,2)
//GRZHDNGJYE 个人账户当年归集余额	NUMBER(18,2)

//LASTDEALDATE	日期戳_1
//CITY_CENTNO 城市网点	        VARCHAR2(10)




