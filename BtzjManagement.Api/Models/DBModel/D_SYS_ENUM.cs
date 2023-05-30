using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 数据字典表
    /// </summary>
    [SugarTable("SYS_DATA_DICTIONARY")]
    public class D_SYS_DATA_DICTIONARY
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SYS_DATA_DICTIONARY_SEQ")]
        public int ID { get; set; }
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
        /// 备注
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// 父类ID
        /// </summary>
        public int PARENT_ID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SORT_ID { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 原sybase i_flag 字段留存
        /// </summary>
        public string ORIGIN_FLAG { get; set; }
        /// <summary>
        /// 城市网点编号
        /// </summary>
        public string CITY_CENTNO { get; set; }
    }
}
