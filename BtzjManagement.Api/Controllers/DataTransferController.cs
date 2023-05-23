using BtzjManagement.Api.Filter;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    [ApiController]
    [Encryption]
    [Route("api/DataTransfer")]
    public class DataTransferController : BaseController
    {
        DataTransferService _transferService;
        public DataTransferController(IConfiguration configuration, DataTransferService transferService) : base(configuration)
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
                //_transferService.SysEnumInitStructure();
                //_transferService.CorporationBasicInfoInitStructure();
                //_transferService.CorporationAcctInfoInitStructure();
                //_transferService.GrkhInitStructure();
                //_transferService.Grkh_ItemInitStructure();
                //_transferService.SysMenuInitStructure();
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
                //_transferService.SysEnumInitData(CityCent());
                //_transferService.CorporationBasicInfoInitData(CityCent());
                //_transferService.CorporationAcctInfoInitData(CityCent());
                //_transferService.SysMenuInitData(CityCent());
            }
            catch (Exception ex)
            {
                return new v_ApiResult(ApiResultCodeConst.ERROR, ex.Message, false);
            }
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, true);
        }


        /// <summary>
        /// 用户信息表数据初始化
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Route("UserInfoInitData")]
        public v_ApiResult UserInfoInitData()
        {
            try
            {
                _transferService.UserInfoInitData(CityCent());
            }
            catch (Exception ex)
            {
                return new v_ApiResult(ApiResultCodeConst.ERROR, ex.Message, false);
            }
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, true);
        }
    }
}
