using SqlSugar;

namespace BtzjManagement.Api.Models.DBModel
{
    [SugarTable("PZ_FUJI", "单位汇缴_凭证附件表")]
    public class D_PZ_FUJI
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "PZ_FUJI_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 记账凭证号
        /// </summary>
        public string JZPZH { get; set; }

        /// <summary>
        /// 附件数量
        /// </summary>
        public int I_FJ_COUNT { get; set; }
    }

}
