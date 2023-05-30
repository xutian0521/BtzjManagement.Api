using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Services
{
    /// <summary>
    /// 账号相关业务类
    /// </summary>
    public class AccountService
    {
        static IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
        public async Task<(int code, string message, v_LoginResult loginResult)> Login(string userName,
                string password, string code, string codeKey, HttpContext httpContext)
        {
            string _code = _memoryCache.Get<string>("ValidateCode_" + codeKey);

            if (!_code.Equals(code))
            {
                return (ApiResultCodeConst.ERROR,"验证码不正确", null);
            }
            var user = await this.GetUserByNameAsync(0, userName);

            if (user == null)
            {
                return (ApiResultCodeConst.ERROR, "用户名不存在", null);
            }

            string _pwd = Common.MD5Encoding(password, user.SALT);
            if (!user.PASSWORD.Equals(_pwd))
            {
                return (ApiResultCodeConst.ERROR, "密码不正确", null);
            }
            user.LAST_LOGIN_TIME = DateTime.Now;
            user.LAST_LOGIN_IP = Common.GetIP(httpContext);
            int updateRow = await SugarSimple.Instance().Updateable(user).ExecuteCommandAsync();
            string userJson = System.Text.Json.JsonSerializer.Serialize(user);

            const int expMin = 60 * 24 * 30;
            //const int expMin = 1;
            var exp = (DateTime.UtcNow.AddMinutes(expMin) - new DateTime(1970, 1, 1)).TotalSeconds;
            var srtJson = JwtHelper.Encrypt(user.ID.ToString(), user.NAME, user.ROLE_ID, exp);

            return (ApiResultCodeConst.SUCCESS, "登录成功", 
                new v_LoginResult()
            {
                ACCESS_TOKEN = srtJson,
                EXPIRES_IN = expMin,
                USER_ID = user.ID.ToString(),
                USER_NAME = user.NAME,
                ROLE_ID = user.ROLE_ID,
                REAL_NAME = user.REAL_NAME,
            });


        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<D_USER_INFO> GetUserByNameAsync(int id, string userName)
        {
            var query =  SugarSimple.Instance().Queryable<D_USER_INFO>().Where(x => x.NAME == userName);
            query = query.WhereIF(!string.IsNullOrEmpty(userName), u => u.NAME == userName);
            query = query.WhereIF(id > 0, u => u.ID == id);

            var user = await query.FirstAsync();

            return user;

        }

        /// <summary>
        /// 获取图像验证码
        /// </summary>
        /// <returns></returns>
        public dynamic GetValidateCode()
        {
            var num = Common.CreateRandomNumber();

            string codeKey = Guid.NewGuid().ToString();
            _memoryCache.Set("ValidateCode_" + codeKey, num, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));

            byte[] bytes = Common.CreateValidateGraphic(num);
            var base64 = Convert.ToBase64String(bytes);
            Console.WriteLine("成功！");
            return new  { codeKey = codeKey, imageData = "data:image/jpeg;base64," + base64 };
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public (int code, string message) Logout(int userId)
        {
            return  (ApiResultCodeConst.SUCCESS, "登出成功");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="oldPassWord">旧密码</param>
        /// <param name="newPassWord">新密码</param>
        /// <returns></returns>
        public async Task<(int code, string message)> ModifiyPassWord(string userId, string oldPassWord, string newPassWord)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return (ApiResultCodeConst.ERROR, "登录已失效!");
            }
            int.TryParse(userId, out int _userId);
            var user = await SugarSimple.Instance().Queryable<D_USER_INFO>().Where(x => x.ID == _userId).FirstAsync();
            if (user == null)
            {
                return (ApiResultCodeConst.ERROR, "没有该用户!");
            }
            var _pwd = Common.MD5Encoding(oldPassWord, user.SALT);
            if (!user.PASSWORD.Equals(_pwd))
            {
                return (ApiResultCodeConst.ERROR, "原始密码不正确！");
            }
            user.PASSWORD = Common.MD5Encoding(newPassWord, user.SALT);
            int  insertedRow=await SugarSimple.Instance().Updateable<D_USER_INFO>(user).ExecuteCommandAsync();
            return insertedRow > 0 ? (ApiResultCodeConst.SUCCESS, "修改成功!") : (ApiResultCodeConst.ERROR, "修改失败!");
        }
    }
}
