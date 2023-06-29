using SqlSugar;

namespace BtzjManagement.Api.Models.DBModel
{
    /// <summary>
    /// 账目凭证附件表
    /// </summary>
    [SugarTable("AC_VOUCHERATTACHMENT")]
    public class D_AC_VOUCHERATTACHMENT
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "AC_VOUCHERATTACHMENT_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 记账凭证号
        /// </summary>
        public string JZPZH { get; set; }

        /// <summary>
        /// 附件数量
        /// </summary>
        public int ATTACHMENTCOUNT { get; set; }
    }

}
