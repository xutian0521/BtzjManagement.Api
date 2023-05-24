using BtzjManagement.Api.Enum;
using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Services
{
    public class BusiCorporationService
    {
        /// <summary>
        /// 新增单位业务主表数据
        /// </summary>
        /// <param name="city_cent"></param>
        /// <param name="ywlsh"></param>
        /// <param name="uniqueKey"></param>
        /// <param name="busitype"></param>
        /// <param name="optMan"></param>
        /// <param name="status"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public bool AddBusiCorporation(string city_cent, string ywlsh, string uniqueKey, int busitype, string optMan, string status = OptStatusConst.新建, string memo = "",ISugarHelper sugarHelper=null)
        {
            D_BUSI_CORPORATION model = new D_BUSI_CORPORATION
            {
                CITY_CENTNO = city_cent,
                YWLSH = ywlsh,
                UNIQUE_KEY = uniqueKey,
                BUSITYPE = busitype,
                MEMO = memo,
                STATUS = status
            };
            if (status == OptStatusConst.新建 || status == OptStatusConst.已归档)
            {
                model.CREATE_MAN = optMan;
                model.CREATE_TIME = DateTime.Now;
            }
            if(sugarHelper == null)
            {
                return SugarHelper.Instance().AddReturnBool(model);
            }
            return sugarHelper.AddReturnBool(model);
        }
    }
}
