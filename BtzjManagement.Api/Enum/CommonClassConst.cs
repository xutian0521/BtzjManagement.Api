using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Enum
{
    /// <summary>
    /// 单位性质
    /// </summary>
    public class DwxzConst
    {
        [Description("行政")]
        public const string 行政 = "1";
        [Description("企业")]
        public const string 企业 = "2";
        [Description("事业")]
        public const string 事业 = "3";
        [Description("合资")]
        public const string 合资 = "4";
        [Description("私营")]
        public const string 私营 = "5";
    }
}
