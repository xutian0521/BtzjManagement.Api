using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
