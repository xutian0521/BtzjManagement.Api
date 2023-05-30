using BtzjManagement.Api.Enum;
using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PersonInfoController : BaseController
    {
        PersonInfoService _personInfoService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="personInfoService"></param>
        public PersonInfoController(IConfiguration configuration, PersonInfoService personInfoService) : base(configuration)
        {
            _personInfoService = personInfoService;
        }

        /// <summary>
        /// 按月汇缴个人开户
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        [Route("CreatePersonInfo")]
        public v_ApiResult CreatePersonInfo(P_In_PersonInfo pmodel)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var r = _personInfoService.CreatePersonInfo(pmodel, CityCent(), GetUser().userName);
                result.Code = r.Item1;
                result.Message = r.Item2;
                result.Content = r.Item3;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取按月汇缴个人开户分页数据
        /// </summary>
        /// <param name="dwzh"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="KHTYPE"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("PersonKhMonthList")]
        public v_ApiResult PersonKhMonthList(string dwzh, int page = 1, int size = 10,int KHTYPE =1, string status = "created")
        {
            v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
            try
            {
                var r = _personInfoService.PersonKhMonthList(CityCent(), dwzh, page, size, KHTYPE, status);
                result.Code = ApiResultCodeConst.SUCCESS;
                result.Message = ApiResultMessageConst.SUCCESS;
                result.Content = r;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 根据id获取按月汇缴个人开户详细数据
        /// </summary>
        /// <param name="ywlsh"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("PersonKHMonthModel")]
        public v_ApiResult PersonKHMonthModel(string ywlsh, int id)
        {
            v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
            try
            {
                var r = _personInfoService.PersonKHMonthModel(ywlsh, id);
                if (r == null)
                {
                    result.Message = "未查询到相关数据";
                    return result;
                }
                result.Code = ApiResultCodeConst.SUCCESS;
                result.Message = ApiResultMessageConst.SUCCESS;
                result.Content = r;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 修改按月汇缴个人开户明细信息
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        [Route("UpdatePersonKHMonthModel")]
        public v_ApiResult UpdatePersonKHMonthModel(P_In_PersonInfo pmodel)
        {
            v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
            try
            {
                var r = _personInfoService.UpdatePersonKHMonthModel(pmodel, CityCent(), GetUser().userName);
                result.Code = r.code;
                result.Message = r.msg;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        
    }
}
