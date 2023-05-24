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
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.Xml.Linq;
using SqlSugar;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

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
            var query = SugarSimple.Instance().Queryable<D_SYS_MENU>();
            query = query.WhereIF(pId > 0, u => u.PID == pId);
            query = isFilterDisabledMenu ? query.Where(u => u.IS_ENABLE == 1) : query;

            
            var list = query.ToList();

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
                        ID = item.ID,
                        NAME = item.NAME,
                        ICON = item.ICON,
                        IS_ENABLE = item.IS_ENABLE,
                        PID = item.PID,
                        PATH = item.PATH,
                        SORT_ID = item.SORT_ID,
                        ALIAS = item.ALIAS,
                        REMARK = item.REMARK,
                        Subs = MenuTreeList(item.ID, isFilterDisabledMenu)
                    };
                    menuList.Add(menu);
                });
                return menuList;
            }
        }

        /// <summary>
        /// 添加或修改菜单
        /// </summary>
        public async Task<(bool, string)> AddOrModifyMenuAsync(int id, int pId,
            string title, string path, string icon, int sortId, bool isEnable, string remark, Guid userId)
        {
            var model = new D_SYS_MENU();
            model.PID = pId;
            model.NAME = title;
            model.PATH = path;
            model.ICON = icon;
            model.SORT_ID = sortId;
            model.IS_ENABLE = isEnable ? 1 : 0;
            model.REMARK = remark;

            if (id > 0) //修改
            {
                model.ID = id;
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



        //---------------------------------------------------用户信息-----------------------------------------------------
        /// <summary>
        /// 查询分页用户信息列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="roleId">权限id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public async Task<Pager<D_USER_INFO>> UserList(string userName, string roleId, int pageIndex = 1, int pageSize = 10)
        {
            var query = SugarSimple.Instance().Queryable<D_USER_INFO>();
            query = query.WhereIF(!string.IsNullOrEmpty(userName),u => u.NAME == userName);
            query = query.WhereIF(!string.IsNullOrEmpty(roleId) , u => u.RULE_ID == roleId);
            var totalCount = query.Count();

            var list = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pager = new Pager<D_USER_INFO>
            {
                list = list,
                total = totalCount
            };
            return pager;
        }

        public async Task<(int code, string message)> AddOrModify(int id, string userName,string passWord, string roleId, string realName, string remark)
        {
            if (id == 0)
            {
                // 新增操作
                var existingUser = await SugarSimple.Instance().Queryable<D_USER_INFO>().Where(u => u.NAME == userName).FirstAsync();
                if (existingUser != null)
                {
                    return (ApiResultCodeConst.ERROR, "用户已存在");
                }

                var newUser = new D_USER_INFO
                {
                    NAME = userName,
                    RULE_ID = roleId,
                    REAL_NAME = realName,
                    REMARK = remark,
                    CREATE_TIME = DateTime.Now
                };

                var insertResult = await SugarSimple.Instance().Insertable(newUser).ExecuteCommandAsync();
                if (insertResult > 0)
                {
                    return (ApiResultCodeConst.SUCCESS, "新增成功");
                }
                else
                {
                    return (ApiResultCodeConst.ERROR, "新增失败");
                }
            }
            else
            {
                // 修改操作
                var existingUser = await SugarSimple.Instance().Queryable<D_USER_INFO>().Where(u => u.ID == id).FirstAsync();
                if (existingUser == null)
                {
                    return (ApiResultCodeConst.ERROR, "用户不存在");
                }

                existingUser.NAME = userName;
                existingUser.RULE_ID = roleId;
                existingUser.REAL_NAME = realName;
                existingUser.REMARK = remark;

                var updateResult = await SugarSimple.Instance().Updateable(existingUser).ExecuteCommandAsync();
                if (updateResult > 0)
                {
                    return (ApiResultCodeConst.SUCCESS, "修改成功");
                }
                else
                {
                    return (ApiResultCodeConst.ERROR, "修改失败");
                }
            }
        }

        public async Task<(int code, string message)> DeleteUser(int id)
        {
            var existingUser = await SugarSimple.Instance().Queryable<D_USER_INFO>().Where(u => u.ID == id).FirstAsync();
            if (existingUser == null)
            {
                return (ApiResultCodeConst.ERROR, "用户不存在");
            }

            var deleteResult = await SugarSimple.Instance().Deleteable<D_USER_INFO>().In(id).ExecuteCommandAsync();
            if (deleteResult > 0)
            {
                return (ApiResultCodeConst.SUCCESS, "删除成功");
            }
            else
            {
                return (ApiResultCodeConst.ERROR, "删除失败");
            }
        }


    }
}
