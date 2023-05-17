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
    [Route("api/DataTransfer")]
    public class DataTransferController : Controller
    {
        DataTransferService _transferService;
        public DataTransferController(DataTransferService transferService)
        {
            _transferService = transferService;
        }

        /// <summary>
        /// 单位基本信息表/单位账户信息表结构初始化
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Route("CorporationInfoInitStructure")]
        public v_ApiResult CorporationInfoInitStructure()
        {
            try
            {
                _transferService.SysEnumInitStructure();
                _transferService.CorporationBasicInfoInitStructure();
                _transferService.CorporationAcctInfoInitStructure();
            }
            catch (Exception ex)
            {
                return new v_ApiResult(ApiResultCodeConst.ERROR, ex.Message, false);
            }
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, true);
        }

        /// <summary>
        /// 单位基本信息表/单位账户信息表数据初始化
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Route("CorporationInfoInitData")]
        public v_ApiResult CORPORATIONINFO_INIT_DATA()
        {
            try
            {
                _transferService.SysEnumInitData();
                _transferService.CorporationBasicInfoInitData();
                _transferService.CorporationAcctInfoInitData();
            }
            catch (Exception ex)
            {
                return new v_ApiResult(ApiResultCodeConst.ERROR, ex.Message, false);
            }
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, true);
        }
    }
}
