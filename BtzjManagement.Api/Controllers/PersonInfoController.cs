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
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("PersonKhMonthList")]
        [ProducesResponseType(typeof(s_ApiResult<Pager<v_Busi_Grkh>>), StatusCodes.Status200OK)]
        public v_ApiResult PersonKhMonthList(string dwzh, int pageIndex = 1, int pageSize = 10, string status = "created")
        {
            v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
            try
            {
                var r = _personInfoService.PersonKhMonthList(CityCent(), dwzh, pageIndex, pageSize, status);
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
        [ProducesResponseType(typeof(s_ApiResult<v_Busi_Grkh>), StatusCodes.Status200OK)]
        public v_ApiResult PersonKHMonthModel(string ywlsh, int id)
        {
            v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
            try
            {
                var r = _personInfoService.PersonKHMonthModel(ywlsh, id, CityCent());
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

        /// <summary>
        /// 删除按月开户明细
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        [Route("RemovePersonKHMonthModel")]
        public v_ApiResult RemovePersonKHMonthModel(P_PersonInfo_Delete pmodel)
        {
            v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
            try
            {
                var r = _personInfoService.RemovePersonKHMonthModel(pmodel, CityCent(), GetUser().userName);
                result.Code = r.code;
                result.Message = r.msg;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 提交按月汇缴个人开户业务
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        [Route("SubmitPersonInfoMonthCreated")]
        public v_ApiResult SubmitPersonInfoMonthCreated(P_PersonInfo_Submit pmodel)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var rT = _personInfoService.SubmitPersonInfoMonthCreated(pmodel, GetUser().userName, CityCent());
                result.Code = rT.code;
                result.Message = rT.Item2;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取缴存额
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        [AcceptVerbs("POST")]
        [Route("CalcYjce")]
        public v_ApiResult CalcYjce(P_PersonInfo_CalcJce pmodel)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var rT = _personInfoService.CalcYjce(pmodel);
                result.Code = ApiResultCodeConst.SUCCESS;
                result.Content = rT;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取单位缴存变更分页数据
        /// </summary>
        /// <param name="dwzh"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <param name="type">all：所有账户状态；dwhj:正常，封存状态</param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [Route("DwJcbgPageList")]
        [ProducesResponseType(typeof(s_ApiResult<Pager<v_CustomerInfo>>), StatusCodes.Status200OK)]
        public v_ApiResult DwJcbgPageList(string dwzh, int pageIndex = 1, int pageSize = 10, string searchKey = "",string type = "all")
        {
            v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
           
            try
            {
                List<string> grzhzt = new List<string> { };
                if (type == "dwhj")
                {
                    grzhzt.AddRange(new List<string> { GrzhztConst.正常, GrzhztConst.封存 });
                }
                var r = _personInfoService.DwJcbgPersonPageList(CityCent(), 1, int.MaxValue, dwzh, searchKey, grzhzt);
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



       

    }
}


