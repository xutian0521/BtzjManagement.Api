using System;

namespace BtzjManagement.Api.Models.QueryModel
{
    /// <summary>
    /// 添加或修改科目-参数
    /// </summary>
    public class P_AddOrModifyKM
    {
        /// <summary>
        /// 科目等级
        /// </summary>
        public int KMJB { get; set; }
        /// <summary>
        /// 科目编号1
        /// </summary>
        public string BH1 { get; set; }
        /// <summary>
        /// 科目编号2
        /// </summary>
        public string BH2 { get; set; }
        /// <summary>
        /// 科目编号3
        /// </summary>
        public string BH3 { get; set; }
        /// <summary>
        /// 科目编号4
        /// </summary>
        public string BH4 { get; set; }
        /// <summary>
        /// 科目名称1
        /// </summary>
        public string MC1 { get; set; }
        /// <summary>
        /// 科目名称2
        /// </summary>
        public string MC2 { get; set; }
        /// <summary>
        /// 科目名称3
        /// </summary>
        public string MC3 { get; set; }
        /// <summary>
        /// 科目名称4
        /// </summary>
        public string MC4 { get; set; }
        /// <summary>
        /// 科目编号
        /// </summary>
        public string KMBH { get; set; }
        /// <summary>
        /// 科目类型
        /// </summary>
        public int KMLX { get; set; }
        /// <summary>
        /// 科目性质
        /// </summary>
        public int KMXZ { get; set; }
        /// <summary>
        /// 记账日期
        /// </summary>
        public DateTime JZRQ { get; set; }
        /// <summary>
        /// 科目借贷
        /// </summary>
        public int kmjd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }
        /// <summary>
        /// 修改或者新增标识
        /// </summary>
        public string add_or_modify { get; set; }

    }
}
