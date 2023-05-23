using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 影像类型表
    /// </summary>
    [SugarTable("IMAGE_MENU")]
    public class D_IMAGE_MENU
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "IMAGE_MENU_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public int PID { get; set; }
        /// <summary>
        /// 影像类型-一级类型来自sys_enum表typekey为gjjopttype
        /// </summary>
        public int MENUTYPE { get; set; }
        /// <summary>
        /// 影像类型名称
        /// </summary>
        public string MENUNAME { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SORTID { get; set; }
        /// <summary>
        /// 是否启用(即前端能否获取到该节点) 0:不使用  1：使用
        /// </summary>
        public int ENABLED { get; set; }
        /// <summary>
        /// 是否必传(ENABLED为1时该字段才生效) 0:不是必传，1:是必传
        /// </summary>
        public int REQUIRED { get; set; }
        /// <summary>
        /// 城市网点编号
        /// </summary>
       public string CITY_CENTNO { get; set; }
    }
}
