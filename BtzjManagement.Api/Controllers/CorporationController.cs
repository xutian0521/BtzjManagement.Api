using BtzjManagement.Api.Enum;
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
    [Encryption]
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
                return _corporationService.CreateCorporation(pmodel, CityCent());
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
        /// <param name="dwzh">单位账号</param>
        /// <param name="dwmc">单位名称</param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("CorporationSelectList")]
        public v_ApiResult CorporationSelectList(string dwzh, string dwmc)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var list = _corporationService.CorporationSelectList(dwzh, dwmc);
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
