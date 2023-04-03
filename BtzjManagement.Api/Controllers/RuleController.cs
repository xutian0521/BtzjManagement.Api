using Microsoft.AspNetCore.Mvc;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using System.Collections.Generic;

namespace BtzjManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        RuleService _ruleService;
        public RuleController(RuleService ruleService)
        {
            _ruleService = ruleService;
        }
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="isFilterDisabledMenu">是否过滤掉禁用菜单</param>
        /// <returns></returns>
        [HttpGet("MenuTreeList")]
        public List<v_SysMenu> MenuTreeList(bool isFilterDisabledMenu = false)
        {
            var list = _ruleService.MenuTreeList(null, isFilterDisabledMenu);
            return list;
        }
    }
}
