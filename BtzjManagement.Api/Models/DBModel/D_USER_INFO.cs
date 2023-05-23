using SqlSugar;
using System;

namespace BtzjManagement.Api.Models.DBModel
{
    [SugarTable("USER_INFO")]
    public class D_USER_INFO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "USER_INFO_SEQ")]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string REAL_NAME { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PASSWORD { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public string RULE_ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATE_TIME { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }
    }
}
