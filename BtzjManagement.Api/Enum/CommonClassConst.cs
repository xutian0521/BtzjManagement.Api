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
        #region 业务审核流程相关
        [Description("新建:表示该业务数据已经创建，但尚未完成。在这个状态下，可以对业务数据进行编辑、修改等操作。")]
        public const string 新建 = "created";
        [Description("等待初审:表示该业务数据已经提交，正在初审相关人员进行审核。")]
        public const string 等待初审 = "chushening";
        [Description("初审出错:表示该业务数据在后台代码处理中出现错误，等待初审人员再次审核或初审退回。")]
        public const string 初审出错 = "chushenerror";
        [Description("初审退回:表示该业务数据被初审人员退回。在这个状态下，可以对业务数据进行编辑、修改等操作。")]
        public const string 初审退回 = "chushengback";
        [Description("等待终审:表示初审通过等待终审或不需要初审直接等待终审")]
        public const string 等待终审 = "zhongshening";
        [Description("终审出错:表示该业务数据在后台代码处理中出现错误，等待终审人员再次审核或退回。")]
        public const string 终审出错 = "zhongshenerror";
        [Description("终审退回:表示该业务数据被终审人员退回。在这个状态下，可以对业务数据进行编辑、修改等操作。")]
        public const string 终审退回 = "zhongshenback";
        [Description("已归档:表示该业务数据已经完成并且不再需要使用。在这个状态下，不允许再对业务数据进行任何操作。")]
        public const string 已归档 = "completed";
        #endregion

        #region 业务操作流程相关
        [Description("修改:表示该业务数据创建后被修改了。")]
        public const string 修改 = "update";
        #endregion
    }

    /// <summary>
    /// 性别
    /// </summary>
    public class XingBieConst 
    {
        [Description("男")]
        public const string 男 = "1";
        [Description("女")]
        public const string 女 = "0";
        [Description("未知")]
        public const string 未知 = null;
    }

    /// <summary>
    /// 公积金操作类型
    /// </summary>
    public class GjjOptType
    {
        [Description("单位汇缴")]
        public const int 单位汇缴 = 1;
        [Description("个人补缴")]
        public const int 个人补缴 = 3;
        [Description("个人结息")]
        public const int 个人结息 = 6;
        [Description("同行转入")]
        public const int 同行转入 = 9;
        [Description("单位转移转入")]
        public const int 单位转移转入 = 12;
        [Description("单位转移转出")]
        public const int 单位转移转出 = 13;
        [Description("同行转出")]
        public const int 同行转出 = 14;
        [Description("个人转入")]
        public const int 个人转入 = 15;
        [Description("个人转出")]
        public const int 个人转出 = 16;
        [Description("公积金个人调帐")]
        public const int 公积金个人调帐 = 18;
        [Description("公积金个人转入")]
        public const int 公积金个人转入 = 19;
        [Description("公积金个人转出")]
        public const int 公积金个人转出 = 20;
        [Description("购买,建造,翻建大修自住住房支取")]
        public const int 购买_建造_翻建大修自住住房支取 = 51;
        [Description("房租超出家庭工资收入的规定比例支取")]
        public const int 房租超出家庭工资收入的规定比例支取 = 52;
        [Description("偿还购房贷款本息支取")]
        public const int 偿还购房贷款本息支取 = 53;
        [Description("其他原因支取")]
        public const int 其他原因支取 = 54;
        [Description("离休,退休销户")]
        public const int 离休_退休销户 = 61;
        [Description("丧失劳动能力,并与单位终止关系销户")]
        public const int 丧失劳动能力_并与单位终止关系销户 = 62;
        [Description("户口迁出本地,或出境定居销户")]
        public const int 户口迁出本地_或出境定居销户 = 63;
        [Description("其他原因销户")]
        public const int 其他原因销户 = 64;
        [Description("红冲单位汇缴")]
        public const int 红冲单位汇缴 = 501;
        [Description("红冲个人补缴")]
        public const int 红冲个人补缴 = 503;
        [Description("红冲个人结息")]
        public const int 红冲个人结息 = 506;
        [Description("红冲同行转入")]
        public const int 红冲同行转入 = 509;
        [Description("红冲单位转移转入")]
        public const int 红冲单位转移转入 = 512;
        [Description("红冲单位转移转出")]
        public const int 红冲单位转移转出 = 513;
        [Description("")]
        public const int 红冲同行转出 = 514;
        [Description("红冲同行转出")]
        public const int 红冲个人转入 = 515;
        [Description("红冲个人转出")]
        public const int 红冲个人转出 = 516;
        [Description("红冲公积金个人调帐")]
        public const int 红冲公积金个人调帐 = 518;
        [Description("红冲公积金个人转入")]
        public const int 红冲公积金个人转入 = 519;
        [Description("红冲公积金个人转出")]
        public const int 红冲公积金个人转出 = 520;
        [Description("红冲购买,建造,翻建大修自住住房支取")]
        public const int 红冲购买_建造_翻建大修自住住房支取 = 551;
        [Description("红冲房租超出家庭工资收入的规定比例支取")]
        public const int 红冲房租超出家庭工资收入的规定比例支取 = 552;
        [Description("红冲偿还购房贷款本息支取")]
        public const int 红冲偿还购房贷款本息支取 = 553;
        [Description("红冲其他原因支取")]
        public const int 红冲其他原因支取 = 554;
        [Description("红冲离休,退休销户")]
        public const int 红冲离休_退休销户 = 561;
        [Description("红冲丧失劳动能力,并与单位终止关系销户")]
        public const int 红冲丧失劳动能力_并与单位终止关系销户 = 562;
        [Description("红冲户口迁出本地,或出境定居销户")]
        public const int 红冲户口迁出本地_或出境定居销户 = 563;
        [Description("红冲其他原因销户")]
        public const int 红冲其他原因销户 = 564;
        [Description("单位开户")]
        public const int 单位开户 = 1000;
        [Description("个人开户")]
        public const int 个人开户 = 1001;
    }


}





    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
	