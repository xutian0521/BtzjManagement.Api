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
using System.Security.Cryptography;

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
            query = query.Where(u => u.PID == pId);
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
                var updateResult = await SugarSimple.Instance().Updateable(model).ExecuteCommandAsync();
                return updateResult > 0 ? (true, "修改成功！") : (false, "修改失败!");
            }
            else
            {
                var insertResult = await SugarSimple.Instance().Insertable(model).ExecuteCommandAsync();
                return insertResult > 0 ? (true, "添加成功！") : (false, "添加失败!");
            }
        }

        /// <summary>
        /// 获取父级菜单枚举
        /// </summary>
        public async Task<List<D_SYS_MENU>> ParentMenuEnums()
        {
            var query = SugarSimple.Instance().Queryable<D_SYS_MENU>();
            query = query.Where( u => u.PID == 0);
            var list = await query.ToListAsync();
            return list.ToList();
        }

        /// <summary>
        /// 载入修改菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<D_SYS_MENU> LoadModifyMenu(int id)
        {
            var query = SugarSimple.Instance().Queryable<D_SYS_MENU>();
            var one = await query.Where(u => u.ID == id).FirstAsync();
            return one;
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

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="roleId">角色id</param>
        /// <param name="realName">真实姓名</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public async Task<(int code, string message)> AddOrModifyUser(int id, string userName,string passWord, string roleId, string realName, string remark)
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

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户的id</param>
        /// <returns></returns>
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

        /// <summary>
        /// 载入修改用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户信息或不存在信息</returns>
        public async Task<(int code, string message, D_USER_INFO user)> LoadModifyUserInfoAsync(string id)
        {
            // 根据ID查询用户信息
            var user = await SugarSimple.Instance().Queryable<D_USER_INFO>().Where(u => u.ID == int.Parse(id)).FirstAsync();

            if (user != null)
            {
                // 返回存在用户信息
                return (code: 1, message: "用户信息存在", user: user);
            }
            else
            {
                // 返回不存在用户信息
                return (code: 0, message: "用户信息不存在", user: null);
            }
        }

        //---------------------------------------------------角色-----------------------------------------------------

        public async Task<(int code, string message, D_SYS_ROLE role)> LoadModifyRoleInfo(int id)
        {
            try
            {
                // 根据角色ID查询角色信息
                var role = await SugarSimple.Instance().Queryable<D_SYS_ROLE>().Where(r => r.ID == id).FirstAsync();

                if (role != null)
                {
                    // 角色存在，返回角色信息
                    return (code: 1, message: "success", role: role);
                }
                else
                {
                    // 角色不存在，返回不存在角色信息
                    return (code: 0, message: "角色不存在", role: null);
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                return (code: -1, message: ex.Message, role: null);
            }
        }

        /// <summary>
        /// 新增或修改角色
        /// </summary>
        /// <returns></returns>
        public async Task<(int code, string message)> AddOrModifyRoleAsync(int id, string roleName, string remark)
        {
            try
            {
                // 根据角色ID查询角色信息
                var role = await SugarSimple.Instance().Queryable<D_SYS_ROLE>().Where(r => r.ID == id).FirstAsync();

                if (role != null)
                {
                    // 角色存在，进行修改操作
                    role.ROLE_NAME = roleName;
                    role.REMARK = remark;

                    // 更新角色信息
                    await SugarSimple.Instance().Updateable(role).ExecuteCommandAsync();

                    return (code: 1, message: "角色修改成功");
                }
                else
                {
                    // 角色不存在，进行新增操作
                    role = new D_SYS_ROLE
                    {
                        ID = id,
                        ROLE_NAME = roleName,
                        REMARK = remark
                    };

                    // 插入角色信息
                    await SugarSimple.Instance().Insertable(role).ExecuteCommandAsync();

                    return (code: 1, message: "角色新增成功");
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                return (code: -1, message: ex.Message);
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        public async Task<(int code, string message)> DeleteRoleAsync(int id)
        {
            try
            {
                // 根据角色ID查询角色信息
                var role = await SugarSimple.Instance().Queryable<D_SYS_ROLE>().Where(r => r.ID == id).FirstAsync();

                if (role != null)
                {
                    // 角色存在，进行删除操作
                    await SugarSimple.Instance().Deleteable<D_SYS_ROLE>().Where(r => r.ID == id).ExecuteCommandAsync();

                    return (code: 1, message: "角色删除成功");
                }
                else
                {
                    // 角色不存在
                    return (code: 0, message: "角色不存在");
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                return (code: -1, message: ex.Message);
            }
        }


        /// <summary>
        /// 角色列表
        /// </summary>
        public async Task<(int code, string message, Pager<D_SYS_ROLE> list)> RoleListAsync(string roleName, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var query = SugarSimple.Instance().Queryable<D_SYS_ROLE>();

                if (!string.IsNullOrEmpty(roleName))
                {
                    // 根据角色名称筛选
                    query = query.Where(r => r.ROLE_NAME.Contains(roleName));
                }

                // 查询总记录数
                var totalCount = await query.CountAsync();

                // 分页查询角色列表
                var list = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                var pager = new Pager<D_SYS_ROLE>
                {
                    list = list,
                    total = totalCount
                };

                return (code: 1, message: "success", list: pager);
            }
            catch (Exception ex)
            {
                // 处理异常
                return (code: -1, message: ex.Message, list: null);
            }
        }
    }
}
