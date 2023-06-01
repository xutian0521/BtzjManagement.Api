namespace BtzjManagement.Api.Models.QueryModel
{
    /// <summary>
    /// 
    /// </summary>
    public class P_AccountLogin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 验证码key
        /// </summary>
        public string codeKey { get; set; }
    }
}
