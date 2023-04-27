using BtzjManagement.Api.Filter;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    [ApiController]
    [Encryption]
    [Route("api/OldSource")]
    public class SybaseController : ControllerBase
    {
        SybaseService _sybaseService;
        public SybaseController(SybaseService sybaseService)
        {
            _sybaseService = sybaseService;
        }
        /// <summary>
        /// sybase原表数据量统计
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET","POST")]
        [Route("AccquireSybaseTableDataCount")]
        public async Task<List<v_TableDataCount>> MenuTreeList()
        {
            var list = await _sybaseService.AccquireSybaseTableDataCount();
            return list;
        }
    }
}
