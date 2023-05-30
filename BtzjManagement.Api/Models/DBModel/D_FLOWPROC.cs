using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 操作流程日志表
    /// </summary>
    [SugarTable("FLOWPROC")]
    public class D_FLOWPROC
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "FLOWPROC_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }
        /// <summary>
        /// 业务id
        /// </summary>
        public int YWID { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 流程名称-来自SYS_DATA_DICTIONARY表type_key为gjjopttype
        /// </summary>
        public string PROC_NAME { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? EXCEC_TIME { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public string EXCEC_MAN { get; set; }
        /// <summary>
        /// 状态-来自OptStatusConst
        /// </summary>
        public string STATUS { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }

    }
}
