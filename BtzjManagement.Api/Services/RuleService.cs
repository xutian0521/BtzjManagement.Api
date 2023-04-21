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
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace BtzjManagement.Api.Services
{
    public class RuleService
    {
        IConfiguration Configuration;
        public RuleService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //---------------------------------------------------菜单-----------------------------------------------------
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="pId">菜单id, 取根目录下所有树菜单 传0</param>
        /// <param name="isFilterDisabledMenu">是否过滤掉禁用菜单</param>
        /// <returns></returns>
        public List<v_SysMenu> MenuTreeList(int pId, bool isFilterDisabledMenu)
        {
            string sql = "SELECT * from sys_menu ";
            string sql_orderBy = " ORDER BY sortid ";
            string sql_where = "WHERE pid=:pid ";
            if (isFilterDisabledMenu)
            {
                sql_where += " AND IsEnable= 1 ";
            }
            DynamicParameters parameters = new DynamicParameters();

            string sqlAll = sql + sql_where + sql_orderBy;
            var list = OracleConnector.Conn().Query<D_SysMenu>(sqlAll, new { pid = pId }).ToList();
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
                        Id = item.Id,
                        Name = item.Name,
                        Icon = item.Icon,
                        IsEnable = item.IsEnable,
                        PId = item.PId,
                        Path = item.Path,
                        SortId = item.SortId,
                        Alias = item.Alias,
                        Remark = item.Remark,
                        Subs = MenuTreeList(item.Id, isFilterDisabledMenu)
                    };
                    menuList.Add(menu);
                });
                return menuList;
            }
        }

        /// <summary>
        /// 添加或修改菜单
        /// </summary>
        public async Task<(bool, string)> AddOrModifyMenuAsync(int id, int pId, string title,
            string path, string icon, int sortId, bool isEnable, string remark, Guid userId)
        {
            var model = new D_SysMenu();
            model.PId = pId;
            model.Name = title;
            model.Path = path;
            model.Icon = icon;
            model.SortId = sortId;
            model.IsEnable = isEnable ? 1 : 0;
            model.Remark = remark;

            if (id > 0) //修改
            {
                model.Id = id;
                var b = await OracleConnector.Conn().UpdateAsync(model);
                //await _sysLogService.RecordActionSysLogObj(SysLogActionConst.修改菜单, userId, null, model);
                return b ? (true, "修改成功！") : (false, "修改失败!");
            }
            else
            {
                var _id = await OracleConnector.Conn().InsertAsync(model);
                //await _sysLogService.RecordActionSysLogObj(SysLogActionConst.添加菜单, userId, null, model);
                return _id > -1 ? (true, "添加成功！") : (false, "添加失败!");
            }
        }
    }
}
