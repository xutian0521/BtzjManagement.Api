using SqlSugar;
using System;
using System.Collections.Generic;

namespace BtzjManagement.Api.Models.ViewModel
{
    /// <summary>
    /// 科目vm
    /// </summary>
    public class v_KM
    {
        /// <summary>
        /// 科目编码
        /// </summary>
        public string S_KMBM { get; set; }

        /// <summary>
        /// 科目级别
        /// </summary>
        public int I_GRADE { get; set; }

        /// <summary>
        /// 一级科目编码
        /// </summary>
        public string S_BM1 { get; set; }

        /// <summary>
        /// 二级科目编码
        /// </summary>
        public string S_BM2 { get; set; }

        /// <summary>
        /// 三级科目编码
        /// </summary>
        public string S_BM3 { get; set; }

        /// <summary>
        /// 四级科目编码
        /// </summary>
        public string S_BM4 { get; set; }

        /// <summary>
        /// 一级科目名称
        /// </summary>
        public string S_MC1 { get; set; }

        /// <summary>
        /// 二级科目名称
        /// </summary>
        public string S_MC2 { get; set; }

        /// <summary>
        /// 三级科目名称
        /// </summary>
        public string S_MC3 { get; set; }

        /// <summary>
        /// 四级科目名称
        /// </summary>
        public string S_MC4 { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string S_KMMC { get; set; }

        /// <summary>
        /// 科目性质
        /// </summary>
        public int I_KMXZ { get; set; }

        /// <summary>
        /// 科目类型
        /// </summary>
        public int I_KMLX { get; set; }

        /// <summary>
        /// 记账日期
        /// </summary>
        public DateTime DT_JZRQ { get; set; }

        /// <summary>
        /// 科目借贷
        /// </summary>
        public int I_KMJD { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string S_MEMO { get; set; }
        /// <summary>
        /// <summary>
        /// 子集合
        /// </summary>
        public List<v_KM> Subs { get; set; } = new List<v_KM>();
        /// <summary>
        /// 科目借贷display
        /// </summary>

        public string I_KMJD_display { get; set; }
        /// <summary>
        /// 科目名称display
        /// </summary>
        public string S_KMMC_display { get; set; }
        /// <summary>
        /// 科目编号display
        /// </summary>
        public string S_KMBM_display { get; set; }
        /// <summary>
        /// 科目编号-科目名称display
        /// </summary>
        public string S_KMBM_KMMC_display { get; set; }
        /// <summary>
        /// 科目类型
        /// </summary>
        public string I_KMLX_display { get; set; }
    }
}
