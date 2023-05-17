
using BtzjManagement.Api.Enum;

namespace BtzjManagement.Api.Models.QueryModel
{
    /// <summary>
    /// 查询
    /// </summary>
    public class QueryCondition
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 查询操作
        /// </summary>
        public QueryOperator Operator { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 运算符
        /// </summary>
        public QueryCharacter Character { get; set; }

    }
}
