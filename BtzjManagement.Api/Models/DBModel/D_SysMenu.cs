
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtzjManagement.Api.Models.DBModel
{
    [SugarTable("SYS_MENU")]
    public class D_SysMenu
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SYS_MENU_SEQ")]
        public int Id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 父级节点id
        /// </summary>
        public int PId { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 菜单标识
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortId { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 城市网点编号
        /// </summary>
        public string CITY_CENTNO { get; set; }
    }
}
