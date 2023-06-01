using BtzjManagement.Api.Models.QueryModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Services;
using BtzjManagement.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    /// <summary>
    /// 账户相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        AccountService _accountService;
        /// <summary>
        /// ctor
        /// </summary>
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="code">验证码</param>
        /// <param name="codeKey">验证码key</param>
        /// <returns></returns>
        [HttpPost("Login"), AllowAnonymous]
        public async Task<v_ApiResult> Login([FromBody]P_AccountLogin model)
        {
            
            var result = await _accountService.Login(model.userName, model.password, model.code, model.codeKey, this.HttpContext);
            return new v_ApiResult(result.code, result.message, result.loginResult);
        }
        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetValidateCode")]
        public v_ApiResult GetValidateCode()
        {
            var result = _accountService.GetValidateCode();
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<v_ApiResult> Logout([FromForm] int userId)
        {
            var result = _accountService.Logout(userId);
            return new v_ApiResult(result.code, result.message);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassWord">旧密码</param>
        /// <param name="newPassWord">新密码</param>
        /// <param name="cityCode">城市代码</param>
        /// <returns></returns>
        [HttpPost("ModifiyPassWord")]
        public async Task<v_ApiResult> ModifiyPassWord(
            [FromForm] string oldPassWord, [FromForm] string newPassWord, [FromForm] string cityCode)
        {
            var user = this.HttpContext.Items["User"] as JwtPayload;
            var result = await _accountService.ModifiyPassWord(user.userId, oldPassWord, newPassWord);
            return new v_ApiResult(result.code, result.message);
        }

    }
}
