﻿using Microsoft.AspNetCore.Mvc;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using System.Collections.Generic;
using BtzjManagement.Api.Filter;
using System;
using System.Threading.Tasks;
using SqlSugar;
using Microsoft.AspNetCore.Http;
using YamlDotNet.Core;

namespace BtzjManagement.Api.Controllers
{
    /// <summary>
    /// 用户/菜单/角色/权限相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Encryption]
    public class RuleController : ControllerBase
    {
        RuleService _ruleService;
        SysEnumService _sysEnumService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ruleService"></param>
        /// <param name="sysEnumService"></param>
        public RuleController(RuleService ruleService, SysEnumService sysEnumService)
        {
            _ruleService = ruleService;
            _sysEnumService = sysEnumService;
        }
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="isFilterDisabledMenu">是否过滤掉禁用菜单</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<v_SysMenu>), StatusCodes.Status200OK)]
        [HttpGet("MenuTreeList")]
        public v_ApiResult MenuTreeList(bool isFilterDisabledMenu = false)
        {
            var list = _ruleService.MenuTreeList(0 , isFilterDisabledMenu);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list );
        }

        /// <summary>
        /// 获取枚举列表下拉选项
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("GetEnum")]
        public v_ApiResult GetEnum(string type)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var list = _sysEnumService.GetListByType(type);
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
        /// 添加或修改菜单
        /// </summary>
        /// <param name="id">菜单id</param>
        /// <param name="pId">父级id</param>
        /// <param name="title">菜单标题</param>
        /// <param name="path">路径</param>
        /// <param name="icon">图标</param>
        /// <param name="sortId">排序</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [HttpPost("AddOrModifyMenu")]
        public async Task<v_ApiResult> AddOrModifyMenu([FromForm] int id, [FromForm] int pId, [FromForm] string title,
            [FromForm] string path, [FromForm] string icon, [FromForm] int sortId, [FromForm] bool isEnable, [FromForm] string remark)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new v_ApiResult(ApiResultCodeConst.ERROR, "菜单名称不能为空");
            }
            var user = this.HttpContext.Items["User"] as JwtPayload;
            Guid userId = Guid.Parse(user.userId);
            var result = await _ruleService.AddOrModifyMenuAsync(id, pId, title, path, icon, sortId, isEnable, remark, userId);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }
        /// <summary>
        /// 获取父级菜单枚举
        /// </summary>
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
        [HttpGet("LoadModifyMenu")]
        public async Task<v_ApiResult> LoadModifyMenu(int id)
        {
            var one = await _ruleService.LoadModifyMenu(id);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, one);
        }

        //---------------------------------------------------用户-----------------------------------------------------

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserList")]
        public async Task<v_ApiResult> UserList(string userName, string roleId, int pageIndex = 1, int pageSize = 10)
        {
            var list = await _ruleService.UserList(userName, roleId, pageIndex, pageSize);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
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
        [HttpPost("AddOrModifyUser")]
        public async Task<v_ApiResult> AddOrModifyUser([FromForm] int id, [FromForm]string userName, [FromForm] string passWord,
            [FromForm] string roleId, [FromForm] string realName, [FromForm] string remark)
        {
            var r = await _ruleService.AddOrModifyUser(id, userName, passWord, roleId, realName, remark);
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
        [HttpGet("RoleList")]
        public async Task<v_ApiResult> RoleList(string roleName, int pageIndex = 1, int pageSize = 10)
        {
            var r = await _ruleService.RoleListAsync(roleName, pageIndex, pageSize);
            return new v_ApiResult(r.code, r.message, r.list);
        }
        /// <summary>
        /// 新增或修改角色
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="roleName">角色名</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [HttpPost("AddOrModifyRole")]
        public async Task<v_ApiResult> AddOrModifyRole([FromForm] int id, [FromForm] string roleName, [FromForm] string remark)
        {
            //var user = this.HttpContext.Items["User"] as JwtPayload;
            //Guid userId = Guid.Parse(user.userId);
            var r = await _ruleService.AddOrModifyRoleAsync(id, roleName, remark);
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

    }
}
