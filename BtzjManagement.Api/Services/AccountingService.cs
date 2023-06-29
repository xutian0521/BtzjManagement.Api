using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Npgsql.Replication.PgOutput.Messages;
using BtzjManagement.Api.Models.ViewModel;
using System.Text.Json;

namespace BtzjManagement.Api.Services
{
    /// <summary>
    /// 核算业务层
    /// </summary>
    public class AccountingService
    {
        /// <summary>
        /// 生成记账凭证凭证
        /// </summary>
        /// <param name="p"></param>
        public async void Createvoucher(P_Remittance p)
        {
            /*
             * 1. 生成记账凭证表记录 AC_VOUCHERATTACHMENT
             * 2. 
             */
            var _JZPZH = this.GeneratePZH();
            var voucher = new D_AC_VOUCHERATTACHMENT();
            voucher.JZPZH = _JZPZH;

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


        public string GeneratePZH()
        {
            return DateTime.Now.Ticks.ToString();
        }
        public async Task<int> GetI_MONTH_PZH(int i_year, int i_month)
        {
            var _I_MONTH = await SugarSimple.Instance().
                Queryable<D_PZH_MAP>().Where(x => x.I_YEAR == i_year && x.I_MONTH == i_month)
                .MaxAsync(x => x.I_MONTH);
            return _I_MONTH + 1;

        }
        public void GetVoucherTemplate()
        {
            v_VoucherTemplate tableData = new v_VoucherTemplate
            {
                Headers = new List<string> { "分录", "摘要", "科目编码", "科目名称", "借方金额", "贷方金额" },
                Rows = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            { "分录", 1 },
            { "摘要", "汇缴1个月" },
            { "科目编码", "2011131" },
            { "科目名称", "住房补贴" },
            { "借方金额", 0.00 },
            { "贷方金额", 1690.00 }
        },
        //...更多行
    }
            };

            string json = JsonSerializer.Serialize(tableData);

        }
    }
}

