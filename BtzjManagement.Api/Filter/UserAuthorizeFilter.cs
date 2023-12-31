﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using JWT;
using JWT.Serializers;
using System.Linq;
using BtzjManagement.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using BtzjManagement.Api.Models.ViewModel;

namespace BtzjManagement.Api.Filter
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class UserAuthorizeFilter : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// 授权过滤器
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            string token = string.Empty;
            try
            {
                token = filterContext.HttpContext.Request.Query["Authorization"];
                if (string.IsNullOrEmpty(token))
                {
                    string header = filterContext.HttpContext.Request.Headers["Authorization"];
                    if (!string.IsNullOrEmpty(header))
                    {
                        token = header.Replace("Bearer ", "");
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("用户认证:Token format invalid :" + ex.ToString());
            }
            var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
            try
            {

                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var json = decoder.Decode(token, secret, verify: true);
                var payload = System.Text.Json.JsonSerializer.Deserialize<JwtPayload>(json);

                // attach user to context on successful jwt validation
                filterContext.HttpContext.Items["User"] = payload;
                var UtcNow = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                bool TokenIsExpire = payload.exp < UtcNow;

                if (!string.IsNullOrEmpty(payload.userId))
                {
                    if (TokenIsExpire)
                    {
                        payload.userId = "";
                    }
                }


                Console.WriteLine(json);
            }
            catch (FormatException)
            {
            }
            catch (TokenExpiredException)
            {
                if (!filterContext.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute)))
                {
                    filterContext.Result = new StatusCodeResult(401);
                }

            }
            catch (SignatureVerificationException)
            {

            }
            catch (Exception)
            {

            }
            return;
        }

    }
}
