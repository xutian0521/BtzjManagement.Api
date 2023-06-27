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
    /// <summary>
    /// 
    /// </summary>
    public class CorporationService
    {
        FlowProcService _flowProcService;
        RuleService _ruleService;
        private readonly IServiceProvider _serviceProvider;
        private readonly object lockObject = new object();

        /// <summary>
        /// 能录入和修改的状态
        /// </summary>
        List<string> statusListCanAddUpdate = new List<string> { OptStatusConst.新建, OptStatusConst.终审退回, OptStatusConst.初审退回 };
        /// <summary>
        /// 业务在途的状态-此时不能修改
        /// </summary>
        List<string> statusListProcess = new List<string> { OptStatusConst.初审出错, OptStatusConst.等待初审, OptStatusConst.终审出错, OptStatusConst.等待终审 };

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="ruleService"></param>
        /// <param name="flowProcService"></param>
        public CorporationService(IServiceProvider serviceProvider, FlowProcService flowProcService, RuleService ruleService)
        {
            _flowProcService = flowProcService;
            _ruleService = ruleService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取最大单位账号数字
        /// </summary>
        /// <returns></returns>
        public int NextDwzhInt()
        {
            var maxDwzh = SugarHelper.Instance().Max<D_CORPORATION_ACCTINFO, int>("DWZH");
            return maxDwzh + 1;
        }

        /// <summary>
        /// 按月汇缴单位开户
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="city_cent"></param>
        /// <param name="optMan"></param>
        /// <returns></returns>
        public v_ApiResult CreateUpdateCorporation(P_In_Corporation_Add pmodel, string city_cent, string optMan)
        {
            v_ApiResult result = new v_ApiResult() { Code = ApiResultCodeConst.ERROR };
            var sugarHelper = SugarHelper.Instance();

            if (string.IsNullOrEmpty(pmodel.DWMC))
            {
                result.Message = "单位名称不能为空";
                return result;
            }

            if (string.IsNullOrEmpty(pmodel.ZZJGDM))
            {
                result.Message = "统一信用代码不能为空";
                return result;
            }

            var status = new string[] { OptStatusConst.新建, OptStatusConst.已归档 };
            Action action = null;
            //判断是修改还是新增-是否有新建状态的数据
            var busiModel = sugarHelper.First<D_BUSI_CORPORATION>(x => x.UNIQUE_KEY.ToUpper() == pmodel.ZZJGDM.Trim().ToUpper() && status.Contains(x.STATUS) && x.CITY_CENTNO == city_cent && x.BUSITYPE == GjjOptType.单位开户);
            if (busiModel == null)//说明是新增开户
            {
                var ywlsh = Common.UniqueYwlsh();
                var newDwzhInt = NextDwzhInt();
                var dwzh = Common.PaddingLeftZero(newDwzhInt, 4);
                var custId = Common.PaddingLeftZero(newDwzhInt, 8);

                if (sugarHelper.IsExist<D_CORPORATION_BASICINFO>(x => x.ZZJGDM.ToUpper() == pmodel.ZZJGDM.Trim().ToUpper() && x.CITY_CENTNO == city_cent))
                {
                    result.Message = "当前单位名称已存在";
                    return result;
                }

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
                    DWMCSX = Common.ConvertChineseToPinYinShouZiMu(pmodel.DWMC),
                    DWSLRQ = pmodel.DWSLRQ,
                    DWXZ = pmodel.DWXZ,
                    DWYB = pmodel.DWYB,
                    JBRGDDHHM = pmodel.JBRGDDHHM,
                    JBRSJHM = pmodel.JBRSJHM,
                    JBRXM = pmodel.JBRXM,
                    JBRZJHM = pmodel.JBRZJHM,
                    JBRZJLX = pmodel.JBRZJLX,
                    ZZJGDM = pmodel.ZZJGDM,
                    CITY_CENTNO = city_cent,
                    OPERID = optMan
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
                    OPENPERSALREADY = OpenPerSalReadyConst.未开户,
                    GRJCBL = 0
                };

                action += () =>
                {
                    //写业务数据
                    D_BUSI_CORPORATION model = new D_BUSI_CORPORATION
                    {
                        CITY_CENTNO = city_cent,
                        YWLSH = ywlsh,
                        UNIQUE_KEY = pmodel.ZZJGDM,
                        BUSITYPE = GjjOptType.单位开户,
                        STATUS = OptStatusConst.新建,
                        CREATE_MAN = optMan,
                        CREATE_TIME = DateTime.Now,
                    };
                    var id = sugarHelper.AddReturnIdentity(model);
                    sugarHelper.Add(basicModel); //写单位基本信息
                    sugarHelper.Add(acctModel);//写单位账户信息
                    _flowProcService.AddFlowProc(ywlsh, id, dwzh, nameof(GjjOptType.单位开户), optMan, OptStatusConst.新建, sugarHelper: sugarHelper);
                };
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
                var basicDwModel = sugarHelper.First<D_CORPORATION_BASICINFO>(x => x.ZZJGDM.ToUpper() == pmodel.ZZJGDM.ToUpper() && x.CITY_CENTNO == city_cent);
                basicDwModel.BASICACCTBRCH = pmodel.BASICACCTBRCH;
                basicDwModel.BASICACCTMC = pmodel.BASICACCTMC;//后面处理根据
                basicDwModel.BASICACCTNO = pmodel.BASICACCTNO;
                basicDwModel.DWDZ = pmodel.DWDZ;
                basicDwModel.DWFRDBXM = pmodel.DWFRDBXM;
                basicDwModel.DWFRDBZJHM = pmodel.DWFRDBZJHM;
                basicDwModel.DWFRDBZJLX = pmodel.DWFRDBZJLX;
                basicDwModel.DWFXR = pmodel.DWFXR;
                basicDwModel.DWMC = pmodel.DWMC;
                basicDwModel.DWMCSX = Common.ConvertChineseToPinYinShouZiMu(pmodel.DWMC);
                basicDwModel.DWSLRQ = pmodel.DWSLRQ /*Common.StringToDate(pmodel.DWSLRQ)*/;
                basicDwModel.DWXZ = pmodel.DWXZ;
                basicDwModel.DWYB = pmodel.DWYB;
                basicDwModel.JBRGDDHHM = pmodel.JBRGDDHHM;
                basicDwModel.JBRSJHM = pmodel.JBRSJHM;
                basicDwModel.JBRXM = pmodel.JBRXM;
                basicDwModel.JBRZJHM = pmodel.JBRZJHM;
                basicDwModel.JBRZJLX = pmodel.JBRZJLX;
                basicDwModel.ZZJGDM = pmodel.ZZJGDM;
                basicDwModel.OPERID = optMan;
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
                acctDwModel.GRJCBL = 0;
                action += () => sugarHelper.Update(acctDwModel);
                action += () => _flowProcService.AddFlowProc(busiModel.YWLSH, busiModel.ID, acctDwModel.DWZH, nameof(GjjOptType.单位开户), optMan, OptStatusConst.修改, sugarHelper: sugarHelper);
                sugarHelper.InvokeTransactionScope(action);
                result.Content = busiModel.YWLSH;
            }
            else // 已使用该统一信用代码开过户
            {
                result.Message = $"当前统一信用代码（{pmodel.ZZJGDM}）已开过户";
                return result;
            }
            result.Code = ApiResultCodeConst.SUCCESS;
            return result;
        }

        /// <summary>
        /// 单位信息筛选下拉列表
        /// </summary>
        /// <param name="city_cent"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public List<v_CorporationSelect> CorporationSelectList(string city_cent, string searchKey)
        {
            var dwzhzts = new string[] { DwzhztConst.正常, DwzhztConst.封存 };
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>> where = (t1, t2) => dwzhzts.Contains(t2.DWZHZT) && t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent;

            List<(bool, Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>>)> wherif = new List<(bool, Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>>)>(); ;
            wherif.Add(new(!string.IsNullOrEmpty(searchKey), (t1, t2) => t2.CITY_CENTNO == city_cent && (t1.DWMC.Contains(searchKey) || t2.DWZH.Contains(searchKey) || t1.ZZJGDM.Contains(searchKey) || t1.DWMCSX.Contains(searchKey))));
            var list = SugarHelper.Instance().QueryMuch<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, v_CorporationSelect>(
                (t1, t2) => new object[] { JoinType.Inner, t1.CUSTID == t2.CUSTID },
                (t1, t2) => new v_CorporationSelect
                {
                    DWMC = t1.DWMC,
                    DWZH = t2.DWZH,
                    DWJCBL = t2.DWJCBL,
                    NEXTPAYMTH = t2.NEXTPAYMTH.Value.ToString("yyyy-MM-dd"),
                    ZZJGDM = t1.ZZJGDM,
                    GRJCBL = t2.GRJCBL,
                    CALC_METHOD = t2.CALC_METHOD,
                    JZNY = t2.JZNY,
                },
                where, wherif);

            //var llll = SugarSimple.Instance().Queryable<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO>((t1, t2) => new object[] { JoinType.Inner, t1.CUSTID == t2.CUSTID })
            //    .Where((t1, t2) => dwzhzts.Contains(t2.DWZHZT) && t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent)
            //    .WhereIF(!string.IsNullOrEmpty(searchKey), (t1, t2) => t2.CITY_CENTNO == city_cent && (t1.DWMC.Contains(searchKey) || t2.DWZH.Contains(searchKey) || t1.USCCID.Contains(searchKey)))
            //    .Select((t1, t2) => new v_CorporationSelect() { DWMC = t1.DWMC, DWZH = t2.DWZH }).ToList();

            return list;
        }

        /// <summary>
        /// 获取保存状态的数据
        /// </summary>
        /// <param name="tyxydm"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        public v_ApiResult CorporationCreatedModel(string tyxydm, string city_cent)
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

            if (bus_model == null)//不存在保存的数据
            {
                result.Code = ApiResultCodeConst.SUCCESS;
                return result;
            }

            if (bus_model.STATUS != OptStatusConst.新建)//说明已经有在途的业务
            {
                result.Message = $"当前统一信用代码（{tyxydm}）已开过户";
                return result;
            }

            //基本信息和账户信息
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>> where = null;
            where = (t1, t2) => t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent && t1.ZZJGDM.ToUpper() == tyxydm.ToUpper();
            var baseModel = GetCorporatiorn(where);

            contentModel.BaseModel = baseModel;
            contentModel.YWLSH = bus_model.YWLSH;
            result.Code = ApiResultCodeConst.SUCCESS;
            //影像信息
            result.Content = contentModel;
            return result;
        }

        /// <summary>
        /// 提交单位开户业务
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="optMan"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        public (bool, string) SubmitCorporationCreated(P_In_Corporation_Submit pmodel, string optMan, string city_cent)
        {
            if (pmodel == null || string.IsNullOrEmpty(pmodel.ywlsh))
            {
                return (false, "请先录入相关业务再提交");
            }
            //先查要提交的数据状态
            var sugarHelper = SugarHelper.Instance();
            var busiModel = sugarHelper.First<D_BUSI_CORPORATION>(x => x.CITY_CENTNO == city_cent && x.YWLSH == pmodel.ywlsh && x.BUSITYPE == GjjOptType.单位开户);
            if (busiModel == null)
            {
                return (false, "未查询到相关业务");
            }

            if (busiModel.STATUS == OptStatusConst.已归档)
            {
                return (false, "当前业务已提交成功，请勿重复提交");
            }

            Action action = null;

            //提交流程
            busiModel.STATUS = OptStatusConst.已归档;
            busiModel.SUBMIT_MAN = optMan;
            busiModel.SUBMIT_TIME = DateTime.Now;
            busiModel.VERIFY_MAN = optMan;
            busiModel.VERIFY_TIME = DateTime.Now;
            var sys_time = _ruleService.GetSysconfig(city_cent).model.DT_SYSTEM;
            busiModel.SYSTEM_TIME = sys_time;
            action += () => sugarHelper.Update(busiModel);

            //更新单位基本信息表
            var basicDwModel = sugarHelper.First<D_CORPORATION_BASICINFO>(x => x.ZZJGDM.ToUpper() == busiModel.UNIQUE_KEY.ToUpper() && x.CITY_CENTNO == city_cent);
            if (basicDwModel == null)
            {
                return (false, "当前业务对应的单位基本信息有误，提交失败");
            }
            basicDwModel.DWKHRQ = DateTime.Now;
            basicDwModel.OPERID = optMan;
            action += () => sugarHelper.Update(basicDwModel);

            //更新单位账户信息表
            var acctDwModel = sugarHelper.First<D_CORPORATION_ACCTINFO>(x => x.CUSTID == basicDwModel.CUSTID && x.CITY_CENTNO == city_cent);
            if (acctDwModel == null)
            {
                return (false, "当前业务对应的单位账户信息有误，提交失败");
            }

            //写入业务流程数据
            action += () => _flowProcService.AddFlowProc(pmodel.ywlsh, busiModel.ID, acctDwModel.DWZH, nameof(GjjOptType.单位开户), optMan, OptStatusConst.已归档, sugarHelper: sugarHelper);
            sugarHelper.InvokeTransactionScope(action);
            return (true, "提交成功");
        }

        /// <summary>
        /// 获取单位按月汇缴初始化相关数据
        /// </summary>
        /// <param name="city_cent"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dwzh"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public v_PaymentMonthInit DwHjMonthInfo(string city_cent, string dwzh/*, int pageIndex,int pageSize,string searchKey = ""*/)
        {
            v_PaymentMonthInit result = new v_PaymentMonthInit { };
            //单位人数，单位月缴存额
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>> where = null;
            where = (t1, t2) => t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent && t2.DWZH == dwzh;
            var baseModel = GetCorporatiorn(where);
            result.corporatiornInfo = baseModel;

            //参与按月汇缴用户分页信息
            //PersonInfoService _personInfoService = _serviceProvider.GetService(typeof(PersonInfoService)) as PersonInfoService;
            //var grzhzt = new List<string> { GrzhztConst.正常, GrzhztConst.封存 };
            //var pager = _personInfoService.DwJcbgPersonPageList(city_cent, pageIndex, pageSize, dwzh, searchKey, grzhzt, KhtypeConst.按月汇缴.ToString());
            //result.pager = pager;

            return result;
        }


        /// <summary>
        /// 获取单位相关信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        internal v_BaseCorporatiorn GetCorporatiorn(Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, bool>> where)
        {
            Expression<Func<D_CORPORATION_BASICINFO, D_CORPORATION_ACCTINFO, v_BaseCorporatiorn>> selectExpression = (t1, t2) => new v_BaseCorporatiorn
            {
                DWMC = t1.DWMC,
                ZZJGDM = t1.ZZJGDM,
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
                DWZH = t2.DWZH,
                DWFCRS = t2.DWFCRS,
                MONTHPAYTOTALAMT = t2.MONTHPAYTOTALAMT,
                DWJCRS = t2.DWJCRS,
                DWZGRS = t2.DWZGRS,
                FACTINCOME = t2.FACTINCOME,
                GRJCBL = t2.GRJCBL,
                JZNY = t2.JZNY,
                DWZHYE = t2.DWZHYE,
                REGHANDBAL = t2.REGHANDBAL
            };

            return SugarHelper.Instance().QueryMuch((t1, t2) => new object[] { JoinType.Inner, t1.CUSTID == t2.CUSTID }, selectExpression, where).FirstOrDefault();
        }

        /// <summary>
        /// 按月汇缴核定暂存
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="optMan"></param>
        /// <param name="city_cent"></param>
        public (int code, string msg, string batchNo) MonthHjCreate(P_MonthHjCreate pmodel, string optMan, string city_cent)
        {
            var msg = ApiResultMessageConst.SUCCESS;
            var code = ApiResultCodeConst.SUCCESS;
            lock (lockObject)
            {
                PersonInfoService _personInfoService = _serviceProvider.GetService(typeof(PersonInfoService)) as PersonInfoService;
                var grzhzt = new List<string> { };
                var sugarHelper = SugarHelper.Instance();

                if (pmodel.monthLength < 1)
                {
                    code = ApiResultCodeConst.ERROR;
                    msg = "请输入要汇缴的月数";
                    return (code, msg, string.Empty);
                }

                //先查有没有在途业务(缴存变更，转移，汇缴)，有在途的不能新增
                List<string> PayMthList = new List<string>();
                for (int i = 0; i < pmodel.monthLength; i++)
                {
                    PayMthList.Add(Common.CalcPayMonth(pmodel.monthStart, i));
                }

                var dwjcList = MonthDwjcModelList(pmodel.dwzh, pmodel.monthStart.Replace("-", ""), city_cent);

                var listProcess = dwjcList.Where(x => statusListProcess.Contains(x.STATUS) || x.STATUS == OptStatusConst.已归档).Select(x => $"业务月度({x.PAYMTH})的汇缴业务为{EnumHelper.GetEnumItemByValue<string>(typeof(OptStatusConst), x.STATUS).key}");
                if (listProcess.Count() > 0)
                {
                    return (ApiResultCodeConst.ERROR, string.Join("!", listProcess), string.Empty);
                }

                //查单位信息
                var dwInfo = GetCorporatiorn((t1, t2) => t2.DWZH == pmodel.dwzh && t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent);
                if (dwInfo.NEXTPAYMTH.Value.ToString("yyyy-MM") != pmodel.monthStart)
                {
                    code = ApiResultCodeConst.ERROR;
                    msg = $"汇缴起始月{pmodel.monthStart}和单位的当前业务月度{dwInfo.NEXTPAYMTH.Value.ToString("yyyy-MM")}不一致！";
                    return (code, msg, string.Empty);
                }

                //从个人账户信息获取该单位月缴存额
                var payAmtAcct = GetPayAmtByCustomerAcct(pmodel.dwzh, city_cent);

                if (payAmtAcct * pmodel.monthLength != pmodel.payamt)
                {
                    code = ApiResultCodeConst.ERROR;
                    msg = $"汇缴金额({pmodel.payamt})和单位实际应缴金额({payAmtAcct * pmodel.monthLength})不一致！";
                    return (code, msg, string.Empty);
                }


                //有暂存数据再次暂存直接删除再重新写数据
                Action action = null;
                List<string> delYwlsh = dwjcList.Select(x => x.YWLSH).ToList();
                if (delYwlsh.Count > 0)
                {
                    action += () =>
                    {
                        //删除清册
                        sugarHelper.Delete<D_MONTH_DWJCQC>(x => delYwlsh.Contains(x.YWLSH));
                        //删除主表
                        sugarHelper.Delete<D_MONTH_DWJC>(x => delYwlsh.Contains(x.YWLSH));
                        //清除之前的日志
                        sugarHelper.Delete<D_FLOWPROC>(x => delYwlsh.Contains(x.YWLSH));

                        //写入日志
                        //_flowProcService.AddFlowProc(item.YWLSH, item.ID, dwzh, nameof(GjjOptType.单位汇缴), optMan, OptStatusConst.删除, sugarHelper: sugarHelper, memo: $"单位({dwzh})业务月度({payamt}) 汇缴暂存数据清除(重新核定)");
                    };
                    sugarHelper.InvokeTransactionScope(action);
                }

                //获取单位当前月的用户数据
                var personList = _personInfoService.DwJcbgPersonPageList(city_cent, 1, int.MaxValue, pmodel.dwzh, string.Empty, grzhzt, KhtypeConst.按月汇缴.ToString());
                var batch = Common.UniqueYwlsh();//批次号
                //写入清册                              
                foreach (var payMth in PayMthList)
                {
                    action = null;
                    //获取上月的汇缴数据
                    var lastPayMth = Common.CalcLastPayMonth(payMth);
                    var lastMonthHj = sugarHelper.First<D_MONTH_DWJC>(x => x.CITY_CENTNO == city_cent && x.DWZH == pmodel.dwzh && x.PAYMTH == lastPayMth);
                    if (lastMonthHj == null)
                    {
                        lastMonthHj = new D_MONTH_DWJC
                        {
                            MTHPAYAMT = dwInfo.MONTHPAYTOTALAMT,
                            DWJCRS = dwInfo.DWJCRS
                        };
                    }

                    var ywlsh = Common.UniqueYwlsh();
                    //主表
                    D_MONTH_DWJC dwjc = new D_MONTH_DWJC
                    {
                        YWLSH = ywlsh,
                        BATCHNO = batch,
                        CITY_CENTNO = city_cent,
                        CREATE_MAN = optMan,
                        CREATE_TIME = DateTime.Now,
                        DWZH = pmodel.dwzh,
                        STATUS = OptStatusConst.新建,
                        DWJCBL = dwInfo.DWJCBL,
                        DWJCRS = dwInfo.DWJCRS,
                        GRJCBL = dwInfo.GRJCBL,
                        MTHPAYAMT = payAmtAcct,
                        PAYMTH = payMth,
                        //todo: 做了缴存变更后赋值
                        BASECHGAMT = 0,
                        BASECHGNUM = 0,
                        LASTMTHPAY = lastMonthHj.MTHPAYAMT,
                        LASTMTHPAYNUM = lastMonthHj.DWJCRS,
                        MTHPAYAMTMNS = payAmtAcct < lastMonthHj.MTHPAYAMT ? lastMonthHj.MTHPAYAMT - payAmtAcct : 0,
                        MTHPAYAMTPLS = payAmtAcct > lastMonthHj.MTHPAYAMT ? payAmtAcct - lastMonthHj.MTHPAYAMT : 0,
                        MTHPAYNUMMNS = dwInfo.DWJCRS < lastMonthHj.DWJCRS ? lastMonthHj.DWJCRS - dwInfo.DWJCRS : 0,
                        MTHPAYNUMPLS = dwInfo.DWJCRS > lastMonthHj.DWJCRS ? dwInfo.DWJCRS - lastMonthHj.DWJCRS : 0,
                    };
                    //清册
                    List<D_MONTH_DWJCQC> qcList = personList.list.Select(x => new D_MONTH_DWJCQC
                    {
                        DWJCBL = x.DWJCBL,
                        DWYJCE = x.DWYJCE,
                        DWZH = pmodel.dwzh,
                        GRJCBL = x.GRJCBL,
                        GRJCJS = x.GRJCJS,
                        GRYJCE = x.GRYJCE,
                        GRZH = x.GRZH,
                        PAYMTH = payMth,
                        REMITPAYAMT = x.DWYJCE + x.GRYJCE,
                        XINGMING = x.XINGMING,
                        YWLSH = ywlsh,
                        ZJHM = x.ZJHM
                    }).ToList();

                    action += () =>
                    {
                        var id = sugarHelper.AddReturnIdentity(dwjc);
                        sugarHelper.Add(qcList);
                        //写入日志
                        _flowProcService.AddFlowProc(ywlsh, id, pmodel.dwzh, nameof(GjjOptType.单位汇缴), optMan, OptStatusConst.新建, sugarHelper: sugarHelper, memo: $"单位({pmodel.dwzh}) 业务月度({pmodel.payamt})汇缴暂存数据新增");
                    };
                    sugarHelper.InvokeTransactionScope(action);//因为需要查询上月的数据，所有事务放在循环里面执行
                }
                
                return (code, msg, batch);
            }
        }

        /// <summary>
        /// 按月汇缴暂存数据删除
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="optMan"></param>
        /// <param name="city_cent"></param>
        public (int code,string msg) MonthHjModelDel(P_MonthHjModelDel pmodel, string optMan, string city_cent)
        {
            List<(bool, Expression<Func<D_MONTH_DWJC, bool>>)> whereIf = new List<(bool, Expression<Func<D_MONTH_DWJC, bool>>)>();
            whereIf.Add(new(!string.IsNullOrEmpty(pmodel.ywlsh), x => x.YWLSH == pmodel.ywlsh));

            var sugarHelper = SugarHelper.Instance();
            //查对应的数据
            List<D_MONTH_DWJC> hjList = sugarHelper.QueryWhereList<D_MONTH_DWJC>(x => x.DWZH == pmodel.dwzh && x.CITY_CENTNO == city_cent && x.BATCHNO == pmodel.batchNo, whereIf: whereIf);

            if (hjList.Count == 0)
            {
                return (ApiResultCodeConst.ERROR, "未查询到相关汇缴业务数据");
            }

            var listProcess = hjList.Where(x => statusListProcess.Contains(x.STATUS) || x.STATUS == OptStatusConst.已归档).Select(x => $"业务月度({x.PAYMTH})的汇缴业务为{EnumHelper.GetEnumItemByValue<string>(typeof(OptStatusConst), x.STATUS).key}");
            if (listProcess.Count() > 0)
            {
                return (ApiResultCodeConst.ERROR,string.Join("!", listProcess));
            }

            Action action = null;
            foreach (var item in hjList)
            {
                action += () =>
                {
                    sugarHelper.Delete(item);//删除业务表
                    sugarHelper.Delete<D_MONTH_DWJCQC>(x => x.YWLSH == item.YWLSH);//删除清册
                    //记录日志
                    _flowProcService.AddFlowProc(item.YWLSH, item.ID, item.DWZH, nameof(GjjOptType.单位汇缴), optMan, OptStatusConst.删除, $"撤销业务月度({item.PAYMTH})汇缴数据", sugarHelper);
                };
            }
            sugarHelper.InvokeTransactionScope(action);

            return (ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS);
        }

        /// <summary>
        /// 按月汇缴暂存数据提交
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="optMan"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        public (int code, string msg) MonthHjModelSubmit(P_MonthHjModelSubmit pmodel, string optMan, string city_cent)
        {
            var sugarHelper = SugarHelper.Instance();
            //查对应的数据
            List<D_MONTH_DWJC> hjList = sugarHelper.QueryWhereList<D_MONTH_DWJC>(x => x.DWZH == pmodel.dwzh && x.CITY_CENTNO == city_cent && x.BATCHNO == pmodel.batchNo);

            if(hjList.Count == 0)
            {
                return (ApiResultCodeConst.ERROR, "未查询到需要提交的汇缴业务数据");
            }

            var listProcess = hjList.Where(x => statusListProcess.Contains(x.STATUS) || x.STATUS == OptStatusConst.已归档).Select(x => $"业务月度({x.PAYMTH})的汇缴业务为{EnumHelper.GetEnumItemByValue<string>(typeof(OptStatusConst), x.STATUS).key}");
            if (listProcess.Count() > 0)
            {
                return (ApiResultCodeConst.ERROR, string.Join("!", listProcess));
            }

            Action action = null;
            foreach (var item in hjList)
            {
                item.STATUS = OptStatusConst.等待初审;
                item.SUBMIT_MAN = optMan;
                item.SUBMIT_TIME = DateTime.Now;

                action += () =>
                {
                    sugarHelper.Update(item);//更新业务表数据

                    //记录日志
                    _flowProcService.AddFlowProc(item.YWLSH, item.ID, item.DWZH, nameof(GjjOptType.单位汇缴), optMan, OptStatusConst.等待初审, sugarHelper:sugarHelper);
                };
            }
            sugarHelper.InvokeTransactionScope(action);

            return (ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS);
        }

        /// <summary>
        /// 获取按月缴存业务数据
        /// </summary>
        /// <param name="dwzh"></param>
        /// <param name="PayMthList"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        internal List<D_MONTH_DWJC> MonthDwjcModelList(string dwzh, string PayMth, string city_cent)
        {
            return SugarHelper.Instance().QueryWhereList<D_MONTH_DWJC>(x => x.CITY_CENTNO == city_cent && x.DWZH == dwzh && Convert.ToInt32(x.PAYMTH) >= Convert.ToInt32(PayMth));
        }


        /// <summary>
        /// 根据单位账号和业务状态获取对应的按月汇缴业务数据
        /// </summary>
        /// <param name="statusType"></param>
        /// <param name="city_cent"></param>
        /// <param name="dwzh"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Pager<v_MonthHj> MonthDwjcModelPageList(string statusType, string city_cent, string dwzh, int pageIndex, int pageSize)
        {
            List<string> statusList = new List<string>() { "test" };//不存在的状态
            List<OrderByClause> orders = new List<OrderByClause> { new OrderByClause { Order = OrderSequence.Asc, Sort = "PAYMTH" } }; ;
            if (statusType == "wtj")
            {
                statusList.AddRange(statusListCanAddUpdate);
            }
            else if (statusType == "process")
            {
                statusList.AddRange(statusListProcess);
            }
            else if (statusType == "complete")
            {
                statusList.Add(OptStatusConst.已归档);
                orders = new List<OrderByClause> { new OrderByClause { Order = OrderSequence.Desc, Sort = "PAYMTH" } };
            }
            else if (statusType == "all")
            {
                statusList = new List<string>();
            }

            List<(bool, Expression<Func<D_MONTH_DWJC, bool>>)> whereIf = new List<(bool, Expression<Func<D_MONTH_DWJC, bool>>)>();
            whereIf.Add(new(statusList != null && statusList.Count > 0, t1 => statusList.Contains(t1.STATUS)));

            int totalCnt = 0;
            Pager<v_MonthHj> page = new Pager<v_MonthHj> { total = totalCnt, list = new List<v_MonthHj>() };

            var pageList = SugarHelper.Instance().QueryPageList<D_MONTH_DWJC>(pageIndex, pageSize, out totalCnt,
                (t1) => t1.DWZH == dwzh && t1.CITY_CENTNO == city_cent,
                whereIf: whereIf,
                OrderBys: orders).Select(x => new v_MonthHj
                {
                    BASECHGAMT = x.BASECHGAMT,
                    BATCHNO = x.BATCHNO,
                    BASECHGNUM = x.BASECHGNUM,
                    CREATE_MAN = x.CREATE_MAN,
                    CREATE_TIME = x.CREATE_TIME,
                    VCHRNOS = x.VCHRNOS,
                    VERIFY_MAN = x.VERIFY_MAN,
                    DWJCBL = x.DWJCBL,
                    DWJCRS = x.DWJCRS,
                    DWZH = x.DWZH,
                    GRJCBL = x.GRJCBL,
                    LASTMTHPAY = x.LASTMTHPAY,
                    ID = x.ID,
                    LASTMTHPAYNUM = x.LASTMTHPAYNUM,
                    MEMO = x.MEMO,
                    MTHPAYAMT = x.MTHPAYAMT,
                    MTHPAYAMTMNS = x.MTHPAYAMTMNS,
                    MTHPAYAMTPLS = x.MTHPAYAMTPLS,
                    MTHPAYNUMMNS = x.MTHPAYNUMMNS,
                    MTHPAYNUMPLS = x.MTHPAYNUMPLS,
                    PAYMTH = x.PAYMTH,
                    STATUS = x.STATUS,
                    SUBMIT_MAN = x.SUBMIT_MAN,
                    SUBMIT_TIME = x.SUBMIT_TIME,
                    VERIFY_TIME = x.VERIFY_TIME,
                    YWLSH = x.YWLSH,
                });
            page.total = totalCnt;
            page.list = pageList;
            return page;
        }

        /// <summary>
        /// 根据单位账号查询个人账号表中的月缴存额
        /// </summary>
        /// <param name="dwzh"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        internal decimal GetPayAmtByCustomerAcct(string dwzh, string city_cent)
        {
            return SugarHelper.Instance().QueryWhereList<D_CUSTOMER_ACCTINFO>(x => x.DWZH == dwzh && x.CITY_CENTNO == city_cent && x.GRZHZT == GrzhztConst.正常).Sum(x => x.MONTHPAYAMT);
        }

        /// <summary>
        /// 根据业务流水号获取按月汇缴清册数据
        /// </summary>
        /// <param name="ywlsh"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Pager<D_MONTH_DWJCQC> MonthDwjcQcModelPageList(string ywlsh, int pageIndex, int pageSize)
        {
            List<OrderByClause> orders = new List<OrderByClause> { new OrderByClause { Order = OrderSequence.Asc, Sort = "GRZH" } };

            int totalCnt = 0;
            Pager<D_MONTH_DWJCQC> page = new Pager<D_MONTH_DWJCQC> { total = totalCnt, list = new List<D_MONTH_DWJCQC>() };

            var pageList = SugarHelper.Instance().QueryPageList<D_MONTH_DWJCQC>(pageIndex, pageSize, out totalCnt,
                (t1) => t1.YWLSH == ywlsh,
                OrderBys: orders);
            page.total = totalCnt;
            page.list = pageList;
            return page;
        }
    }
}
