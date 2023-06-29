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
        /// 科目编号
        /// </summary>
        public string KMBH { get; set; }

        /// <summary>
        /// 科目级别
        /// </summary>
        public int KMJB { get; set; }

        /// <summary>
        /// 一级科目编码
        /// </summary>
        public string BH1 { get; set; }

        /// <summary>
        /// 二级科目编码
        /// </summary>
        public string BH2 { get; set; }

        /// <summary>
        /// 三级科目编码
        /// </summary>
        public string BH3 { get; set; }

        /// <summary>
        /// 四级科目编码
        /// </summary>
        public string BH4 { get; set; }

        /// <summary>
        /// 一级科目名称
        /// </summary>
        public string MC1 { get; set; }

        /// <summary>
        /// 二级科目名称
        /// </summary>
        public string MC2 { get; set; }

        /// <summary>
        /// 三级科目名称
        /// </summary>
        public string MC3 { get; set; }

        /// <summary>
        /// 四级科目名称
        /// </summary>
        public string MC4 { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string KMMC { get; set; }

        /// <summary>
        /// 科目性质
        /// </summary>
        public int KMXZ { get; set; }

        /// <summary>
        /// 科目类型
        /// </summary>
        public int KMLX { get; set; }

        /// <summary>
        /// 记账日期
        /// </summary>
        public DateTime JZRQ { get; set; }

        /// <summary>
        /// 科目借贷
        /// </summary>
        public int KMJD { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }
        /// <summary>
        /// <summary>
        /// 子集合
        /// </summary>
        public List<v_KM> Subs { get; set; } = new List<v_KM>();
        /// <summary>
        /// 科目借贷display
        /// </summary>

        public string KMJD_display { get; set; }
        /// <summary>
        /// 科目名称display
        /// </summary>
        public string KMMC_display { get; set; }
        /// <summary>
        /// 科目编号display
        /// </summary>
        public string KMBH_display { get; set; }
        /// <summary>
        /// 科目编号-科目名称display
        /// </summary>
        public string KMBH_KMMC_display { get; set; }
        /// <summary>
        /// 科目类型
        /// </summary>
        public string KMLX_display { get; set; }
    }
}
