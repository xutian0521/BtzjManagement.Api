using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
{
    public class BaseController : ControllerBase
    {

        IConfiguration _configuration;
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 获取网点编号
        /// </summary>
        /// <returns></returns>
        internal string CityCent()
        {
            return _configuration.GetValue<string>("CityCent");
        }

        internal JwtPayload GetUser()
        {
            var user = /*new JwtPayload { userName = "llll" };*/ this.HttpContext.Items["User"] as JwtPayload;
            return user;
        }

    }
}
