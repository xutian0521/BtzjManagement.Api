using Microsoft.AspNetCore.Mvc;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using System.Collections.Generic;
using BtzjManagement.Api.Filter;
using System;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Encryption]
    public class RuleController : ControllerBase
    {
        RuleService _ruleService;
        SysEnumService _sysEnumService;
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
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserList")]
        public async Task<v_ApiResult> UserList(string userName, string roleId, int pageIndex = 1, int pageSize = 10)
        {
            var list = await _ruleService.UserList(userName, roleId, pageIndex, pageSize);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
        }
        [HttpPost("AddOrModify")]
        public async Task<v_ApiResult> AddOrModify([FromForm] int id, [FromForm]string userName, [FromForm] string passWord,
            [FromForm] string roleId, [FromForm] string realName, [FromForm] string remark)
        {
            var r = await _ruleService.AddOrModify(id, userName, passWord, roleId, realName, remark);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, r.message);
        }

        [HttpGet("DelateUser")]
        public async Task<v_ApiResult> DeleteUser(int id)
        {
            var r = await _ruleService.DeleteUser(id);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, r.message);
        }

    }
}
