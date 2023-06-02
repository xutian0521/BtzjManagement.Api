using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
       /// <summary>
       /// 单位基本信息
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
        public string CREATE_MAN { get; set; }
        public string CREATE_TIME { get; set; }
        public string UPDATE_MAN { get; set; }
        public string UPDATE_TIME { get; set; }
        public string OPER_ID { get; set; }
    }
}






//CREATEUSER 创建用户	VARCHAR2(40)
//CREATETIME 创建时间_2	TIMESTAMP(6)

//UPDATEUSER 最后修改用户	VARCHAR2(40)
//UPDATETIME 最后修改时间_2	TIMESTAMP(6)
//OPERID 操作员	VARCHAR2(80)
//LASTDEALDATE 日期戳_1	TIMESTAMP(6)
//CITY_CENTNO 城市网点	        VARCHAR2(10)