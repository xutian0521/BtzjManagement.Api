﻿using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    /// <summary>
    /// 科目控制器
    /// </summary>
    [Route("api/KM")]
    [ApiController]
    public class KMController : BaseController
    {
        KMService _kMService;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="kMService"></param>
        public KMController(IConfiguration configuration, KMService kMService) : base(configuration)
        {
            _kMService = kMService;
        }

        /// <summary>
        /// 科目树列表
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<v_KM>), StatusCodes.Status200OK)]
        [HttpGet("GetKMTreeList")]
        public v_ApiResult GetKMTreeList()
        {
            var user = base.GetUser();
            var list = _kMService.GetKMTreeList();
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
        }
        /// <summary>
        /// 科目树列表
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(s_ApiResult<v_KM>), StatusCodes.Status200OK)]
        [HttpGet("GetKMTreeListByKMBH")]
        public v_ApiResult GetKMTreeListByKMBH(string KMBH)
        {
            var user = base.GetUser();
            var list = _kMService.GetKMTreeListByKMBH(KMBH);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, list);
        }
        /// <summary>
        /// 添加或修改科目
        /// </summary>
        [HttpPost("AddOrModifyKM")]
        public async Task<v_ApiResult> AddOrModifyKM([FromBody]P_AddOrModifyKM p)
        {
            var user = base.GetUser();
            var result = await _kMService.AddOrModifyKM(p);
            return new v_ApiResult(result.code, result.message);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="KMBH">科目编号</param>
        /// <returns></returns>
        [HttpGet("DeleteKM")]
        public async Task<v_ApiResult> DeleteKM(string KMBH)
        {

            var result = await _kMService.DeleteKM(KMBH);
            return new v_ApiResult(result.code, result.message);
        }
        /// <summary>
        /// 载入修改菜单信息
        /// </summary>
        /// <param name="KMBH">科目编号</param>
        /// <returns></returns>
        [HttpGet("LoadModifyKM")]
        public async Task<v_ApiResult> LoadModifyKM(string KMBH)
        {
            var one = await _kMService.LoadModifyKM(KMBH);
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, one);
        }

    }
}
