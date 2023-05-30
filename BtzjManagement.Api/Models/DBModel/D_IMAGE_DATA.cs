using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 影像数据表
    /// </summary>
    [SugarTable("IMAGE_DATA")]
    public class D_IMAGE_DATA
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "IMAGE_DATA_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 影像类型id
        /// </summary>
        public int MENU_ID { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }
        /// <summary>
        /// 影像路径
        /// </summary>
        public string PATH { get; set; }
    }
}
