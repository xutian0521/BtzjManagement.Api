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

        /// <summary>
        /// 获取保存状态的数据
        /// </summary>
        /// <param name="tyxydm"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        public v_ApiResult CorporationCreatedModel(string tyxydm,string city_cent)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            var status = new string[] { OptStatusConst.新建, OptStatusConst.等待初审, OptStatusConst.等待终审, OptStatusConst.初审出错, OptStatusConst.终审出错, OptStatusConst.初审退回, OptStatusConst.终审退回};
            //业务数据-业务数据
            var bus_model = SugarHelper.Instance().First<D_BUSI_CORPORATION>(x => x.USCCID.ToUpper() == tyxydm && x.CITY_CENTNO == city_cent && x.BUSITYPE == GjjOptType.单位开户);

            if(bus_model == null)//不存在保存的数据
            {
                result.Code = ApiResultCodeConst.SUCCESS;
                result.Content = new v_CorporationCreated();
                return result;
            }

            //基本信息和账户信息
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>> where = null;
            where = (t1, t2) => t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent && t1.USCCID.ToUpper().Contains(tyxydm.ToUpper());
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, v_BaseCorporatiorn>> selectExpression = (t1, t2) => new v_BaseCorporatiorn
            {
                DWMC = t1.DWMC,
                USCCID = t1.USCCID,
                BASICACCTBRCH = t1.BASICACCTBRCH,
                DWDZ = t1.DWDZ,
                DWFRDBXM = t1.DWFRDBXM,
                BASICACCTMC = t1.BASICACCTMC,
                BASICACCTNO = t1.BASICACCTNO,
                DWFRDBZJHM = t1.DWFRDBZJHM,
                DWFRDBZJLX = t1.DWFRDBZJLX,
                DWFXR = t1.DWFXR,
                JBRGDDHHM = t1.JBRGDDHHM,
                JBRSJHM = t1.JBRSJHM,
                JBRXM = t1.JBRXM,
                JBRZJHM = t1.JBRZJHM,
                JBRZJLX = t1.JBRZJLX,
                DWXZ = t1.DWXZ,
                DWYB = t1.DWYB,
                DWSLRQ = t1.DWSLRQ,
                CALC_METHOD = t2.CALC_METHOD,
                DWJCBL = t2.DWJCBL,
                DWQJRQ = t1.DWQJRQ,
                NEXTPAYMTH = t2.NEXTPAYMTH,
                STYHDM = t2.STYHDM,
                FROMFLAG = t2.FROMFLAG,
                STYHMC = t2.STYHMC,
                STYHZH = t2.STYHZH,
                DWZH = t2.DWZH
            };
            var baseModel = SugarHelper.Instance().QueryMuch((t1, t2) => new object[] { JoinType.Inner, t1.CUSTID == t2.CUSTID }, selectExpression, where).FirstOrDefault();

            if (bus_model.STATUS != OptStatusConst.新建)//说明已经有在途的业务
            {
                result.Message = $"该统一信用代码代码（{tyxydm}）有在途未办结的单位开户业务";
            }

            result.Content = baseModel;
            return result;
            //影像信息
        }

    }
}
