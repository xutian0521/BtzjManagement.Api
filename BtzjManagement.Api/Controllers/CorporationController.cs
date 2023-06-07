﻿using BtzjManagement.Api.Enum;
using BtzjManagement.Api.Filter;
using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using BtzjManagement.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    [Route("api/Corporation")]
    [ApiController]
    //[Encryption]
    public class CorporationController : BaseController
    {
        CorporationService _corporationService;
        public CorporationController(IConfiguration configuration, CorporationService corporationService) : base(configuration)
        {
            _corporationService = corporationService;
        }

        /// <summary>
        /// 按月汇缴单位开户
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        [Route("CreateCorporation")]
        public v_ApiResult CreateCorporation(P_In_Corporation_Add pmodel)
        {
            //var list = EnumHepler.GetEnumDescriptionItems<string>(typeof(DwxzConst));

            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                return _corporationService.CreateUpdateCorporation(pmodel, CityCent(), GetUser().userName);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 单位信息筛选下拉列表
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("CorporationSelectList")]
        [ProducesResponseType(typeof(s_ApiResult<List<v_CorporationSelect>>), StatusCodes.Status200OK)]
        public v_ApiResult CorporationSelectList(string searchKey)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var list = _corporationService.CorporationSelectList(CityCent(), searchKey);
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
        /// 获取保存状态的单位开户数据
        /// </summary>
        /// <param name="tyxydm"></param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("CorporationCreatedModel")]
        [ProducesResponseType(typeof(s_ApiResult<v_CorporationCreated>), StatusCodes.Status200OK)]
        public v_ApiResult CorporationCreatedModel(string tyxydm)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                return _corporationService.CorporationCreatedModel(tyxydm, CityCent());
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 提交单位开户业务
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        [Route("SubmitCorporationCreated")]
        public v_ApiResult SubmitCorporationCreated(P_In_Corporation_Submit pmodel)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var rT = _corporationService.SubmitCorporationCreated(pmodel, GetUser().userName, CityCent());
                result.Code = rT.Item1 ? ApiResultCodeConst.SUCCESS : ApiResultCodeConst.ERROR;
                result.Message = rT.Item2;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
