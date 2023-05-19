using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 个人开户业务主表
    /// </summary>
    [SugarTable("GRKH")]
    public class D_GRKH
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "GRKH_SEQ")]
        public string ID { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string DWMC { get; set; }
        /// <summary>
        /// 开户类型(按月1，一次性2)
        /// </summary>
        public int KHTYPE { get; set; } = 1;
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
        /// 业务状态
        /// </summary>
        public string STATUS { get; set; }
        /// <summary>
        /// 城市网点
        /// </summary>
        public string CITY_CENTNO { get; set; }

    }
}
