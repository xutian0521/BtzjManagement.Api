using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Utils
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class BaseDbContext
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }
        /// <summary>
        /// 获得SqlSugarClient
        /// 注意当前方法的类不能是静态的 public static class这么写是错误的
        /// </summary>
        public static SqlSugarClient Instance
        {
            get
            {
                var db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = ConnectionString,
                    DbType = DbType.Oracle,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
                db.Ado.CommandTimeOut = 30000;//设置超时时间
                //db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
                //{
                //    Console.WriteLine($"执行时间：{db.Ado.SqlExecutionTime.TotalMilliseconds}毫秒 \r\nSQL如下：{sql} \r\n参数：{GetParams(pars)} ", "SQL执行");
                //};
                //db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
                //{
                //    if (db.TempItems == null) db.TempItems = new Dictionary<string, object>();
                //};
                //db.Aop.OnError = (exp) =>//执行SQL 错误事件
                //{
                //    Console.WriteLine($"SQL错误:{exp.Message}\r\nSQL如下：{exp.Sql}", "SQL执行");
                //    throw new Exception(exp.Message);
                //};
                //db.Aop.OnDiffLogEvent = (it) => //可以方便拿到 数据库操作前和操作后的数据变化。
                //{
                //    var editBeforeData = it.BeforeData; //变化前数据
                //    var editAfterData = it.AfterData; //变化后数据
                //    var sql = it.Sql; //SQL
                //    var parameter = it.Parameters; //参数
                //    var data = it.BusinessData; //业务数据
                //    var time = it.Time ?? new TimeSpan(); //时间
                //    var diffType = it.DiffType; //枚举值 insert 、update 和 delete 用来作业务区分

                //    //你可以在这里面写日志方法
                //    var log = $"时间:{time.TotalMilliseconds}\r\n";
                //    log += $"类型:{diffType.ToString()}\r\n";
                //    log += $"SQL:{sql}\r\n";
                //    log += $"参数:{GetParams(parameter)}\r\n";
                //    log += $"业务数据:{JsonHelper.ObjectToJson(data)}\r\n";
                //    log += $"变化前数据:{JsonHelper.ObjectToJson(editBeforeData)}\r\n";
                //    log += $"变化后数据:{JsonHelper.ObjectToJson(editAfterData)}\r\n";
                //    Console.WriteLine(log);
                //};
                return db;
            }
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        private static string GetParams(SugarParameter[] pars)
        {
            return pars.Aggregate("", (current, p) => current + $"{p.ParameterName}:{p.Value}, ");
        }


    }
}
