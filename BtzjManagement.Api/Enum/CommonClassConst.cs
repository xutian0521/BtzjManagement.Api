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

    /// <summary>
    /// 单位账号状态
    /// </summary>
    public class DwzhztConst
    {
        [Description("正常")]
        public const string 正常 = "0";
        [Description("封存")]
        public const string 封存 = "1";
        [Description("作废")]
        public const string 作废 = "2";
    }

    /// <summary>
    /// 个人账户状态
    /// </summary>
    public class GrzhztConst
    {
        [Description("正常")]
        public const string 正常 = "0";
        [Description("封存")]
        public const string 封存 = "1";
        [Description("转出")]
        public const string 转出 = "2";
        [Description("离休,退休")]
        public const string 离休退休 = "3";
        [Description("丧失劳动能力,并与单位终止关系")]
        public const string 丧失劳动能力并与单位终止关系 = "4";
        [Description("户口迁出本地,或出境定居")]
        public const string 户口迁出本地或出境定居 = "5";
        [Description("其他")]
        public const string 其他 = "6";
    }

    /// <summary>
    /// 个人开户类型
    /// </summary>
    public class KhtypeConst
    {
        [Description("按月汇缴")]
        public const int 按月汇缴 = 1;
        [Description("一次性汇缴")]
        public const int 一次性汇缴 = 2;
    }

    /// <summary>
    /// 业务流程状态
    /// </summary>
    public class OptStatusConst
    {
        [Description("新建:表示该业务数据已经创建，但尚未完成。在这个状态下，可以对业务数据进行编辑、修改等操作。")]
        public const string 新建 = "created";
        [Description("等待初审:表示该业务数据已经提交，正在初审相关人员进行审核。")]
        public const string 等待初审 = "chushening";
        [Description("初审未通过:表示该业务数据在后台代码处理中出现错误，等待初审人员再次审核或初审退回。")]
        public const string 初审出错 = "chushenerror";
        [Description("初审退回:表示该业务数据被初审人员退回。在这个状态下，可以对业务数据进行编辑、修改等操作。")]
        public const string 初审退回 = "chushengback";
        [Description("等待终审:表示初审通过等待终审或不需要初审直接等待终审")]
        public const string 等待终审 = "zhongshening";
        [Description("终审未通过:表示该业务数据在后台代码处理中出现错误，等待终审人员再次审核或退回。")]
        public const string 终审出错 = "zhongshenerror";
        [Description("终审退回:表示该业务数据被终审人员退回。在这个状态下，可以对业务数据进行编辑、修改等操作。")]
        public const string 终审退回 = "zhongshenback";
        [Description("已归档:表示该业务数据已经完成并且不再需要使用。在这个状态下，不允许再对业务数据进行任何操作。")]
        public const string 已归档 = "completed";
    }

    /// <summary>
    /// 性别
    /// </summary>
    public class XingBieConst 
    {
        [Description("男")]
        public const string 男 = "1";
        [Description("女")]
        public const string 女 = "1";
        [Description("未知")]
        public const string 未知 = null;
    }



}
