using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Services
{
    public class ActionService
    {
        //汇缴
        public async void Remittance(P_Remittance p)
        {
            /*
             * 1. 生成汇缴主表记录OPERATION_GJJ_ENTER
             * 2. 
             */
            var _JZPZH = this.GeneratePZH();
            var gjj_enter = new D_OPERATION_GJJ_ENTER();
            gjj_enter.JZPZH = _JZPZH;
            gjj_enter.FSRQ = DateTime.Now;
            gjj_enter.DWZH = p.DWZH;
            gjj_enter.HJYF = p.HJYF;
            gjj_enter.I_HJ_YEAR = p.I_HJ_YEAR;
            gjj_enter.I_HJ_MONTH = p.I_HJ_MONTH;
            gjj_enter.I_HJFS = p.I_HJFS; //?
            gjj_enter.S_USERNAME = p.S_USERNAME;
            gjj_enter.DC_HJJE = p.DC_HJJE;

            var list_gjj_enter_forgr = this.ComputeHJJE(p.DWZH, p.monthCount);
            D_PZ_FUJI fj = new D_PZ_FUJI();
            fj.JZPZH = _JZPZH;
            fj.I_FJ_COUNT = p.I_FJ_COUNT;

            D_PZH_MAP map = new D_PZH_MAP();
            map.JZPZH = _JZPZH;
            map.I_YEAR = p.I_HJ_YEAR;
            map.I_MONTH = p.I_HJ_MONTH;
            map.I_MONTH_PZH = await this.GetI_MONTH_PZH(p.I_HJ_YEAR, p.I_HJ_MONTH);

            D_PZK pzk = new D_PZK();
   
            //var insertResult = await SugarSimple.Instance().Insertable(model).ExecuteCommandAsync();
            //return insertResult > 0 ? (ApiResultCodeConst.SUCCESS, "添加成功！") : (ApiResultCodeConst.ERROR, "添加失败!");

        }

        public async Task ComputeHJJE(string dwzh, int monthCount)
        {
            /*
             * 1.获取单位的缴存比例
             * 2.根据个人的工资*单位缴存比例*月份
             */
            List<D_OPERATION_GJJ_ENTER_FORGR> resultPersons = new List<D_OPERATION_GJJ_ENTER_FORGR>();
            var dwacc = await SugarSimple.Instance().Queryable<D_CORPORATION_ACCTINFO>().Where(x => x.DWZH == dwzh).FirstAsync();
            var persons = await SugarSimple.Instance().Queryable<D_CUSTOMER_ACCTINFO>().Where(x => x.DWZH == dwzh).ToListAsync();
            foreach (var person in persons)
            {
                D_OPERATION_GJJ_ENTER_FORGR rp = new D_OPERATION_GJJ_ENTER_FORGR();
                rp.JE = (person.GRJCBL * person.GRJCJS + person.DWJCBL * person.GRJCJS) * monthCount;
                rp.GRZH = person.GRZH;
                resultPersons.Add(rp);
            }
        }

        public string GeneratePZH()
        {
            return DateTime.Now.Ticks.ToString();
        }
        public async Task<int> GetI_MONTH_PZH(int i_year, int i_month)
        {
            var _I_MONTH = await SugarSimple.Instance().
                Queryable<D_PZH_MAP>().Where(x => x.I_YEAR == i_year && x.I_MONTH ==i_month)
                .MaxAsync(x => x.I_MONTH);
            return _I_MONTH + 1;

        }
    }
}

