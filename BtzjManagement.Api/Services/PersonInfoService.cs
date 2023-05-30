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
        /// <summary>
        /// 能录入和修改的状态
        /// </summary>
        List<string> statusListCanAddUpdate = new List<string> { OptStatusConst.新建, OptStatusConst.终审退回,OptStatusConst.初审退回 };
        /// <summary>
        /// 业务在途的状态-此时不能修改
        /// </summary>
        List<string> statusListProcess = new List<string> { OptStatusConst.初审出错, OptStatusConst.等待初审, OptStatusConst.终审出错, OptStatusConst.等待终审 };

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="flowProcService"></param>
        /// <param name="corporationService"></param>
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

            //查用户是否已有账户
            var hasKh = HasKH(pmodel.ZJHM, city_cent);

            if (hasKh.hasKh)//有账户
            {
                return (ApiResultCodeConst.ERROR, hasKh.msg, string.Empty);
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
        /// <param name="KHTYPE"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Pager<v_Busi_Grkh> PersonKhMonthList(string city_cent, string dwzh, int pageIndex, int pageSize, int KHTYPE = 1, string status = OptStatusConst.新建)
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
                (t1, t2) => new v_Busi_Grkh { YWLSH = t1.YWLSH, CSNY = t2.CSNY, DWJCBL = t2.DWJCBL, DWZH = t2.DWZH, GRCKZHHM = t2.GRCKZHHM, GRCKZHKHMC = t2.GRCKZHKHMC, GRCKZHKHYHDM = t2.GRCKZHKHYHDM, GRJCJS = t2.GRJCJS, GRYJCE = t2.GRYJCE, KHTYPE = t1.KHTYPE, QJRQ = t2.QJRQ, SJHM = t2.SJHM, STATUS = t1.STATUS, XINGBIE = t2.XINGBIE, XINGMING = t2.XINGMING, YWYD = t2.YWYD, ZJHM = t2.ZJHM, ZJLX = t2.ZJLX, ID=t2.ID },
                (t1, t2) => t1.DWZH == dwzh && t1.CITY_CENTNO == city_cent && ywStatusList.Contains(t1.STATUS) && t1.KHTYPE == KHTYPE,
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
        public v_Busi_PersonInfo GetCreatedMonthGrhk(string zjhm,string city_cent)
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
                (t1, t2) => t2.ZJHM == zjhm && t1.CITY_CENTNO == city_cent && t1.KHTYPE == KhtypeConst.按月汇缴).FirstOrDefault();
            return model;
        }


        /// <summary>
        /// 根据grkh_item 表数据id获取数据
        /// </summary>
        /// <param name="ywlsh"></param>
        /// <param name="id">grkh_item 表数据id</param>
        /// <returns></returns>
        public v_Busi_Grkh PersonKHMonthModel(string ywlsh ,int id)
        {
            return GetPersonKHMonthVModel((t1, t2) => t2.ID == id && t2.YWLSH == ywlsh);
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

            var oldVModel = GetPersonKHMonthVModel((t1, t2) => t2.ID == pmodel.ID && t2.YWLSH == pmodel.YWLSH);
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
                var r = HasKH(pmodel.ZJHM, city_cent);
                if (r.hasKh)
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
            action += () => sugarHelper.Update(grkh_itemOld);

            //流程明细
            action += () => _flowProcService.AddFlowProc(grkhOld.YWLSH, grkhOld.ID, grkhOld.DWZH, nameof(GjjOptType.个人开户), optName, OptStatusConst.修改, sugarHelper: sugarHelper,memo:$"修改用户({pmodel.ZJHM})录入信息");

            sugarHelper.InvokeTransactionScope(action);

            return (ApiResultCodeConst.SUCCESS, "操作成功");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ywlsh"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public (int code, string msg) RemovePersonKHMonthModel(string ywlsh, int id)
        {
            var grkhOld = SugarHelper.Instance().First<D_GRKH>(x => x.YWLSH == ywlsh);
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
            var gr_itemCnt = SugarHelper.Instance();
            var gr_itemOld = SugarHelper.Instance().First<D_GRKH_ITEM>(x => x.YWLSH == ywlsh && x.ID == id);
            if(gr_itemOld == null)
            {
                return (ApiResultCodeConst.ERROR, "未查询到相关业务明细信息，无法删除");
            }

            SugarHelper.Instance().Delete(gr_itemOld);
            return (ApiResultCodeConst.SUCCESS, "操作成功");
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
                  (t1, t2) => new v_Busi_Grkh { YWLSH = t1.YWLSH, CSNY = t2.CSNY, DWJCBL = t2.DWJCBL, DWZH = t2.DWZH, GRCKZHHM = t2.GRCKZHHM, GRCKZHKHMC = t2.GRCKZHKHMC, GRCKZHKHYHDM = t2.GRCKZHKHYHDM, GRJCJS = t2.GRJCJS, GRYJCE = t2.GRYJCE, KHTYPE = t1.KHTYPE, QJRQ = t2.QJRQ, SJHM = t2.SJHM, STATUS = t1.STATUS, XINGBIE = t2.XINGBIE, XINGMING = t2.XINGMING, YWYD = t2.YWYD, ZJHM = t2.ZJHM, ZJLX = t2.ZJLX, ID = t2.ID },
                    whereLambda: whereLambda,
                    whereif: whereif
                ).FirstOrDefault();
        }

        internal (bool hasKh, string msg) HasKH(string zjhm, string city_cent)
        {
            //先查账户基本信息表

            //查按月开户业务表
            var modelMonth = GetCreatedMonthGrhk(zjhm, city_cent);
            if (modelMonth != null)
            {
                if (statusListCanAddUpdate.Contains(modelMonth.STATUS))//未提交状态
                {
                    return (true, $"用户（{zjhm}）在单位（{modelMonth.DWZH}）已有保存状态的数据，操作失败");
                }
                if (statusListProcess.Contains(modelMonth.STATUS))//在途的
                {
                    return (true, $"用户（{zjhm}）在单位（{modelMonth.DWZH}）已有在途状态的数据，操作失败");
                }
            }

            //查一次性开户业务表

            return (false, string.Empty);
        }
    }
}
