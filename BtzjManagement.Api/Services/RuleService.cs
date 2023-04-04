using Dapper;
using Microsoft.Extensions.Configuration;
using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BtzjManagement.Api.Services
{
    public class RuleService
    {
        IConfiguration Configuration;
        public RuleService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="pId">菜单id, 取根目录下所有树菜单 传0</param>
        /// <param name="isFilterDisabledMenu">是否过滤掉禁用菜单</param>
        /// <returns></returns>
        public List<v_SysMenu> MenuTreeList(int pId, bool isFilterDisabledMenu)
        {
            string sql = "SELECT * from sys_menu ";
            string sql_orderBy = " ORDER BY menu_order ";
            string sql_where = "WHERE parent_id=:parent_id ";
            if (isFilterDisabledMenu)
            {
                sql_where += " AND IsEnable= 1 ";
            }
            DynamicParameters parameters = new DynamicParameters();

            string sqlAll = sql + sql_where + sql_orderBy;
            var list = OracleConnector.Conn().Query<D_SysMenu>(sqlAll, new { parent_id= pId }).ToList();
            var menuList = new List<v_SysMenu>();

            if (list.Count() <= 0)
            {
                return null;
            }
            else
            {
                list.ForEach(item =>
                {
                    var menu = new v_SysMenu()
                    {
                        menu_id = item.menu_id,
                        menu_name = item.menu_name,
                        menu_icon = item.menu_icon,
                        IsEnable = item.IsEnable,
                        parent_id = item.parent_id,
                        menu_url = item.menu_url,
                        menu_order = item.menu_order,
                        Subs = MenuTreeList(item.menu_id, isFilterDisabledMenu)
                    };
                    menuList.Add(menu);
                });
                return menuList;
            }
        }
    }
}
