using BtzjManagement.Api.Enum;
using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Services
{
    public class CorporationService
    {
        //获取最大单位账号数字
        public int NextDwzhInt()
        {
            var maxDwzh = SugarHelper.Instance().Max<D_CORPORATION_ACCTINFO, int>("DWZH");
            return maxDwzh + 1;
        }

        /// <summary>
        /// 按月汇缴单位开户
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        public v_ApiResult CreateCorporation(P_In_Corporation_Add pmodel,string city_cent)
        {
            v_ApiResult result = new v_ApiResult() { Code= ApiResultCodeConst.ERROR };

            var sugarHelper = SugarHelper.Instance();

            var newDwzhInt = NextDwzhInt();
            var dwzh = Common.PaddingDwzh(newDwzhInt, 4);
            var custId = Common.PaddingDwzh(newDwzhInt, 8);
            //var city_cent = _configuration.GetValue<string>("CityCent");

            if (string.IsNullOrEmpty(pmodel.DWMC))
            {
                result.Message = "单位名称不能为空";
                return result;
            }

            if (sugarHelper.IsExist<D_CORPORATION_BASICINFO>(x => x.DWMC == pmodel.DWMC.Trim()))
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
                DWZHZT = DwzhztConst.正常,
            };
            sugarHelper.InvokeTransactionScope(() => { sugarHelper.Add(basicModel); sugarHelper.Add(acctModel); });

            result.Code = ApiResultCodeConst.SUCCESS;

            return result;
        }

        /// <summary>
        /// 单位信息筛选下拉列表
        /// </summary>
        /// <param name="dwzh"></param>
        /// <param name="dwmc"></param>
        /// <returns></returns>
        public List<v_CorporationSelect> CorporationSelectList(string dwzh, string dwmc)
        {
            var dwzhzts = new string[] { DwzhztConst.正常, DwzhztConst.封存 };
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>> where = null;
            if (!string.IsNullOrEmpty(dwzh) && !string.IsNullOrEmpty(dwmc))
            {
                where = (t1, t2) => t2.DWZH.Contains(dwzh) && t1.DWMC.Contains(dwmc) && dwzhzts.Contains(t2.DWZHZT);
            }
            else if (string.IsNullOrEmpty(dwzh) && string.IsNullOrEmpty(dwmc))
            {
                where = (t1, t2) => dwzhzts.Contains(t2.DWZHZT);
            }
            else
            {
                if (!string.IsNullOrEmpty(dwzh))
                {
                    where = (t1, t2) => t2.DWZH.Contains(dwzh) && dwzhzts.Contains(t2.DWZHZT);
                }

                if (!string.IsNullOrEmpty(dwmc))
                {
                    where = (t1, t2) => t1.DWMC.Contains(dwmc) && dwzhzts.Contains(t2.DWZHZT);
                }
            }

            var list = SugarHelper.Instance().QueryMuch<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, v_CorporationSelect>((t1, t2) => new object[] { JoinType.Inner, t1.CUSTID == t2.CUSTID }, (t1, t2) => new v_CorporationSelect { DWMC = t1.DWMC, DWZH = t2.DWZH }, where);
            return list;
        }
    }
}
