using Microsoft.AspNetCore.Mvc;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using System.Collections.Generic;
using BtzjManagement.Api.Filter;
using System;

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


    }
}
