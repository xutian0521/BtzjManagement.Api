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
using System.Configuration;
using System.Reflection;

namespace BtzjManagement.Api.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RuleService
    {
        IConfiguration _configuration;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        public RuleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //---------------------------------------------------菜单-----------------------------------------------------
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="pId">菜单id, 取根目录下所有树菜单 传0</param>
        /// <param name="isFilterDisabledMenu">是否过滤掉禁用菜单</param>
        /// <returns></returns>
        public List<v_SysMenu> MenuTreeList(int roleId, int pId, bool isFilterDisabledMenu)
        {
            var query = SugarSimple.Instance().Queryable<D_SYS_MENU, D_SYS_ROLE_MENU>((m, rm) => new object[] { 
                JoinType.Left, m.ID == rm.ROLE_ID  }).Select((m, rm) => new D_SYS_MENU()
                { ID =m.ID, NAME = m.NAME, ICON = m.ICON, PID = m.PID, PATH = m.PATH, ALIAS = m.ALIAS, IS_ENABLE = m.IS_ENABLE, SORT_ID = m.SORT_ID, REMARK = m.REMARK});
            query = query.Where(m => m.PID == pId && m.ID == roleId);
            query = isFilterDisabledMenu ? query.Where(m => m.IS_ENABLE == 1) : query;

            
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
                        Subs = MenuTreeList(roleId, item.ID, isFilterDisabledMenu)
                    };
                    menuList.Add(menu);
                });
                return menuList;
            }
        }

        /// <summary>
        /// 添加或修改菜单
        /// </summary>
        public async Task<(int code, string message)> AddOrModifyMenuAsync(int id, int pId,
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
                return updateResult > 0 ? (ApiResultCodeConst.SUCCESS, "修改成功！") : (ApiResultCodeConst.ERROR, "修改失败!");
            }
            else
            {
                var insertResult = await SugarSimple.Instance().Insertable(model).ExecuteCommandAsync();
                return insertResult > 0 ? (ApiResultCodeConst.SUCCESS, "添加成功！") : (ApiResultCodeConst.ERROR, "添加失败!");
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
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(int code, string message)> DeleteMenuAsync(int id)
        {
            var existingMenu = await SugarSimple.Instance().Queryable<D_SYS_MENU>().Where(u => u.ID == id).FirstAsync();
            if (existingMenu == null)
            {
                return (ApiResultCodeConst.ERROR, "菜单不存在");
            }
            //说明是父级菜单
            if (existingMenu.PID == 0)
            {
                //删除父级菜单下的所有子菜单

                await SugarSimple.Instance().Deleteable<D_SYS_MENU>().Where(x => x.PID == id).ExecuteCommandAsync();
            }
            var deleteResult = await SugarSimple.Instance().Deleteable<D_SYS_MENU>().In(id).ExecuteCommandAsync();
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
        /// 设置角色菜单
        /// </summary>
        /// <returns></returns>
        public async Task<(int code, string message)> SettingRoleMenuAsync(int roleId, List<int> menuIds)
        {
            int insertedRow = 0;
            int deletedRow = 0;
            var DbContext = SugarSimple.Instance();
            try
            {

                await DbContext.BeginTranAsync();
                List<D_SYS_ROLE_MENU> addList = new List<D_SYS_ROLE_MENU>();
                List<D_SYS_ROLE_MENU> deleteList = new List<D_SYS_ROLE_MENU>();
                var list = await DbContext.Queryable<D_SYS_ROLE_MENU>().Where(x => x.ROLE_ID == roleId).ToListAsync();
                foreach (var item in menuIds)
                {
                    if (!list.Any(x => x.MENU_ID == item))
                    {

                        addList.Add(
                            new D_SYS_ROLE_MENU
                            {
                                MENU_ID = item,
                                CAN_ADD = 1,
                                CAN_AUDIT = 1,
                                CAN_EDIT = 1,
                                CAN_DELETE = 1,
                                ROLE_ID = roleId
                            });
                    }
                }
                foreach (var item in list)
                {
                    if (!menuIds.Any(x => x == item.MENU_ID))
                    {
                        deleteList.Add(new D_SYS_ROLE_MENU { ID = item.ID });
                    }
                }
                insertedRow = await DbContext.Insertable(addList).ExecuteCommandAsync();
                deletedRow = await DbContext.Deleteable(deleteList).ExecuteCommandAsync();
                await DbContext.CommitTranAsync();
            }
            catch (Exception ex)
            {
                DbContext.RollbackTran();
                throw new Exception(ex.Message);
            }
            finally
            {
                DbContext.Dispose();
            }


            return insertedRow > 0 || deletedRow > 0  ? (ApiResultCodeConst.SUCCESS , "设置成功！") : (ApiResultCodeConst.ERROR, "设置失败!");
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

        //---------------------------------------------------字典-----------------------------------------------------

        /// <summary>
        /// 根据枚举类型获取枚举列表-主要用于前端下拉列表渲染
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<v_SYS_DATA_DICTIONARY> GetDataDictionaryListByType(string type)
        {
            return SugarSimple.Instance().Queryable<D_SYS_DATA_DICTIONARY>().Where(x => x.TYPE_KEY == type.Trim()).Select(x => new v_SYS_DATA_DICTIONARY
            {
                LABEL = x.LABEL,
                TYPE_KEY = x.TYPE_KEY,
                VAL = x.VAL
            }).ToList();
        }

        /// <summary>
        /// 字典列表递归
        /// </summary>
        /// <param name="pId">字典id, 取根目录下所有树菜单 传0</param>
        /// <returns></returns>
        public List<v_SYS_DATA_DICTIONARY> DataDictionaryTreeList(int pId)
        {
            var list = SugarSimple.Instance().Queryable<D_SYS_DATA_DICTIONARY>().Where(x => x.PARENT_ID == pId).OrderBy(x => x.SORT_ID).ToList();
            var enumList = new List<v_SYS_DATA_DICTIONARY>();

            if (list.Count() <= 0)
            {
                return enumList;
            }
            else
            {
                list.ForEach(item =>
                {
                    var menu = new v_SYS_DATA_DICTIONARY()
                    {
                        ID = item.ID,
                        PARENT_ID = item.PARENT_ID,
                        LABEL = item.LABEL,
                        TYPE_KEY = item.TYPE_KEY,
                        VAL = item.VAL,
                        DESCRIPTION = item.DESCRIPTION,
                        SORT_ID = item.SORT_ID,
                        Subs = DataDictionaryTreeList(item.ID)
                    };
                    enumList.Add(menu);
                });
                return enumList;
            }
        }
        /// <summary>
        /// 枚举字典类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<D_SYS_DATA_DICTIONARY>> GetDataDictionaryListByParent()
        {
            var list = await SugarSimple.Instance().Queryable<D_SYS_DATA_DICTIONARY>().Where(x => x.PARENT_ID == 0).OrderBy(x => x.SORT_ID).ToListAsync();

            return list;
        }

        /// <summary>
        /// 载入修改枚举字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(int code, string message, v_SYS_DATA_DICTIONARY @enum) > LoadModifyEnumInfoById(int id)
        {
            var model = await SugarSimple.Instance().Queryable<D_SYS_DATA_DICTIONARY, D_SYS_DATA_DICTIONARY>((d1, d2) => new object[] {
                JoinType.Left, d1.PARENT_ID == d1.ID}).Where((d1, d2) => d1.ID == id)
                 .Select((d1, d2) => new v_SYS_DATA_DICTIONARY
                 {
                     ID = d1.ID,
                     DESCRIPTION = d1.DESCRIPTION,
                     LABEL = d1.LABEL,
                     PARENT_ID = d1.PARENT_ID,
                     SORT_ID = d1.SORT_ID,
                     TYPE_KEY = d1.TYPE_KEY,
                     PARENT_TYPE_KEY = d2.TYPE_KEY,
                     VAL = d1.VAL
                 }).FirstAsync();
            if (model == null)
            {
                return (ApiResultCodeConst.ERROR, "字典不存在", null);
            }
            return (ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, model);
        }

        /// <summary>
        /// 新增或修改枚举字典
        /// </summary>
        /// <returns></returns>
        public async Task<(int code, string message)> AddOrModifyEnum(int id, string TYPEKEY,
            string LABEL, int PARENTID, string VAL, string DESCRIPTION, int SORT)
        {
            D_SYS_DATA_DICTIONARY model = null;
            model = await SugarSimple.Instance().Queryable<D_SYS_DATA_DICTIONARY>().Where(u => u.ID == id).FirstAsync();
            if (model == null)
            {
                model = new D_SYS_DATA_DICTIONARY();
            }
            model.ID = id;
            model.TYPE_KEY = TYPEKEY;
            model.LABEL = LABEL;
            model.PARENT_ID = PARENTID;
            model.DESCRIPTION = DESCRIPTION;
            model.SORT_ID = SORT;
            model.VAL = VAL;

            int insertResult = 0;
            int updateResult = 0;
            if (id > 0) //修改
            {
                updateResult = await SugarSimple.Instance().Insertable(model).ExecuteCommandAsync();
            }
            else //新增
            {
                updateResult = await SugarSimple.Instance().Updateable(model).ExecuteCommandAsync();
            }

            return insertResult > 0 || updateResult > 0 ? (ApiResultCodeConst.SUCCESS, "操作成功") : (ApiResultCodeConst.ERROR, "操作失败");

        }

        /// <summary>
        /// 根据id删除枚举
        /// </summary>
        /// <param name="id">枚举id</param>
        /// <returns></returns>
        public async Task<(int code, string message)> DeleteEnumAsync(int id)
        {
            var existingUser = await SugarSimple.Instance().Queryable<D_SYS_DATA_DICTIONARY>().Where(u => u.ID == id).FirstAsync();
            if (existingUser == null)
            {
                return (ApiResultCodeConst.ERROR, "字段不存在");
            }
            var deleteResult = await SugarSimple.Instance().Deleteable<D_SYS_DATA_DICTIONARY>().In(id).ExecuteCommandAsync();
            return deleteResult > 0 ? (ApiResultCodeConst.SUCCESS, "删除成功") : (ApiResultCodeConst.ERROR, "删除失败");
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
            query = query.WhereIF(!string.IsNullOrEmpty(roleId) , u => u.ROLE_ID == roleId);
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
        /// <param name="password">密码</param>
        /// <param name="roleId">角色id</param>
        /// <param name="realName">真实姓名</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public async Task<(int code, string message)> AddOrModifyUser(int id, string userName,string password, string roleId, string realName, string remark)
        {
            var _Salt = _configuration.GetValue<string>("Salt");
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
                    PASSWORD = Common.MD5Encoding(password, _Salt),
                    SALT = _Salt,
                    ROLE_ID = roleId,
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
                existingUser.PASSWORD = Common.MD5Encoding(password, _Salt);
                existingUser.ROLE_ID = roleId;
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

        /// <summary>
        /// 载入修改角色信息
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
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

        //---------------------------------------------------账务配置-----------------------------------------------------
        /// <summary>
        /// 查询账务基本配置
        /// </summary>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        public (int code, string message, D_SYS_CONFIG model) GetSysconfig(string city_cent)
        {
            var model = SugarSimple.Instance().Queryable<D_SYS_CONFIG>().First(x => x.CITY_CENTNO == city_cent);
            if(model == null)
            {
                return (ApiResultCodeConst.ERROR, "未查询到相关账务配置", model);
            }
            return (ApiResultCodeConst.SUCCESS, "", model);
        }
    }
}
