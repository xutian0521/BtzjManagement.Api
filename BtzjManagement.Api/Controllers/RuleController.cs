using Microsoft.AspNetCore.Mvc;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using System.Collections.Generic;
using BtzjManagement.Api.Filter;
using System;
using System.Threading.Tasks;
using SqlSugar;
using Microsoft.AspNetCore.Http;
using YamlDotNet.Core;
using Microsoft.Extensions.Configuration;
using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Models.DBModel;

namespace BtzjManagement.Api.Controllers
{
    /// <summary>
    /// 用户/菜单/角色/权限相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Encryption]
    public class RuleController : BaseController
    {
        RuleService _ruleService;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="ruleService"></param>
        public RuleController(IConfiguration configuration, RuleService ruleService): base(configuration)
        {
            _ruleService = ruleService;
        }
        //---------------------------------------------------菜单-----------------------------------------------------

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="isFilterDisabledMenu">是否过滤掉禁用菜单</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<v_SysMenu>), StatusCodes.Status200OK)]
        [HttpGet("MenuTreeList")]
        public v_ApiResult MenuTreeList(bool isFilterDisabledMenu = false)
        {
            var user = base.GetUser();
            var list = _ruleService.MenuTreeList(int.Parse( user.roleId), 0, isFilterDisabledMenu);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list );
        }


        /// <summary>
        /// 添加或修改菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddOrModifyMenu")]
        public async Task<v_ApiResult> AddOrModifyMenu(P_AddOrModifyMenu p)
        {
            if (string.IsNullOrEmpty(p.title))
            {
                return new v_ApiResult(ApiResultCodeConst.ERROR, "菜单名称不能为空");
            }
            var user = base.GetUser();
            int userId = int.Parse(user.userId);
            var result = await _ruleService.AddOrModifyMenuAsync(p.id,
                p.pId, p.title, p.path, p.icon, p.sortId, p.isEnable, p.remark, userId);
            return new v_ApiResult(result.code, result.message);
        }
        /// <summary>
        /// 获取父级菜单枚举
        /// </summary>
        [ProducesResponseType(typeof(s_ApiResult<List<D_SYS_MENU>>), StatusCodes.Status200OK)]
        [HttpGet("ParentMenuEnums")]
        public async Task<v_ApiResult> ParentMenuEnums()
        {
            var list = await _ruleService.ParentMenuEnums();
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
        }
        /// <summary>
        /// 载入修改菜单信息
        /// </summary>
        /// <param name="id">菜单id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<D_SYS_MENU>), StatusCodes.Status200OK)]
        [HttpGet("LoadModifyMenu")]
        public async Task<v_ApiResult> LoadModifyMenu(int id)
        {
            var one = await _ruleService.LoadModifyMenu(id);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, one);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">菜单id</param>
        /// <returns></returns>
        [HttpGet("DeleteMenu")]
        public async Task<v_ApiResult> DeleteMenu(int id)
        {

            var result = await _ruleService.DeleteMenuAsync(id);
            return new v_ApiResult(result.code, result.message);
        }
        /// <summary>
        /// 载入修改角色菜单信息
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<List<v_SysMenu>>), StatusCodes.Status200OK)]
        [HttpGet("LoadModifyRoleMenu")]
        public v_ApiResult LoadModifyRoleMenu(int roleId)
        {
            var list = _ruleService.MenuTreeList(roleId, 0, false);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
        }
        /// <summary>
        /// 设置角色菜单
        /// </summary>
        /// <param name="model">角色model</param>
        /// <returns></returns>
        [HttpPost("SettingRoleMenu")]
        public async Task<v_ApiResult> SettingRoleMenu([FromBody]P_SettingRoleMenu model)
        {
            var result = await _ruleService.SettingRoleMenuAsync(model.roleId, model.menuIds);
            return new v_ApiResult(result.code, result.message);
        }
        //---------------------------------------------------用户-----------------------------------------------------

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<Pager<D_USER_INFO>>), StatusCodes.Status200OK)]
        [HttpGet("UserList")]
        public async Task<v_ApiResult> UserList(string userName, string roleId, int pageIndex = 1, int pageSize = 10)
        {
            var list = await _ruleService.UserList(userName, roleId, pageIndex, pageSize);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
        }
        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="p">p</param>
        /// <returns></returns>
        [HttpPost("AddOrModifyUser")]
        public async Task<v_ApiResult> AddOrModifyUser(P_AddOrModifyUser p)
        {
            var r = await _ruleService.AddOrModifyUser(p.id, p.userName, p.password, p.roleId, p.realName, p.remark);
            return new v_ApiResult(r.code, r.message, null);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户的id</param>
        /// <returns></returns>
        [HttpGet("DelateUser")]
        public async Task<v_ApiResult> DeleteUser(int id)
        {
            var r = await _ruleService.DeleteUser(id);
            return new v_ApiResult(r.code, r.message, null);
        }

        /// <summary>
        /// 载入修改用户信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<D_USER_INFO>), StatusCodes.Status200OK)]
        [HttpGet("LoadModifyUserInfo")]
        public async Task<v_ApiResult> LoadModifyUserInfo(string id)
        {
            var r =  await _ruleService.LoadModifyUserInfoAsync(id);
            return new v_ApiResult(r.code, r.message, r.user);
        }
        //---------------------------------------------------角色-----------------------------------------------------

        /// <summary>
        /// 载入修改角色信息
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<D_SYS_ROLE>), StatusCodes.Status200OK)]
        [HttpGet("LoadModifyRoleInfo")]
        public async Task<v_ApiResult> LoadModifyRoleInfo(int id)
        {
            var r = await _ruleService.LoadModifyRoleInfo(id);
            return new v_ApiResult(r.code, r.message, r.role);
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<Pager<D_SYS_ROLE>>), StatusCodes.Status200OK)]
        [HttpGet("RoleList")]
        public async Task<v_ApiResult> RoleList(string roleName, int pageIndex = 1, int pageSize = 10)
        {
            var r = await _ruleService.RoleListAsync(roleName, pageIndex, pageSize);
            return new v_ApiResult(r.code, r.message, r.list);
        }
        /// <summary>
        /// 新增或修改角色
        /// </summary>
        /// <param name="p">p</param>
        /// <returns></returns>
        [HttpPost("AddOrModifyRole")]
        public async Task<v_ApiResult> AddOrModifyRole(P_AddOrModifyRole p)
        {
            //var user = this.HttpContext.Items["User"] as JwtPayload;
            //Guid userId = Guid.Parse(user.userId);
            var r = await _ruleService.AddOrModifyRoleAsync(p.id, p.roleName, p.remark);
            return new v_ApiResult(r.code, r.message);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpPost("DeleteRole")]
        public async Task<v_ApiResult> DeleteRole([FromForm] int id)
        {
            //var user = this.HttpContext.Items["User"] as JwtPayload;
            //Guid userId = Guid.Parse(user.userId);
            var r = await _ruleService.DeleteRoleAsync(id);
            return new v_ApiResult(r.code, r.message);
        }

        //---------------------------------------------------字典-----------------------------------------------------


        /// <summary>
        /// 获取枚举列表下拉选项
        /// </summary>
        /// <param name="type">字典类型</param>
        /// <param name="val">字典值</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<List<D_SYS_DATA_DICTIONARY>>), StatusCodes.Status200OK)]
        [HttpGet("GetDataDictionaryListByType")]
        public v_ApiResult GetDataDictionaryListByType(string type,string val="")
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var list = _ruleService.GetDataDictionaryListByType(type, val);
                result.Code = ApiResultCodeConst.SUCCESS;
                result.Content = list;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
        /// <summary>
        /// 字典列表递归
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<List<v_SYS_DATA_DICTIONARY>>), StatusCodes.Status200OK)]
        [HttpGet("GetDataDictionaryTreeList")]
        public v_ApiResult GetDataDictionaryTreeList()
        {
            var list = _ruleService.DataDictionaryTreeList(0);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
        }
        /// <summary>
        /// 数据字典类型
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<List<D_SYS_DATA_DICTIONARY>>), StatusCodes.Status200OK)]
        [HttpGet("GetDataDictionaryListByParent")]
        public async Task<v_ApiResult> GetDataDictionaryListByParent()
        {
            var list = await _ruleService.GetDataDictionaryListByParent();
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
        }
        /// <summary>
        /// 载入修改枚举字典
        /// </summary>
        /// <param name="id">字典id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<v_SYS_DATA_DICTIONARY>), StatusCodes.Status200OK)]
        [HttpGet("LoadModifyEnumInfoById")]
        public async Task<v_ApiResult> LoadModifyEnumInfoById(int id)
        {
            var r=  await _ruleService.LoadModifyEnumInfoById(id);
            return new v_ApiResult(r.code, r.message, r.@enum);
        }

        /// <summary>
        /// 新增或修改字典
        /// </summary>
        /// <param name="p">p</param>
        /// <returns></returns>
        [HttpPost("AddOrModifyDictionary")]
        public async Task<v_ApiResult> AddOrModifyDictionary(P_AddOrModifyDictionary p)
        {
            var result = await _ruleService.AddOrModifyDictionary(p);
            return new v_ApiResult(result.code, result.message);
        }
        /// <summary>
        /// 删除枚举字典
        /// </summary>
        /// <param name="id">字典id</param>
        /// <returns></returns>
        [HttpGet("DeleteEnum")]
        public async Task<v_ApiResult> DeleteEnum(int id)
        {
            var result = await _ruleService.DeleteEnumAsync(id);
            return new v_ApiResult(result.code, result.message);
        }
    }
}
