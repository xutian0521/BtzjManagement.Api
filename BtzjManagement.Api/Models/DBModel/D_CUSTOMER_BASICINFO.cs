using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 个人基本信息
    /// </summary>
    [SugarTable("CUSTOMER_BASICINFO")]
    public class D_CUSTOMER_BASICINFO
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "CUSTOMER_BASICINFO_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CUSTID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 姓名全拼
        /// </summary>
        public string XMQP { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZJLX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZJHM { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string XINGBIE { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime? CSNY { get; set; }

        /// <summary>
        /// 参加工作时间
        /// </summary>
        public DateTime? WORK_DATE { get; set; }
        /// <summary>
        /// 固定电话号码
        /// </summary>
        public string GDDHHM { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string SJHM { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string HYZK { get; set; }
        /// <summary>
        /// 配偶客户编号
        /// </summary>
        public string MATE_CUSTID { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        public string CREATE_MAN { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CREATE_TIME { get; set; }
        /// <summary>
        /// 最后修改用户
        /// </summary>
        public string UPDATE_MAN { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? UPDATE_TIME { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string OPER_ID { get; set; }
        /// <summary>
        /// 日期戳-有变化就更新
        /// </summary>
        public DateTime? LASTDEAL_DATE { get; set; }
        /// <summary>
        /// 城市网点
        /// </summary>
        public string CITY_CENTNO { get; set; }
    }
}
