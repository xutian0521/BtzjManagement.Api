using BtzjManagement.Api.Model.QueryModel;
using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Services
{
    public class SysEnumService
    {
        /// <summary>
        /// 根据枚举类型获取枚举列表-主要用于前端下拉列表渲染
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<v_SYS_ENUM> GetListByType(string type)
        {
            return SugarHelper.Instance().QueryWhereList<D_SYS_ENUM>(x => x.TYPEKEY == type.Trim()).Select(x => new v_SYS_ENUM
            {
                LABEL = x.LABEL,
                TYPEKEY = x.TYPEKEY,
                VAL = x.VAL
            }).ToList();
        }
    }
}
