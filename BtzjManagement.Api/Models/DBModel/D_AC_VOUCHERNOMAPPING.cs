using SqlSugar;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 账目凭证对应表
    /// </summary>
    [SugarTable("AC_VOUCHERNOMAPPING")]
    public class D_AC_VOUCHERNOMAPPING
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "AC_VOUCHERNOMAPPING_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 记账凭证号
        /// </summary>
        public string JZPZH { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int YEAR { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int MONTH { get; set; }

        /// <summary>
        /// 月份多次
        /// </summary>
        public int MONTHREPEATEDLY { get; set; }

        /// <summary>
        /// 凭证类型
        /// </summary>
        public int VOUCHERTYPE { get; set; }
    }

}
