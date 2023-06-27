using SqlSugar;

namespace BtzjManagement.Api.Models.DBModel
{
    [SugarTable("OPERATION_GJJ_ENTER_FORGR", "单位汇缴个人子表主表")]
    public class D_OPERATION_GJJ_ENTER_FORGR
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "OPERATION_GJJ_ENTER_FORGR_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 记账凭证号
        /// </summary>
        public string JZPZH { get; set; }

        /// <summary>
        /// 个人账号
        /// </summary>
        public string GRZH { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal JE { get; set; }

        /// <summary>
        /// 单位公积金
        /// </summary>
        public decimal DC_DWGJJ { get; set; }
    }

}
