using BtzjManagement.Api.Model.QueryModel;
using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
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
    public class SysEnumService
    {
        /// <summary>
        /// 根据枚举类型获取枚举列表-主要用于前端下拉列表渲染
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public List<v_SYS_ENUM> GetListByType(string type, string val = "")
        {
            List<(bool, Expression<Func<D_SYS_ENUM, bool>>)> whereIf = new List<(bool, Expression<Func<D_SYS_ENUM, bool>>)> { };
            whereIf.Add(new(!string.IsNullOrEmpty(val), x => x.VAL == val));

            return SugarHelper.Instance().QueryWhereList<D_SYS_ENUM>(x => x.TYPEKEY == type.Trim(),whereIf).Select(x => new v_SYS_ENUM
            {
                LABEL = x.LABEL,
                TYPEKEY = x.TYPEKEY,
                VAL = x.VAL
            }).ToList();
        }
    }
}
