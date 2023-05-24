using System;
using System.Collections.Generic;

namespace BtzjManagement.Api.Models.ViewModel
{
    public class v_SysMenu
    {
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
        public List<v_SysMenu> Subs { get; set; } = new List<v_SysMenu>();
    }
}
