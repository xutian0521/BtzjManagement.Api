﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 个人开户业务明细表
    /// </summary>
    [SugarTable("GRKH_ITEM")]
    public class D_GRKH_ITEM
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "GRKH_ITEM_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 业务月度
        /// </summary>
        public string YWYD { get; set; }
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
        /// 出生日期
        /// </summary>
        public DateTime? CSNY { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string SJHM { get; set; }
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
        /// 个人月缴存额
        /// </summary>
        public decimal GRYJCE { get; set; }
        /// <summary>
        /// 单位月缴存额
        /// </summary>
        public decimal DWYJCE { get; set; }
        /// <summary>
        /// 起缴日期
        /// </summary>
        public DateTime? QJRQ { get; set; }
        /// <summary>
        /// 个人账户状态
        /// </summary>
        public string GRZHZT { get; set; }
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
        /// 工作时间
        /// </summary>
        public DateTime? WORK_DATE { get; set; }
        /// <summary>
        /// 固定电话号码
        /// </summary>
        public string GDDHHM { get; set; }
    }
}














