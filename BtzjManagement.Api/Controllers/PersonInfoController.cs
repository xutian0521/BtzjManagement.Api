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
    [Route("api/[controller]")]
    [ApiController]
    public class PersonInfoController : BaseController
    {
        PersonInfoService _personInfoService;
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


    }
}
