using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.ViewModel
{
    /// <summary>
    /// 表字段对象
    /// </summary>
    public class v_TableInit
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string columnName { get; set; }
        /// <summary>
        /// 列类型及相关约束
        /// </summary>
        public string columnTypeAndLimit { get; set; }
        /// <summary>
        /// 列描述
        /// </summary>
        public string columnDesc{ get; set; }
    }
}
