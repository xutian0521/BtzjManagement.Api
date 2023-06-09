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
        public int i_grade { get; set; }
        /// <summary>
        /// 科目编号1
        /// </summary>
        public string s_bm1 { get; set; }
        /// <summary>
        /// 科目编号2
        /// </summary>
        public string s_bm2 { get; set; }
        /// <summary>
        /// 科目编号3
        /// </summary>
        public string s_bm3 { get; set; }
        /// <summary>
        /// 科目编号4
        /// </summary>
        public string s_bm4 { get; set; }
        /// <summary>
        /// 科目名称1
        /// </summary>
        public string s_mc1 { get; set; }
        /// <summary>
        /// 科目名称2
        /// </summary>
        public string s_mc2 { get; set; }
        /// <summary>
        /// 科目名称3
        /// </summary>
        public string s_mc3 { get; set; }
        /// <summary>
        /// 科目名称4
        /// </summary>
        public string s_mc4 { get; set; }
        /// <summary>
        /// 科目编码
        /// </summary>
        public string s_kmbm { get; set; }
        /// <summary>
        /// 科目类型
        /// </summary>
        public int i_kmlx { get; set; }
        /// <summary>
        /// 科目性质
        /// </summary>
        public int i_kmxz { get; set; }
        /// <summary>
        /// 记账日期
        /// </summary>
        public DateTime dt_jzrq { get; set; }
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
