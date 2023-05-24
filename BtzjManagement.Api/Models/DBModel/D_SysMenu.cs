
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtzjManagement.Api.Models.DBModel
{
    [SugarTable("SYS_MENU")]
    public class D_SYS_MENU
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SYS_MENU_SEQ")]
        public int ID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string ALIAS { get; set; }
        /// <summary>
        /// 父级节点id
        /// </summary>
        public int PID { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string PATH { get; set; }
        /// <summary>
        /// 菜单标识
        /// </summary>
        public string ICON { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SORT_ID { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public int IS_ENABLE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// 城市网点编号
        /// </summary>
        public string CITY_CENTNO { get; set; }
    }
}
