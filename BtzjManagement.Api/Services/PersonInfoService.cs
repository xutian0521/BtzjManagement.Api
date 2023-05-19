using BtzjManagement.Api.Enum;
using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Services
{
    public class PersonInfoService
    {
        /// <summary>
        /// 录入按月补贴个人开户信息
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="city_cent">网点编号</param>
        /// <param name="optName">操作人</param>
        /// <returns>业务流水号</returns>
        public v_ApiResult CreatePersonInfo(P_In_PersonInfo pmodel,string city_cent, string optName)
        {
            v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
            var ywlsh = Common.UniqueYwlsh();
            var sugarHelper = SugarHelper.Instance();
            //先写入主表数据
            D_GRKH grkh = new D_GRKH
            {
                CITY_CENTNO = city_cent,
                CREATE_MAN = optName,
                CREATE_TIME = DateTime.Now,
                DWMC = pmodel.DWMC,
                DWZH = pmodel.DWZH,
                KHTYPE = KhtypeConst.按月汇缴,
                YWLSH = ywlsh,
                STATUS = OptStatusConst.新建,
            };

            //写入明细表数据
            D_GRKH_ITEM grhk_Item = new D_GRKH_ITEM
            {
                YWLSH = ywlsh,
                CSNY = pmodel.CSNY,
                DWJCBL = pmodel.DWJCBL,
                DWZH = pmodel.DWZH,
                GRCKZHHM = pmodel.GRCKZHHM,
                GRCKZHKHYHDM = pmodel.GRCKZHKHYHDM,
                GRCKZHKHMC = "",
                GRJCJS = pmodel.GRJCJS,
                GRYJCE = pmodel.GRYJCE,
                GRZHZT = GrzhztConst.正常,
                QJRQ = pmodel.QJRQ,
                SJHM = pmodel.SJHM,
                XINGBIE = "",
                XINGMING = pmodel.XINGMING,
                YWYD = pmodel.YWYD,
                ZJHM = pmodel.ZJHM,
                ZJLX = pmodel.ZJLX
            };

            Action action = () => { sugarHelper.Add(grkh); sugarHelper.Add(grhk_Item); };
            sugarHelper.InvokeTransactionScope(action);
            result.Code = ApiResultCodeConst.SUCCESS;
            result.Content = ywlsh;

            return result;
        }
    }
}
