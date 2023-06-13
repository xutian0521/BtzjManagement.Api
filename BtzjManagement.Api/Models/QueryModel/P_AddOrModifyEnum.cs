using Microsoft.AspNetCore.Mvc;

namespace BtzjManagement.Api.Models.QueryModel
{
    /// <summary>
    /// 新增或修改枚举字典P
    /// </summary>
    public class P_AddOrModifyEnum
    {
        /// <summary>
        /// 字典id
        /// </summary>
        public int id{get;set;}
        /// <summary>
        /// 字典key
        /// </summary>
        public string dataKey{get;set;}
        /// <summary>
        /// 字典别名
        /// </summary>
        public string dataKeyAlias{get;set;}
        /// <summary>
        /// 父级id
        /// </summary>
        public int? pId{get;set;}
        /// <summary>
        /// 字典值
        /// </summary>
        public string dataValue{get;set;}
        /// <summary>
        /// 描述
        /// </summary>
        public string dataDescription{get;set;}
        /// <summary>
        /// 排序
        /// </summary>
        public int? sortId{get;set;}
    }
}
