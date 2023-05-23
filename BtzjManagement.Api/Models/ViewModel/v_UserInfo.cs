namespace BtzjManagement.Api.Models.ViewModel
{
    public class v_UserInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string REALNAME { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PASSWORD { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public string RULEID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }
    }
}
