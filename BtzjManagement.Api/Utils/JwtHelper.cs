using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using System.Collections.Generic;

namespace BtzjManagement.Api.Utils
{
    /// <summary>
    /// jwt帮助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// jwt加密
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userName">用户名</param>
        /// <param name="roleId">角色id</param>
        /// <param name="exp">过期时间</param>
        /// <returns></returns>
        public static string Encrypt(string userId, string userName, string roleId, double exp)
        {
            #region 1) 加密
            var payload = new Dictionary<string, object>
            {
                { "userId", userId },
                { "userName", userName },
                { "roleId", roleId },
                { "exp", exp }
            };
            var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";//不要泄露
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            return token;
            #endregion

        }
        /// <summary>
        /// jwt加密
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userName">用户名</param>
        /// <param name="roleId">角色id</param>
        /// <param name="exp">过期时间</param>
        /// <returns></returns>
        public static string SecondaryAuthEncrypt(string userId, string userName, int roleId, double exp)
        {
            #region 1) 加密
            var payload = new Dictionary<string, object>
            {
                { "userId", userId },
                { "userName", userName },
                { "roleId", roleId },
                { "exp", exp }
            };
            var secret = "MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBALow9";//不要泄露
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            return token;
            #endregion

        }

    }
}
