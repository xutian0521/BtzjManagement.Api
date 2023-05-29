using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 单位业务信息主表
    /// </summary>
    [SugarTable("BUSI_CORPORATION")]
    public class D_BUSI_CORPORATION
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "BUSI_CORPORATION_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }
        /// <summary>
        /// 标识符号-单位账号/统一信用代码
        /// </summary>
        public string UNIQUE_KEY { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int BUSITYPE { get; set; }
        /// <summary>
        /// 保存时间
        /// </summary>
        public DateTime? CREATE_TIME { get; set; }
        /// <summary>
        /// 保存人
        /// </summary>
        public string CREATE_MAN { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? SUBMIT_TIME { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string SUBMIT_MAN { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? VERIFY_TIME { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string VERIFY_MAN { get; set; }
        /// <summary>
        /// 系统账务日期
        /// </summary>
        public DateTime SYSTEM_TIME { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public string STATUS { get; set; }
        /// <summary>
        /// 城市网点
        /// </summary>
        public string CITY_CENTNO { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }

    }
}
