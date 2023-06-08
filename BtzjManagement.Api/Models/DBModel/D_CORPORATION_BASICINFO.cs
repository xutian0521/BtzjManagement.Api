using SqlSugar;
using System;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 单位基本信息
    /// </summary>
    [SugarTable("CORPORATION_BASICINFO")]
    public class D_CORPORATION_BASICINFO
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "CORPORATION_BASICINFO_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CUSTID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string DWMC { get; set; }
        /// <summary>
        /// 单位名称缩写
        /// </summary>
        public string DWMCSX { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        public string DWDZ { get; set; }
        /// <summary>
        /// 单位性质
        /// </summary>
        public string DWXZ { get; set; }
        /// <summary>
        /// 单位邮编
        /// </summary>
        public string DWYB { get; set; }
        /// <summary>
        /// 单位发薪日
        /// </summary>
        public string DWFXR { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string ZZJGDM { get; set; }
        /// <summary>
        /// 单位设立日期
        /// </summary>
        public DateTime? DWSLRQ { get; set; }
        /// <summary>
        /// 单位开户日期
        /// </summary>
        public DateTime? DWKHRQ { get; set; }
        /// <summary>
        /// 单位起缴日期
        /// </summary>
        public DateTime? DWQJRQ { get; set; }
        /// <summary>
        /// 基本存款户开户行
        /// </summary>
        public string BASICACCTBRCH { get; set; }
        /// <summary>
        /// 基本存款户账号
        /// </summary>
        public string BASICACCTNO { get; set; }
        /// <summary>
        /// 基本存款户名称
        /// </summary>
        public string BASICACCTMC { get; set; }
        /// <summary>
        /// 单位法人代表姓名
        /// </summary>
        public string DWFRDBXM { get; set; }
        /// <summary>
        /// 单位法人代表证件类型
        /// </summary>
        public string DWFRDBZJLX { get; set; }
        /// <summary>
        /// 单位法人代表证件号码
        /// </summary>
        public string DWFRDBZJHM { get; set; }
        /// <summary>
        /// 经办人姓名
        /// </summary>
        public string JBRXM { get; set; }
        /// <summary>
        /// 经办人固定电话号码
        /// </summary>
        public string JBRGDDHHM { get; set; }
        /// <summary>
        /// 经办人手机号码
        /// </summary>
        public string JBRSJHM { get; set; }
        /// <summary>
        /// 经办人证件类型
        /// </summary>
        public string JBRZJLX { get; set; }
        /// <summary>
        /// 经办人证件号码
        /// </summary>
        public string JBRZJHM { get; set; }
        /// <summary>
        /// 城市网点编号
        /// </summary>
        public string CITY_CENTNO { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string OPERID { get; set; }
    }
}

