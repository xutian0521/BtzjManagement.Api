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
    public class CorporationController : ControllerBase
    {
        IConfiguration _configuration;
        CorporationService _corporationService;
        public CorporationController(IConfiguration configuration,CorporationService corporationService)
        {
            _configuration = configuration;
            _corporationService = corporationService;
        }

        [AcceptVerbs("POST")]
        [Route("CreateCorporation")]
        public v_ApiResult CreateCorporation(P_In_Corporation_Add pmodel)
        {
            //var list = EnumHepler.GetEnumDescriptionItems<string>(typeof(DwxzConst));

            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            try
            {
                var sugarHelper = SugarHelper.Instance();

                var newDwzhInt = _corporationService.NextDwzhInt();
                var dwzh = Common.PaddingDwzh(newDwzhInt, 4);
                var custId = Common.PaddingDwzh(newDwzhInt, 8);
                var city_cent = _configuration.GetValue<string>("CityCent");

                if (string.IsNullOrEmpty(pmodel.DWMC))
                {
                    result.Message = "单位名称不能为空";
                    return result;
                }

                if(sugarHelper.IsExist<D_CORPORATION_BASICINFO>(x=>x.DWMC == pmodel.DWMC.Trim()))
                {
                    result.Message = "当前单位名称已存在";
                    return result;
                }

                //单位基本信息
                D_CORPORATION_BASICINFO basicModel = new D_CORPORATION_BASICINFO
                {
                    BASICACCTBRCH = pmodel.BASICACCTBRCH,
                    BASICACCTMC = pmodel.BASICACCTMC,
                    BASICACCTNO = pmodel.BASICACCTNO,
                    CUSTID = custId,
                    DWDZ = pmodel.DWDZ,
                    DWFRDBXM = pmodel.DWFRDBXM,
                    DWFRDBZJHM = pmodel.DWFRDBZJHM,
                    DWFRDBZJLX = pmodel.DWFRDBZJLX,
                    DWFXR = pmodel.DWFXR,
                    DWMC = pmodel.DWMC,
                    DWSLRQ = pmodel.DWSLRQ /*Common.StringToDate(pmodel.DWSLRQ)*/,
                    DWXZ = pmodel.DWXZ,
                    DWYB = pmodel.DWYB,
                    JBRGDDHHM = pmodel.JBRGDDHHM,
                    JBRSJHM = pmodel.JBRSJHM,
                    JBRXM = pmodel.JBRXM,
                    JBRZJHM = pmodel.JBRZJHM,
                    JBRZJLX = pmodel.JBRZJLX,
                    USCCID = pmodel.USCCID,
                    CITY_CENTNO = city_cent
                };

                //单位账户信息
                D_CORPORATION_ACCTINFO acctModel = new D_CORPORATION_ACCTINFO
                {
                    CITY_CENTNO = city_cent,
                    CALC_METHOD = pmodel.CALC_METHOD,
                    CUSTID = custId,
                    DWFCRS = 0,
                    DWJCBL = pmodel.DWJCBL,
                    DWJCRS = 0,
                    DWZGRS = 0,
                    DWZH = dwzh,
                    FROMFLAG = pmodel.FROMFLAG,
                    FACTINCOME = 0,
                    MONTHPAYTOTALAMT = 0,
                    NEXTPAYMTH = pmodel.NEXTPAYMTH,
                    REGHANDBAL = 0,
                    STYHZH = pmodel.STYHZH,
                    STYHDM = pmodel.STYHDM,
                    STYHMC = pmodel.STYHMC,
                    DWZHYE = 0,
                };
                var rInsert = sugarHelper.InvokeTransactionScope(() => { sugarHelper.Add(basicModel); sugarHelper.Add(acctModel); });

                result.Code = rInsert ? ApiResultCodeConst.SUCCESS : ApiResultCodeConst.ERROR;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
