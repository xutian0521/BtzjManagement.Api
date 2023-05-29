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
using System.Threading.Tasks;

namespace BtzjManagement.Api.Services
{
    public class PersonInfoService
    {
        FlowProcService _flowProcService;
        CorporationService _corporationService;
        public PersonInfoService(FlowProcService flowProcService,CorporationService corporationService)
        {
            _flowProcService = flowProcService;
            _corporationService = corporationService;
        }

        /// <summary>
        /// 录入按月补贴个人开户信息
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="city_cent">网点编号</param>
        /// <param name="optName">操作人</param>
        /// <returns>业务流水号</returns>
        public (int,string,string) CreatePersonInfo(P_In_PersonInfo pmodel,string city_cent, string optName)
        {
            //v_ApiResult result = new v_ApiResult { Code = ApiResultCodeConst.ERROR };
            var sugarHelper = SugarHelper.Instance();
            Action action = null;
            //先查单位信息
            if (string.IsNullOrEmpty(pmodel.DWZH))
            {
                return (ApiResultCodeConst.ERROR, $"单位账号不能为空", string.Empty);
            }

            if (string.IsNullOrEmpty(pmodel.ZJHM))
            {
                return (ApiResultCodeConst.ERROR, $"证件号码不能为空", string.Empty);
            }

            if (pmodel.DWJCBL <= 0)
            {
                return (ApiResultCodeConst.ERROR, $"请输入正确的单位缴存比例", string.Empty);
            }

            var dwInfo = _corporationService.GetCorporatiorn((t1, t2) => t2.DWZH == pmodel.DWZH && t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent);

            if(dwInfo == null)
            {
                return (ApiResultCodeConst.ERROR, $"未查询到相关单位（{pmodel.DWZH}）信息", string.Empty);
            }

            //先查这个单位是否有保存状态的主表数据，如果有接着用这个主表，如果没有就新建主表
            var grkh = sugarHelper.First<D_GRKH>(x => x.DWZH == pmodel.DWZH && x.CITY_CENTNO == city_cent);
            int ywid = 0;//主表业务id
            if (grkh == null)
            {
                //无主表，需要新建
                var ywlsh = Common.UniqueYwlsh();
                //先写入主表数据
                grkh = new D_GRKH
                {
                    CITY_CENTNO = city_cent,
                    CREATE_MAN = optName,
                    CREATE_TIME = DateTime.Now,
                    DWMC = pmodel.DWMC,
                    DWZH = pmodel.DWZH,
                    KHTYPE = KhtypeConst.按月汇缴,
                    YWLSH = ywlsh,
                    STATUS = OptStatusConst.新建
                };
                action += () => { ywid = sugarHelper.AddReturnIdentity(grkh); };
            }
            else
            {
                ywid = grkh.ID;
                grkh.CREATE_MAN = optName;
                grkh.CREATE_TIME = DateTime.Now;

                action += () => { sugarHelper.Update(grkh); };
            }

            //查用户是否已有账户
            var model = GetCreatedGrhk(pmodel.ZJHM, KhtypeConst.按月汇缴, city_cent);

            if (model == null)//没录入过该用户
            {
                //写入明细表数据
                D_GRKH_ITEM grhk_Item = new D_GRKH_ITEM
                {
                    YWLSH = grkh.YWLSH,
                    CSNY = pmodel.CSNY,
                    DWJCBL = pmodel.DWJCBL,
                    DWZH = pmodel.DWZH,
                    GRCKZHHM = pmodel.GRCKZHHM,
                    GRCKZHKHYHDM = pmodel.GRCKZHKHYHDM,
                    GRCKZHKHMC = pmodel.GRCKZHKHMC,
                    GRJCJS = pmodel.GRJCJS,
                    GRYJCE = pmodel.GRYJCE,
                    GRZHZT = GrzhztConst.正常,
                    QJRQ = pmodel.QJRQ,
                    SJHM = pmodel.SJHM,
                    XINGBIE = pmodel.XINGBIE,
                    XINGMING = pmodel.XINGMING,
                    YWYD = pmodel.YWYD,
                    ZJHM = pmodel.ZJHM,
                    ZJLX = pmodel.ZJLX,
                };

                action += () =>
                {
                    sugarHelper.Add(grhk_Item);
                    _flowProcService.AddFlowProc(grkh.YWLSH, ywid, pmodel.DWZH, nameof(GjjOptType.个人开户), optName, OptStatusConst.新建, sugarHelper: sugarHelper);
                };

                sugarHelper.InvokeTransactionScope(action);
            }
            else if (model.STATUS == OptStatusConst.新建)
            {
                if (model.DWZH == pmodel.DWZH)
                {
                    return (ApiResultCodeConst.ERROR, $"当前用户（{pmodel.ZJHM}）在当前单位已有录入保存状态的数据", string.Empty);
                }
                return (ApiResultCodeConst.ERROR, $"当前用户（{pmodel.ZJHM}）在其他单位已有录入保存状态的数据", string.Empty);
            }
            else
            {
                return (ApiResultCodeConst.ERROR, $"当前用户（{pmodel.ZJHM}）已开户，请勿重复操作", string.Empty);
            }

            return (ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, grkh.YWLSH);
        }


        /// <summary>
        /// 根据证件号码，开户类型获取个人开户数据
        /// </summary>
        /// <param name="zjhm"></param>
        /// <param name="KhtypeConst"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        public v_Busi_PersonInfo GetCreatedGrhk(string zjhm,int KhtypeConst,string city_cent)
        {
            //查是否已有录入状态的账户
            var model = SugarHelper.Instance().QueryMuch<D_GRKH, D_GRKH_ITEM, v_Busi_PersonInfo>(
                (t1, t2) => new object[] { JoinType.Left, t1.YWLSH == t2.YWLSH },
                (t1, t2) => new v_Busi_PersonInfo
                {
                    STATUS = t1.STATUS,
                    CSNY = t2.CSNY,
                    ZJHM = t2.ZJHM,
                    DWJCBL = t2.DWJCBL,
                    DWZH = t2.DWZH,
                    GRCKZHHM = t2.GRCKZHHM,
                    GRCKZHKHMC = t2.GRCKZHKHMC,
                    GRCKZHKHYHDM = t2.GRCKZHKHYHDM,
                    GRJCJS = t2.GRJCJS,
                    GRYJCE = t2.GRYJCE,
                    GRZHZT = t2.GRZHZT,
                    KHTYPE = t1.KHTYPE,
                    QJRQ = t2.QJRQ,
                    SJHM = t2.SJHM,
                    XINGBIE = t2.XINGBIE,
                    XINGMING = t2.XINGMING,
                    YWLSH = t2.YWLSH,
                    YWYD = t2.YWYD,
                    ZJLX = t2.ZJLX
                },
                (t1, t2) => t2.ZJHM == zjhm && t1.CITY_CENTNO == city_cent && t1.KHTYPE == KhtypeConst).FirstOrDefault();
            return model;
        }

    }
}
