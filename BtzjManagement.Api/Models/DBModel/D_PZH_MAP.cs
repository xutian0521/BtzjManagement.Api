using SqlSugar;

namespace BtzjManagement.Api.Models.DBModel
{
    [SugarTable("PZH_MAP", "单位汇缴_凭证对应表")]
    public class D_PZH_MAP
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "PZH_MAP_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 记账凭证号
        /// </summary>
        public string JZPZH { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int I_YEAR { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int I_MONTH { get; set; }

        /// <summary>
        /// 凭证号
        /// </summary>
        public int I_MONTH_PZH { get; set; }

        /// <summary>
        /// 凭证类型
        /// </summary>
        public int I_PZ_TYPE { get; set; }
    }

}
