using System.Collections.Generic;

namespace BtzjManagement.Api.Models.ViewModel
{
    /// <summary>
    /// controller分页实体
    /// </summary>
    /// <typeparam name="T">model</typeparam>
    public class Pager<T>
    {
        /// <summary>
        /// 集合
        /// </summary>
        public IEnumerable<T> list { get; set; } = new List<T>();
        /// <summary>
        /// 列表总数
        /// </summary>
        public int total { get; set; }
    }


    /// <summary>
    /// 公共查询放回实体 
    /// </summary>
    public class v_ApiResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        public v_ApiResult()
        {

        }
        /// <summary>
        /// cotr2
        /// </summary>
        /// <param name="content"></param>
        public v_ApiResult(dynamic content)
        {
            this.Content = content;

            this.Code = 1;
        }
        /// <summary>
        /// ctor3
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public v_ApiResult(int code, string message, dynamic content)
        {
            this.Message = message;
            this.Content = content;
            this.Code = code;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public dynamic Content { get; set; }
    }
}
