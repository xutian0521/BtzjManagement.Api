using Microsoft.AspNetCore.Mvc;

namespace BtzjManagement.Api.Models.QueryModel
{
    /// <summary>
    /// 新增或修改枚举字典P
    /// </summary>
    public class P_AddOrModifyDictionary
    {
        /// <summary>
        /// 字典id
        /// </summary>
        public int id{get;set;}
        /// <summary>
        /// 字典key
        /// </summary>
        public string LABEL { get;set;}
        /// <summary>
        /// 字典别名
        /// </summary>
        public string TYPEKEY { get;set;}
        /// <summary>
        /// 父级id
        /// </summary>
        public int PARENTID { get;set;}
        /// <summary>
        /// 字典值
        /// </summary>
        public string VAL { get;set;}
        /// <summary>
        /// 描述
        /// </summary>
        public string DESCRIPTION { get;set;}
        /// <summary>
        /// 排序
        /// </summary>
        public int SORTID { get;set;}
    }
}
