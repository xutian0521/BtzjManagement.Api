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
    public class PersonInfoService
    {
        FlowProcService _flowProcService;
        CorporationService _corporationService;
        RuleService _ruleService;
        /// <summary>
        /// 能录入和修改的状态
        /// </summary>
        List<string> statusListCanAddUpdate = new List<string> { OptStatusConst.新建, OptStatusConst.终审退回,OptStatusConst.初审退回 };
        /// <summary>
        /// 业务在途的状态-此时不能修改
        /// </summary>
        List<string> statusListProcess = new List<string> { OptStatusConst.初审出错, OptStatusConst.等待初审, OptStatusConst.终审出错, OptStatusConst.等待终审 };

        /// <summary>
        /// 个人账户状态-不能新增用户
        /// </summary>
        List<string> GrzhztNotAddGrzhztNotAdd = new List<string> { GrzhztConst.封存, GrzhztConst.正常 };

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="flowProcService"></param>
        /// <param name="corporationService"></param>
        /// <param name="ruleService"></param>
        public PersonInfoService(FlowProcService flowProcService,CorporationService corporationService, RuleService ruleService)
        {
            _flowProcService = flowProcService;
            _corporationService = corporationService;
            _ruleService = ruleService;
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

            //查业务信息中该证件号码是否有开户业务
            var busiKhInfo = BusiHasKH(pmodel.ZJHM, city_cent);
            if (busiKhInfo.isWrong)
            {
                return (ApiResultCodeConst.ERROR, busiKhInfo.msg, string.Empty);
            }

            //查该证件号码是否已有账户
            var custKhInfo = CustHasKH(pmodel.ZJHM, city_cent);
            if (custKhInfo.isWrong)//有问题
            {
                return (ApiResultCodeConst.ERROR, custKhInfo.msg, string.Empty);
            }

            //先查这个单位是否有保存状态的主表数据，如果有接着用这个主表，如果没有就新建主表
            var grkh = sugarHelper.QueryWhereList<D_GRKH>(x => x.DWZH == pmodel.DWZH && x.CITY_CENTNO == city_cent && x.KHTYPE == KhtypeConst.按月汇缴).OrderByDescending(x => x.ID).FirstOrDefault();
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
                if(grkh.STATUS == OptStatusConst.已归档)
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
                if (statusListCanAddUpdate.Contains(grkh.STATUS))//可修改
                {
                    ywid = grkh.ID;
                    grkh.CREATE_MAN = optName;
                    grkh.CREATE_TIME = DateTime.Now;

                    action += () => { sugarHelper.Update(grkh); };
                }
                else if(statusListProcess.Contains(grkh.STATUS))//在途
                {
                    return (ApiResultCodeConst.ERROR, $"当前单位（{pmodel.DWZH}）有未办结的按月缴存个人开户业务，请稍后重试", string.Empty);
                }
               
            }

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
                DWYJCE = pmodel.DWYJCE,
                GDDHHM = pmodel.GDDHHM,
                GRJCBL = pmodel.GRJCBL,
                WORK_DATE = pmodel.WORK_DATE
            };

            action += () =>
            {
                sugarHelper.Add(grhk_Item);
                _flowProcService.AddFlowProc(grkh.YWLSH, ywid, pmodel.DWZH, nameof(GjjOptType.个人开户), optName, OptStatusConst.新建, sugarHelper: sugarHelper);
            };

            sugarHelper.InvokeTransactionScope(action);

            return (ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, grkh.YWLSH);
        }

        /// <summary>
        /// 获取按月汇缴个人开户数据分页
        /// </summary>
        /// <param name="city_cent"></param>
        /// <param name="dwzh"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Pager<v_Busi_Grkh> PersonKhMonthList(string city_cent, string dwzh, int pageIndex, int pageSize, string status = OptStatusConst.新建)
        {
            List<string> ywStatusList = new List<string> { };
            if (status == OptStatusConst.新建)
            {
                ywStatusList.Add(OptStatusConst.新建);
            }

            List<(bool, Expression<Func<D_GRKH, D_GRKH_ITEM, bool>>)> whereIf = new List<(bool, Expression<Func<D_GRKH, D_GRKH_ITEM, bool>>)>();
            whereIf.Add(new(ywStatusList.Count > 0, (t1, t2) => ywStatusList.Contains(t1.STATUS)));

            int totalCnt = 0;
            Pager<v_Busi_Grkh> page = new Pager<v_Busi_Grkh> { total = totalCnt, list = new List<v_Busi_Grkh>() };

            var pageList = SugarHelper.Instance().QueryMuchDescriptorPageList<D_GRKH, D_GRKH_ITEM, v_Busi_Grkh>(pageIndex, pageSize, out totalCnt,
                (t1, t2) => new object[] { JoinType.Left, t1.YWLSH == t2.YWLSH },
                (t1, t2) => new v_Busi_Grkh
                {
                    YWLSH = t1.YWLSH,
                    CSNY = t2.CSNY,
                    DWJCBL = t2.DWJCBL,
                    DWZH = t2.DWZH,
                    GRCKZHHM = t2.GRCKZHHM,
                    GRCKZHKHMC = t2.GRCKZHKHMC,
                    GRCKZHKHYHDM = t2.GRCKZHKHYHDM,
                    GRJCJS = t2.GRJCJS,
                    GRYJCE = t2.GRYJCE,
                    KHTYPE = t1.KHTYPE,
                    QJRQ = t2.QJRQ,
                    SJHM = t2.SJHM,
                    STATUS = t1.STATUS,
                    XINGBIE = t2.XINGBIE,
                    XINGMING = t2.XINGMING,
                    YWYD = t2.YWYD,
                    ZJHM = t2.ZJHM,
                    ZJLX = t2.ZJLX,
                    ID = t2.ID,
                    DWMC = t1.DWMC,
                    WORK_DATE = t2.WORK_DATE,
                    DWYJCE = t2.DWYJCE,
                    GDDHHM = t2.GDDHHM,
                    GRJCBL = t2.GRJCBL
                },
                (t1, t2) => t1.DWZH == dwzh && t1.CITY_CENTNO == city_cent && ywStatusList.Contains(t1.STATUS) && t1.KHTYPE == KhtypeConst.按月汇缴,
                whereIf: whereIf,
                OrderBys: new List<OrderByClause> { new OrderByClause { Order = OrderSequence.Asc, Sort = "ID" } });
            page.total = totalCnt;
            page.list = pageList;
            return page;
        }


        /// <summary>
        /// 根据证件号码，开户类型获取个人开户数据
        /// </summary>
        /// <param name="zjhm"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        public List<v_Busi_PersonInfo> GetCreatedMonthGrhk(string zjhm,string city_cent)
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
                    ZJLX = t2.ZJLX,
                },
                (t1, t2) => t2.ZJHM == zjhm && t1.CITY_CENTNO == city_cent && t1.KHTYPE == KhtypeConst.按月汇缴);
            return model;
        }


        /// <summary>
        /// 根据grkh_item 表数据id获取数据
        /// </summary>
        /// <param name="ywlsh"></param>
        /// <param name="city_cent"></param>
        /// <param name="id">grkh_item 表数据id</param>
        /// <returns></returns>
        public v_Busi_Grkh PersonKHMonthModel(string ywlsh ,int id,string city_cent)
        {
            return GetPersonKHMonthVModel((t1, t2) => t2.ID == id && t2.YWLSH == ywlsh && t1.CITY_CENTNO == city_cent);
        }


        /// <summary>
        /// 修改按月汇缴个人开户明细信息
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="city_cent"></param>
        /// <param name="optName"></param>
        /// <returns></returns>
        public (int code,string msg) UpdatePersonKHMonthModel(P_In_PersonInfo pmodel,string city_cent,string optName)
        {
            if (string.IsNullOrEmpty(pmodel.ZJHM))
            {
                return (ApiResultCodeConst.ERROR, $"证件号码不能为空");
            }

            if (string.IsNullOrEmpty(pmodel.XINGMING))
            {
                return (ApiResultCodeConst.ERROR, $"姓名不能为空");
            }

            var oldVModel = GetPersonKHMonthVModel((t1, t2) => t2.ID == pmodel.ID && t2.YWLSH == pmodel.YWLSH && t1.CITY_CENTNO == city_cent);
            if (oldVModel == null)
            {
                return (ApiResultCodeConst.ERROR, $"修改失败，未查询到相关保存信息");
            }

            if (!statusListCanAddUpdate.Contains(oldVModel.STATUS))//在途或办结
            {
                return (ApiResultCodeConst.ERROR, $"业务处于{oldVModel.STATUS}状态,不可操作");
            }

            if(pmodel.ZJHM != oldVModel.ZJHM)//修改了证件号码，看新的证件号码有没有开过户
            {
                var r = CustHasKH(pmodel.ZJHM, city_cent);
                if (r.isWrong)
                {
                    return (ApiResultCodeConst.ERROR, r.msg);
                }
            }
            var sugarHelper = SugarHelper.Instance();
            Action action = null;
            var grkhOld = sugarHelper.First<D_GRKH>(x => x.YWLSH == pmodel.YWLSH && x.CITY_CENTNO == city_cent && statusListCanAddUpdate.Contains(x.STATUS));
            var grkh_itemOld = sugarHelper.First<D_GRKH_ITEM>(x => x.YWLSH == pmodel.YWLSH && x.ID == pmodel.ID);

            //更新主业务表
            grkhOld.STATUS = OptStatusConst.新建;
            grkhOld.CREATE_MAN = optName;
            grkhOld.CREATE_TIME = DateTime.Now;
            action += () => sugarHelper.Update(grkhOld);

            //更新明细表
            grkh_itemOld.ZJHM = pmodel.ZJHM;
            grkh_itemOld.XINGMING = pmodel.XINGMING;
            grkh_itemOld.YWYD = pmodel.YWYD;
            grkh_itemOld.CSNY = pmodel.CSNY;
            grkh_itemOld.GRCKZHHM = pmodel.GRCKZHHM;
            grkh_itemOld.GRCKZHKHMC = pmodel.GRCKZHKHMC;
            grkh_itemOld.GRCKZHKHYHDM = pmodel.GRCKZHKHYHDM;
            grkh_itemOld.GRJCJS = pmodel.GRJCJS;
            grkh_itemOld.GRYJCE = pmodel.GRYJCE;
            grkh_itemOld.QJRQ = pmodel.QJRQ;
            grkh_itemOld.SJHM = pmodel.SJHM;
            grkh_itemOld.XINGBIE = pmodel.XINGBIE;
            grkh_itemOld.ZJLX = pmodel.ZJLX;
            grkh_itemOld.WORK_DATE = pmodel.WORK_DATE;
            grkh_itemOld.GRJCBL = pmodel.GRJCBL;
            grkh_itemOld.DWYJCE = pmodel.DWYJCE;
            grkh_itemOld.GDDHHM = pmodel.GDDHHM;
            action += () => sugarHelper.Update(grkh_itemOld);

            //流程明细
            action += () => _flowProcService.AddFlowProc(grkhOld.YWLSH, grkhOld.ID, grkhOld.DWZH, nameof(GjjOptType.个人开户), optName, OptStatusConst.修改, sugarHelper: sugarHelper,memo:$"修改用户({pmodel.ZJHM})录入信息");

            sugarHelper.InvokeTransactionScope(action);

            return (ApiResultCodeConst.SUCCESS, "操作成功");
        }

        /// <summary>
        /// 删除业务明细表数据
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="city_cent"></param>
        /// <param name="optMan"></param>
        /// <returns></returns>
        public (int code, string msg) RemovePersonKHMonthModel(P_PersonInfo_Delete pmodel,string city_cent,string optMan)
        {
            var sugarHelper = SugarHelper.Instance();
            Action action = null;
            var grkhOld = sugarHelper.First<D_GRKH>(x => x.YWLSH == pmodel.ywlsh && x.CITY_CENTNO == city_cent);
            if (grkhOld == null)
            {
                return (ApiResultCodeConst.ERROR, "未查询到相关业务信息，无法删除");
            }

            if(grkhOld.STATUS == OptStatusConst.已归档)
            {
                return (ApiResultCodeConst.ERROR, "该笔业务已办结，无法删除");
            }

            if (statusListProcess.Contains(grkhOld.STATUS))
            {
                return (ApiResultCodeConst.ERROR, "该笔业务在途，无法删除");
            }

            //明细表如果没有数据了，主表数据业务删除
            var gr_itemOther = sugarHelper.IsExist<D_GRKH_ITEM>(x => x.ID != pmodel.id && x.YWLSH == pmodel.ywlsh);
            var gr_itemOld = sugarHelper.First<D_GRKH_ITEM>(x => x.YWLSH == pmodel.ywlsh && x.ID == pmodel.id);
            if(gr_itemOld == null)
            {
                return (ApiResultCodeConst.ERROR, "未查询到相关业务明细信息，无法删除");
            }
            action += () => sugarHelper.Delete(gr_itemOld);//删除明细

            if (!gr_itemOther)//删除当前数据后没有明细数据了
            {
                action += () => sugarHelper.Delete(grkhOld);//删除主表数据
                //写入业务流程数据
                action += () => _flowProcService.AddFlowProc(grkhOld.YWLSH, grkhOld.ID, grkhOld.DWZH, nameof(GjjOptType.个人开户), optMan, OptStatusConst.删除, sugarHelper: sugarHelper);
            }

            sugarHelper.InvokeTransactionScope(action);

            return (ApiResultCodeConst.SUCCESS, "操作成功");
        }

        /// <summary>
        /// 提交个人按月开户业务
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="city_cent"></param>
        /// <param name="optMan"></param>
        /// <returns></returns>
        public (int code, string msg) SubmitPersonInfoMonthCreated(P_PersonInfo_Submit pmodel, string optMan, string city_cent)
        {
            if (pmodel == null || string.IsNullOrEmpty(pmodel.ywlsh))
            {
                return (ApiResultCodeConst.ERROR, "请先录入相关业务再提交");
            }

            //先查要提交的个人开户主表数据状态
            var sugarHelper = SugarHelper.Instance();
            var busiModel = sugarHelper.First<D_GRKH>(x => x.CITY_CENTNO == city_cent && x.YWLSH == pmodel.ywlsh);
            if (busiModel == null)
            {
                return (ApiResultCodeConst.ERROR, "未查询到相关业务");
            }

            if (busiModel.STATUS == OptStatusConst.已归档)
            {
                return (ApiResultCodeConst.ERROR, "当前业务已提交成功，请勿重复提交");
            }

            var busiItemList = sugarHelper.QueryWhereList<D_GRKH_ITEM>(x => x.YWLSH == pmodel.ywlsh);
            if (busiItemList.Count < 1)
            {
                return (ApiResultCodeConst.ERROR, "未查询到相关明细业务");
            }

            //获取单位账户信息
            var dwAcctInfo = sugarHelper.First<D_CORPORATION_ACCTINFO>(x => x.CITY_CENTNO == city_cent && x.DWZH == busiModel.DWZH);


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
            //var msgErr = string.Empty;
            //var msgRst = true;
            foreach (var item in busiItemList)
            {
                var custid = string.Empty;
                var grzh = Common.PersonGrzhGenerate(item.DWZH, NextGrzhInt(item.DWZH, true));
                var custKhInfo = CustHasKH(item.ZJHM, city_cent);
                if (custKhInfo.isWrong)
                {
                    return (ApiResultCodeConst.ERROR, custKhInfo.msg);
                }

                if (custKhInfo.hasBasicInfo && !string.IsNullOrEmpty(custKhInfo.custid))//有了基本信息,新增个人账户信息
                {
                    custid = custKhInfo.custid;
                    //修改个人基本信息，
                    var custInfoOld = sugarHelper.First<D_CUSTOMER_BASICINFO>(x => x.CUSTID == custid && x.CITY_CENTNO == city_cent);
                    if (custInfoOld == null)
                    {
                        return (ApiResultCodeConst.ERROR, "用户基本信息查询异常");
                    }
                    custInfoOld.CSNY = item.CSNY;
                    custInfoOld.CUSTID = custid;
                    custInfoOld.ZJHM = item.ZJHM;
                    custInfoOld.HYZK = null;
                    custInfoOld.LASTDEAL_DATE = DateTime.Now;
                    custInfoOld.MATE_CUSTID = null;
                    custInfoOld.SJHM = item.SJHM;
                    custInfoOld.WORK_DATE = item.WORK_DATE;
                    custInfoOld.UPDATE_MAN = optMan;
                    custInfoOld.UPDATE_TIME = DateTime.Now;
                    action += () => sugarHelper.Update(custInfoOld);
                }
                else
                {
                    custid = Common.PersonCustIDGenerate(item.ZJHM.ToUpper());
                    //新增基本信息
                    var basicModel = new D_CUSTOMER_BASICINFO
                    {
                        CITY_CENTNO = city_cent,
                        OPER_ID = optMan,
                        CREATE_MAN = optMan,
                        CREATE_TIME = DateTime.Now,
                        CSNY = item.CSNY,
                        CUSTID = custid,
                        ZJHM = item.ZJHM,
                        HYZK = null,
                        GDDHHM = null,
                        LASTDEAL_DATE = DateTime.Now,
                        MATE_CUSTID = null,
                        SJHM = item.SJHM,
                        WORK_DATE = item.WORK_DATE,
                        XINGBIE = item.XINGBIE,
                        XINGMING = item.XINGMING,
                        ZJLX = item.ZJLX
                    };
                    action += () => sugarHelper.Add(basicModel);
                }

                //新增个人账户信息
                var acctModel = new D_CUSTOMER_ACCTINFO
                {
                    ACCT_TYPE = KhtypeConst.按月汇缴.ToString(),
                    CHECK_FLAG = AyhjCheckFlagConst.未核定,
                    CITY_CENTNO = city_cent,
                    CUSTID = custid,
                    DWJCBL = item.DWJCBL,
                    DWYJCE = item.DWYJCE,
                    DWZH = item.DWZH,
                    GRCKZHHM = item.GRCKZHHM,
                    GRCKZHKHMC = item.GRCKZHKHMC,
                    GRCKZHKHYHDM = item.GRCKZHKHYHDM,
                    GRJCBL = item.GRJCBL,
                    GRJCJS = item.GRJCJS,
                    GRYJCE = item.GRYJCE,
                    GRZH = grzh,
                    GRZHDNGJYE = 0,
                    GRZHSNJZYE = 0,
                    GRZHYE = 0,
                    KHRQ = sys_time,
                    GRZHSNJZRQ = null,
                    GRZHZT = GrzhztConst.正常,
                    LASTDEALDATE = DateTime.Now,
                    LOCK_REASON = null,
                    LOCK_DATE = null,
                    MONTHPAYAMT = item.DWYJCE + item.GRYJCE,
                    QJRQ = item.QJRQ,
                    UNLOCK_DATE = null,
                    XHRQ = null,
                    XHYY = null,
                    YCXBTJE = 0,
                    YCX_CHECK_FLAG = null,
                    LASTPAYMONTH = null,
                };
                action += () => sugarHelper.Add(acctModel);
            }
            //更新单位账户信息
            dwAcctInfo.DWZGRS = dwAcctInfo.DWZGRS + busiItemList.Count();
            dwAcctInfo.DWJCRS = dwAcctInfo.DWJCRS+ busiItemList.Count();
            dwAcctInfo.FACTINCOME = dwAcctInfo.FACTINCOME + busiItemList.Sum(x => x.GRJCJS);
            dwAcctInfo.OPENPERSALREADY = OpenPerSalReadyConst.已开户;
            dwAcctInfo.MONTHPAYTOTALAMT = dwAcctInfo.MONTHPAYTOTALAMT + busiItemList.Sum(x => x.DWYJCE + x.GRYJCE);
            action += () => sugarHelper.Update(dwAcctInfo);

            //写入业务流程数据
            action += () => _flowProcService.AddFlowProc(pmodel.ywlsh, busiModel.ID, busiModel.DWZH, nameof(GjjOptType.个人开户), optMan, OptStatusConst.已归档, sugarHelper: sugarHelper);
            sugarHelper.InvokeTransactionScope(action);
            return (ApiResultCodeConst.SUCCESS, "提交成功");
        }

        /// <summary>
        /// 获取按月汇缴个人开户业务数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="whereif"></param>
        /// <returns></returns>
        internal v_Busi_Grkh GetPersonKHMonthVModel(Expression<Func<D_GRKH, D_GRKH_ITEM, bool>> whereLambda = null, List<(bool, Expression<Func<D_GRKH, D_GRKH_ITEM, bool>>)> whereif = null)
        {
            return SugarHelper.Instance().QueryMuch<D_GRKH, D_GRKH_ITEM, v_Busi_Grkh>(
                (t1, t2) => new object[] { JoinType.Left, t1.YWLSH == t2.YWLSH },
                  (t1, t2) => new v_Busi_Grkh
                  {
                      YWLSH = t1.YWLSH,
                      CSNY = t2.CSNY,
                      DWJCBL = t2.DWJCBL,
                      DWZH = t2.DWZH,
                      GRCKZHHM = t2.GRCKZHHM,
                      GRCKZHKHMC = t2.GRCKZHKHMC,
                      GRCKZHKHYHDM = t2.GRCKZHKHYHDM,
                      GRJCJS = t2.GRJCJS,
                      GRYJCE = t2.GRYJCE,
                      KHTYPE = t1.KHTYPE,
                      QJRQ = t2.QJRQ,
                      SJHM = t2.SJHM,
                      STATUS = t1.STATUS,
                      XINGBIE = t2.XINGBIE,
                      XINGMING = t2.XINGMING,
                      YWYD = t2.YWYD,
                      ZJHM = t2.ZJHM,
                      ZJLX = t2.ZJLX,
                      ID = t2.ID,
                      DWMC = t1.DWMC,
                      GRJCBL = t2.GRJCBL,
                      GDDHHM = t2.GDDHHM,
                      DWYJCE = t2.DWYJCE,
                      WORK_DATE = t2.WORK_DATE
                  },
                    whereLambda: whereLambda,
                    whereif: whereif
                ).FirstOrDefault();
        }

        /// <summary>
        /// 判断业务中是否开户
        /// </summary>
        /// <param name="zjhm"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        internal (bool isWrong,string msg) BusiHasKH(string zjhm, string city_cent)
        {
            //查按月开户业务表
            var modelListMonth = GetCreatedMonthGrhk(zjhm, city_cent);
            foreach (var item in modelListMonth)
            {
                if (statusListCanAddUpdate.Contains(item.STATUS))//未提交状态
                {
                    return (true, $"用户（{zjhm}）在单位（{item.DWZH}）已有保存状态的数据，操作失败 ");
                }
                if (statusListProcess.Contains(item.STATUS))//在途的
                {
                    return (true, $"用户（{zjhm}）在单位（{item.DWZH}）已有在途状态的数据，操作失败 ");
                }
            }

            //查一次性开户业务表
            return (false, string.Empty);
        }

        /// <summary>
        /// 根据证件号码判断该用户基本信息账户信息是否开过户
        /// </summary>
        /// <param name="zjhm"></param>
        /// <param name="city_cent"></param>
        /// <returns></returns>
        internal (bool isWrong,bool hasAcctInfo, bool hasBasicInfo,string custid, string msg) CustHasKH(string zjhm, string city_cent)
        {
            var isWrong = false;//是否有问题
            var hasBasicInfo = false;//是否有账户信息
            var hasAcctInfo = false;//是否有基本信息
            var custid = string.Empty;//custid

            //看是否有账户信息
            var acctList = GetV_CustomerInfos((t1, t2) => !string.IsNullOrEmpty(t1.ZJHM) && t1.ZJHM.ToLower() == zjhm.ToLower() && t1.CITY_CENTNO == city_cent && t2.CITY_CENTNO == city_cent);
            var acctInfoZC = acctList.FirstOrDefault(x => GrzhztNotAddGrzhztNotAdd.Contains(x.GRZHZT));
            if (acctInfoZC != null)
            {
                custid = acctInfoZC.CUSTID;
                hasBasicInfo = true;
                hasAcctInfo = true;
                isWrong = true;
                return (isWrong, hasAcctInfo, hasBasicInfo, custid, $"用户（{zjhm}）在单位（{acctInfoZC.DWZH}）已有{acctInfoZC.GRZHZT}状态的账户信息，操作失败 ");
            }

            if (acctList.Count > 0)
            {
                var t = acctList.OrderBy(x => x.GRZHZT).ThenByDescending(x => x.KHRQ).FirstOrDefault();
                custid = t.CUSTID;
                hasBasicInfo = true;
                hasAcctInfo = false;
            }
            else
            {
                //查是否有基本信息
                var custBasic = SugarHelper.Instance().QueryWhereList<D_CUSTOMER_BASICINFO>(x => !string.IsNullOrEmpty(x.ZJHM) && x.ZJHM.ToLower() == zjhm.ToLower()).OrderByDescending(x => x.CUSTID).FirstOrDefault();
                if (custBasic != null)//已有基本信息
                {
                    custid = custBasic.CUSTID;
                    hasBasicInfo = true;
                }
            }
            return (isWrong, hasAcctInfo, hasBasicInfo, custid, string.Empty);
        }

        /// <summary>
        /// 获取基本信息_账户信息
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="whereif"></param>
        /// <returns></returns>
        internal List<v_CustomerInfo> GetV_CustomerInfos(Expression<Func<D_CUSTOMER_BASICINFO, D_CUSTOMER_ACCTINFO, bool>> whereLambda = null, List<(bool, Expression<Func<D_CUSTOMER_BASICINFO, D_CUSTOMER_ACCTINFO, bool>>)> whereif = null)
        {
            var vModelList = SugarHelper.Instance().QueryMuch<D_CUSTOMER_BASICINFO, D_CUSTOMER_ACCTINFO, v_CustomerInfo>(
                (t1, t2) => new object[] { JoinType.Left, t1.CUSTID == t2.CUSTID },
                (t1, t2) => new v_CustomerInfo
                {
                    CUSTID = t1.CUSTID,
                    ACCT_TYPE = t2.ACCT_TYPE,
                    CHECK_FLAG = t2.CHECK_FLAG,
                    CITY_CENTNO = t2.CITY_CENTNO,
                    CSNY = t1.CSNY,
                    DWJCBL = t2.DWJCBL,
                    DWYJCE = t2.DWYJCE,
                    DWZH = t2.DWZH,
                    GDDHHM = t1.GDDHHM,
                    GRCKZHHM = t2.GRCKZHHM,
                    GRCKZHKHMC = t2.GRCKZHKHMC,
                    GRCKZHKHYHDM = t2.GRCKZHKHYHDM,
                    GRJCBL = t2.GRJCBL,
                    GRJCJS = t2.GRJCJS,
                    GRYJCE = t2.GRYJCE,
                    GRZH = t2.GRZH,
                    GRZHDNGJYE = t2.GRZHDNGJYE,
                    GRZHSNJZRQ = t2.GRZHSNJZRQ,
                    GRZHSNJZYE = t2.GRZHSNJZYE,
                    GRZHYE = t2.GRZHYE,
                    GRZHZT = t2.GRZHZT,
                    HYZK = t1.HYZK,
                    KHRQ = t2.KHRQ,
                    LASTPAYMONTH = t2.LASTPAYMONTH,
                    LOCK_DATE = t2.LOCK_DATE,
                    LOCK_REASON = t2.LOCK_REASON,
                    MATE_CUSTID = t1.MATE_CUSTID,
                    MONTHPAYAMT = t2.MONTHPAYAMT,
                    QJRQ = t2.QJRQ,
                    SJHM = t1.SJHM,
                    UNLOCK_DATE = t2.UNLOCK_DATE,
                    WORK_DATE = t1.WORK_DATE,
                    XHRQ = t2.XHRQ,
                    XHYY = t2.XHYY,
                    XINGBIE = t1.XINGBIE,
                    XINGMING = t1.XINGMING,
                    YCXBTJE = t2.YCXBTJE,
                    YCX_CHECK_FLAG = t2.YCX_CHECK_FLAG,
                    ZJHM = t1.ZJHM,
                    ZJLX = t1.ZJLX
                },
                whereLambda,
                whereif
                );
            return vModelList;
        }

        /// <summary>
        /// 获取下一个个人账号数字
        /// </summary>
        /// <returns></returns>
        internal int NextGrzhInt(string dwzh,bool isAyjc)
        {
            var sql = string.Empty;

            if (isAyjc)
            {
                sql = " select max(SUBSTR(GRZH, 5, 5)) from CUSTOMER_ACCTINFO where dwzh = :dwzh and SUBSTR(GRZH, 5, 5) not like '9%' ";
            }
            else
            {
                sql = " select max(SUBSTR(GRZH, 5, 5)) from CUSTOMER_ACCTINFO where dwzh = :dwzh  and SUBSTR(GRZH, 5, 5) like '9%' ";
            }

            var maxDwzh = Convert.ToInt32(SugarHelper.Instance().QuerySqlScalar(sql, new SugarParameter[] { new SugarParameter("dwzh", dwzh) }));
            return maxDwzh + 1;
        }
    }
}
