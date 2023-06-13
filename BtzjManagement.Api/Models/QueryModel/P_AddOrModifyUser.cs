using Microsoft.AspNetCore.Mvc;

namespace BtzjManagement.Api.Models.QueryModel
{
    /// <summary>
    /// 添加或修改用户 P
    /// </summary>
    public class P_AddOrModifyUser
    {
        /// <summary>
        /// id
        /// </summary>
        public int id{ get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName{ get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password{ get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public string roleId{ get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realName{ get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
