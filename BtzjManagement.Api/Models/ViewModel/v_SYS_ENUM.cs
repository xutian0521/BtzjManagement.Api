using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.ViewModel
{
    /// <summary>
    /// 数据字典枚举vm
    /// </summary>
    public class v_SYS_DATA_DICTIONARY
    {
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public int PARENT_ID { get; set; }
        
        /// <summary>
        /// 父级枚举类型
        /// </summary>
        public string PARENT_TYPE_KEY { get; set; }
        /// <summary>
        /// 枚举类型
        /// </summary>
        public string TYPE_KEY { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string VAL { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string LABEL { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SORT_ID { get; set; }
        /// <summary>
        /// <summary>
        /// 子集合
        /// </summary>
        public List<v_SYS_DATA_DICTIONARY> Subs { get; set; }
    }
}
