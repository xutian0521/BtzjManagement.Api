namespace BtzjManagement.Api.Models.QueryModel
{
    /// <summary>
    /// 新增或修改角色 P
    /// </summary>
    public class P_AddOrModifyRole
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string roleName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
