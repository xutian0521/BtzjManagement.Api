using SqlSugar;

namespace BtzjManagement.Api.Models.DBModel
{
    [SugarTable("SYS_ENUM")]
    public class D_SYS_ROLE
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "SYS_ROLE_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string ROLE_NAME { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public string SORT_ID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }
    }
}
