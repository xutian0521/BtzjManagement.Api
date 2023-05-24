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
        BusiCorporationService _busiCorporationService;
        FlowProcService _flowProcService;
        public CorporationService(BusiCorporationService busiCorporationService, FlowProcService flowProcService)
        {
            _busiCorporationService = busiCorporationService;
            _flowProcService = flowProcService;
        }

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
        public v_ApiResult CreateUpdateCorporation(P_In_Corporation_Add pmodel,string city_cent,string optMan)
        {
            v_ApiResult result = new v_ApiResult() { Code= ApiResultCodeConst.ERROR };
            var sugarHelper = SugarHelper.Instance();
           

            if (string.IsNullOrEmpty(pmodel.DWMC))
            {
                result.Message = "单位名称不能为空";
                return result;
            }

            if (string.IsNullOrEmpty(pmodel.USCCID))
            {
                result.Message = "统一信用代码不能为空";
                return result;
            }

            var status = new string[] { OptStatusConst.新建, OptStatusConst.已归档 };
            Action action = null;
            //判断是修改还是新增-是否有新建状态的数据
            var busiModel = sugarHelper.First<D_BUSI_CORPORATION>(x => x.UNIQUE_KEY.ToUpper() == pmodel.USCCID.Trim().ToUpper() && status.Contains(x.STATUS) && x.CITY_CENTNO == city_cent);
            if (busiModel == null)//说明是新增
            {
                var ywlsh = Common.UniqueYwlsh();
                var newDwzhInt = NextDwzhInt();
                var dwzh = Common.PaddingDwzh(newDwzhInt, 4);
                var custId = Common.PaddingDwzh(newDwzhInt, 8);

                if (sugarHelper.IsExist<D_CORPORATION_BASICINFO>(x => x.USCCID.ToUpper() == pmodel.USCCID.Trim().ToUpper() && x.CITY_CENTNO == city_cent))
                {
                    result.Message = "当前单位名称已存在";
                    return result;
                }

                //先向业务表写数据
                action += () => _busiCorporationService.AddBusiCorporation(city_cent, ywlsh, pmodel.USCCID, GjjOptType.单位开户, optMan, sugarHelper: sugarHelper);

                //单位基本信息
                D_CORPORATION_BASICINFO basicModel = new D_CORPORATION_BASICINFO
                {
                    BASICACCTBRCH = pmodel.BASICACCTBRCH,
                    BASICACCTMC = pmodel.BASICACCTMC,//后面处理根据
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

                action += () => { var id = sugarHelper.Add(basicModel); sugarHelper.Add(acctModel); _flowProcService.AddFlowProc(ywlsh, id, dwzh, nameof(GjjOptType.单位开户), optMan, OptStatusConst.新建, sugarHelper: sugarHelper); };
                sugarHelper.InvokeTransactionScope(action);
                result.Content = ywlsh;
            }
            else if (busiModel.STATUS == OptStatusConst.新建)//修改
            {
                //更细业务表
                busiModel.CREATE_MAN = optMan;
                busiModel.CREATE_TIME = DateTime.Now;
                action += () => sugarHelper.Update(busiModel);
                //更新单位基本信息表
                var basicDwModel = sugarHelper.First<D_CORPORATION_BASICINFO>(x => x.USCCID.ToUpper() == pmodel.USCCID.ToUpper() && x.CITY_CENTNO == city_cent);
                basicDwModel.BASICACCTBRCH = pmodel.BASICACCTBRCH;
                basicDwModel.BASICACCTMC = pmodel.BASICACCTMC;//后面处理根据
                basicDwModel.BASICACCTNO = pmodel.BASICACCTNO;
                basicDwModel.DWDZ = pmodel.DWDZ;
                basicDwModel.DWFRDBXM = pmodel.DWFRDBXM;
                basicDwModel.DWFRDBZJHM = pmodel.DWFRDBZJHM;
                basicDwModel.DWFRDBZJLX = pmodel.DWFRDBZJLX;
                basicDwModel.DWFXR = pmodel.DWFXR;
                basicDwModel.DWMC = pmodel.DWMC;
                basicDwModel.DWSLRQ = pmodel.DWSLRQ /*Common.StringToDate(pmodel.DWSLRQ)*/;
                basicDwModel.DWXZ = pmodel.DWXZ;
                basicDwModel.DWYB = pmodel.DWYB;
                basicDwModel.JBRGDDHHM = pmodel.JBRGDDHHM;
                basicDwModel.JBRSJHM = pmodel.JBRSJHM;
                basicDwModel.JBRXM = pmodel.JBRXM;
                basicDwModel.JBRZJHM = pmodel.JBRZJHM;
                basicDwModel.JBRZJLX = pmodel.JBRZJLX;
                basicDwModel.USCCID = pmodel.USCCID;
                action += () => sugarHelper.Update(basicDwModel);

                //更新单位账户信息表
                var acctDwModel = sugarHelper.First<D_CORPORATION_ACCTINFO>(x => x.CUSTID == basicDwModel.CUSTID && x.CITY_CENTNO == city_cent);
                acctDwModel.CALC_METHOD = pmodel.CALC_METHOD;
                acctDwModel.DWFCRS = 0;
                acctDwModel.DWJCBL = pmodel.DWJCBL;
                acctDwModel.DWJCRS = 0;
                acctDwModel.DWZGRS = 0;
                acctDwModel.FROMFLAG = pmodel.FROMFLAG;
                acctDwModel.FACTINCOME = 0;
                acctDwModel.MONTHPAYTOTALAMT = 0;
                acctDwModel.NEXTPAYMTH = pmodel.NEXTPAYMTH;
                acctDwModel.REGHANDBAL = 0;
                acctDwModel.STYHZH = pmodel.STYHZH;
                acctDwModel.STYHDM = pmodel.STYHDM;
                acctDwModel.STYHMC = pmodel.STYHMC;
                acctDwModel.DWZHYE = 0;
                acctDwModel.DWZHZT = DwzhztConst.正常;
                action += () => sugarHelper.Update(acctDwModel);
                action+=() => _flowProcService.AddFlowProc(busiModel.YWLSH, basicDwModel.ID, acctDwModel.DWZH, nameof(GjjOptType.单位开户), optMan, OptStatusConst.修改, sugarHelper: sugarHelper);
                sugarHelper.InvokeTransactionScope(action);
                result.Content = busiModel.YWLSH;
            }
            else // 已使用该统一信用代码开过户
            {
                result.Message = $"当前统一信用代码（{pmodel.USCCID}）已开过户";
                return result;
            }
            result.Code = ApiResultCodeConst.SUCCESS;
            return result;
        }

        /// <summary>
        /// 单位信息筛选下拉列表
        /// </summary>
        /// <param name="dwzh"></param>
        /// <param name="dwmc"></param>
        /// <returns></returns>
        public List<v_CorporationSelect> CorporationSelectList(string serchKey, string city_cent)
        {
            var dwzhzts = new string[] { DwzhztConst.正常, DwzhztConst.封存 };
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>> where = (t1, t2) => dwzhzts.Contains(t2.DWZHZT) && t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent; ;
            if (!string.IsNullOrEmpty(serchKey))
            {
                where = (t1, t2) => dwzhzts.Contains(t2.DWZHZT) && t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent && (t1.DWMC.Contains(serchKey) || t2.DWZH.Contains(serchKey) || t1.USCCID.Contains(serchKey));
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
            v_CorporationCreated contentModel = new v_CorporationCreated();
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR, Content = contentModel };
            var status = new string[] { OptStatusConst.新建, OptStatusConst.已归档 };
            if (string.IsNullOrEmpty(tyxydm))
            {
                result.Code = ApiResultCodeConst.SUCCESS;
                return result;
            }

            //业务数据-业务数据
            var bus_model = SugarHelper.Instance().First<D_BUSI_CORPORATION>(x => x.UNIQUE_KEY.ToUpper() == tyxydm.ToUpper() && x.CITY_CENTNO == city_cent && x.BUSITYPE == GjjOptType.单位开户 && status.Contains(x.STATUS));

            if(bus_model == null)//不存在保存的数据
            {
                result.Code = ApiResultCodeConst.SUCCESS;
                return result;
            }

            //基本信息和账户信息
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>> where = null;
            where = (t1, t2) => t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent && t1.USCCID.ToUpper() == tyxydm.ToUpper();

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
                result.Message = $"当前统一信用代码（{tyxydm}）已开过户";
            }
            contentModel.BaseModel = baseModel;
            contentModel.YWLSH = bus_model.YWLSH;

            //影像信息
            result.Content = contentModel;
            return result;
        }

    }
}
