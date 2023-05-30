using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [SugarTable("SYS_ROLE_MENU")]
    public class D_SYS_ROLE_MENU
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SYS_ROLE_MENU_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int ROLE_ID { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MENU_ID { get; set; }

        /// <summary>
        /// 是否有添加权限
        /// </summary>
        public int CAN_ADD { get; set; }

        /// <summary>
        /// 是否有编辑权限
        /// </summary>
        public int CAN_EDIT { get; set; }

        /// <summary>
        /// 是否有删除权限
        /// </summary>
        public int CAN_DELETE { get; set; }

        /// <summary>
        /// 是否有查看权限
        /// </summary>
        public int CAN_AUDIT { get; set; }
    }

}
