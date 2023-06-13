using BtzjManagement.Api.Enum;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class v_Busi_PersonInfo
    {
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }
        /// <summary>
        /// 开户类型(按月1，一次性2)
        /// </summary>
        public int KHTYPE { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public string STATUS { get; set; }

        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 业务月度
        /// </summary>
        public string YWYD { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZJLX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZJHM { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string XINGBIE { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? CSNY { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string SJHM { get; set; }
        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }
        /// <summary>
        /// 个人缴存基数
        /// </summary>
        public decimal GRJCJS { get; set; }
        /// <summary>
        /// 个人月缴存额
        /// </summary>
        public decimal GRYJCE { get; set; }
        /// <summary>
        /// 起缴日期
        /// </summary>
        public DateTime? QJRQ { get; set; }
        /// <summary>
        /// 个人账户状态
        /// </summary>
        public string GRZHZT { get; set; }
        /// <summary>
        /// 个人存款账户号码
        /// </summary>
        public string GRCKZHHM { get; set; }
        /// <summary>
        /// 个人存款账户开户名称
        /// </summary>
        public string GRCKZHKHMC { get; set; }
        /// <summary>
        /// 个人存款账户开户银行代码
        /// </summary>
        public string GRCKZHKHYHDM { get; set; }

    }


    public class v_Busi_Grkh
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string DWMC { get; set; }
        /// <summary>
        /// 开户类型(按月1，一次性2)
        /// </summary>
        public int KHTYPE { get; set; }
        /// <summary>
        /// 开户类型-DESC
        /// </summary>
        public string KHTYPE_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<int>(typeof(KhtypeConst), KHTYPE).desc;
            }
        }

        /// <summary>
        /// 业务状态
        /// </summary>
        public string STATUS { get; set; }
        /// <summary>
        /// 业务月度
        /// </summary>
        public string YWYD { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZJLX { get; set; }
        /// <summary>
        /// 证件类型-DESC
        /// </summary>
        public string ZJLX_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(ZjhmLxConst), ZJLX).desc;
            }
        }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZJHM { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string XINGBIE { get; set; }
        /// <summary>
        /// 性别-DESC
        /// </summary>
        public string XINGBIE_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(XingBieConst), XINGBIE).desc;
            }
        }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? CSNY { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string SJHM { get; set; }
        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }
        /// <summary>
        /// 个人缴存基数
        /// </summary>
        public decimal GRJCJS { get; set; }
        /// <summary>
        /// 个人月缴存额
        /// </summary>
        public decimal GRYJCE { get; set; }
        /// <summary>
        /// 起缴日期
        /// </summary>
        public DateTime? QJRQ { get; set; }
        /// <summary>
        /// 个人存款账户号码
        /// </summary>
        public string GRCKZHHM { get; set; }
        /// <summary>
        /// 个人存款账户开户名称
        /// </summary>
        public string GRCKZHKHMC { get; set; }
        /// <summary>
        /// 个人存款账户开户银行代码
        /// </summary>
        public string GRCKZHKHYHDM { get; set; }
        /// <summary>
        /// 个人缴存比例
        /// </summary>
        public decimal GRJCBL { get; set; }
        /// <summary>
        /// 单位月缴存额
        /// </summary>
        public decimal DWYJCE { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        public DateTime? WORK_DATE { get; set; }
        /// <summary>
        /// 固定电话号码
        /// </summary>
        public string GDDHHM { get; set; }


    }

    public class v_CustomerInfo
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CUSTID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZJLX { get; set; }
        /// <summary>
        /// 证件类型-DESC
        /// </summary>
        public string ZJLX_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(ZjhmLxConst), ZJLX).desc;
            }
        }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZJHM { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string XINGBIE { get; set; }
        /// <summary>
        /// 性别-DESC
        /// </summary>
        public string XINGBIE_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(XingBieConst), XINGBIE).desc;
            }
        }
        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime? CSNY { get; set; }

        /// <summary>
        /// 参加工作时间
        /// </summary>
        public DateTime? WORK_DATE { get; set; }
        /// <summary>
        /// 固定电话号码
        /// </summary>
        public string GDDHHM { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string SJHM { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string HYZK { get; set; }
        /// <summary>
        /// 婚姻状况-DESC
        /// </summary>
        public string HYZK_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(HyzkConst), HYZK).desc;
            }
        }
        /// <summary>
        /// 配偶客户编号
        /// </summary>
        public string MATE_CUSTID { get; set; }

        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 个人账号
        /// </summary>
        public string GRZH { get; set; }
        /// <summary>
        /// 个人账户状态
        /// </summary>
        public string GRZHZT { get; set; }
        /// <summary>
        /// 个人账户状态-DESC
        /// </summary>
        public string GRZHZT_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(GrzhztConst), GRZHZT).desc;
            }
        }
        /// <summary>
        /// 开户日期
        /// </summary>
        public DateTime? KHRQ { get; set; }
        /// <summary>
        /// 封存日期
        /// </summary>
        public DateTime? LOCK_DATE { get; set; }
        /// <summary>
        /// 封存原因
        /// </summary>
        public string LOCK_REASON { get; set; }
        /// <summary>
        /// 封存原因-DESC
        /// </summary>
        public string LOCK_REASON_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(LockReasonConst), LOCK_REASON).desc;
            }
        }
        /// <summary>
        /// 启封日期
        /// </summary>
        public DateTime? UNLOCK_DATE { get; set; }
        /// <summary>
        /// 销户日期
        /// </summary>
        public DateTime? XHRQ { get; set; }
        /// <summary>
        /// 销户原因
        /// </summary>
        public string XHYY { get; set; }
        /// <summary>
        /// 账户类型 0：按月 1:一次性
        /// </summary>
        public string ACCT_TYPE { get; set; }
        /// <summary>
        /// 账户类型-DESC
        /// </summary>
        public string ACCT_TYPE_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<int>(typeof(KhtypeConst), Convert.ToInt32(ACCT_TYPE)).desc;
            }
        }

        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }
        /// <summary>
        /// 个人缴存比例
        /// </summary>
        public decimal GRJCBL { get; set; }
        /// <summary>
        /// 个人缴存基数
        /// </summary>
        public decimal GRJCJS { get; set; }
        /// <summary>
        /// 单位月缴存额
        /// </summary>
        public decimal DWYJCE { get; set; }
        /// <summary>
        /// 个人月缴存额
        /// </summary>
        public decimal GRYJCE { get; set; }
        /// <summary>
        /// 月汇缴额
        /// </summary>
        public decimal MONTHPAYAMT { get; set; }
        /// <summary>
        /// 末次汇缴月
        /// </summary>
        public DateTime? LASTPAYMONTH { get; set; }
        /// <summary>
        /// 起缴日期
        /// </summary>
        public DateTime? QJRQ { get; set; }
        /// <summary>
        /// 一次性补贴金额
        /// </summary>
        public decimal YCXBTJE { get; set; }
        /// <summary>
        ///  一次性缴存状态 0：未缴交 1：生成汇缴业务未记账 2：已复核
        /// </summary>
        public string YCX_CHECK_FLAG { get; set; }
        /// <summary>
        /// 一次性缴存状态-DESC
        /// </summary>
        public string YCX_CHECK_FLAG_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(YcxCheckFlagConst), YCX_CHECK_FLAG).desc;
            }
        }
        /// <summary>
        /// 个人账户余额
        /// </summary>
        public decimal GRZHYE { get; set; }
        /// <summary>
        /// 个人账户上年结转余额
        /// </summary>
        public decimal GRZHSNJZYE { get; set; }
        /// <summary>
        /// 个人账户上年结转日期
        /// </summary>
        public DateTime? GRZHSNJZRQ { get; set; }
        /// <summary>
        /// 个人账户当年归集余额
        /// </summary>
        public decimal GRZHDNGJYE { get; set; }
        /// <summary>
        /// 个人存款账户号码
        /// </summary>
        public string GRCKZHHM { get; set; }
        /// <summary>
        /// 个人存款账户开户名称
        /// </summary>
        public string GRCKZHKHMC { get; set; }
        /// <summary>
        /// 个人存款账户开户银行代码
        /// </summary>
        public string GRCKZHKHYHDM { get; set; }
        /// <summary>
        /// 核定标志
        /// </summary>
        public string CHECK_FLAG { get; set; }
        /// <summary>
        /// 核定标志-DESC
        /// </summary>
        public string CHECK_FLAG_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(AyhjCheckFlagConst), CHECK_FLAG).desc;
            }
        }
        /// <summary>
        /// 城市网点
        /// </summary>
        public string CITY_CENTNO { get; set; }
    }


    public class v_CorporationPersonInfo
    {
        /// <summary>
        /// 单位账号
        /// </summary>
        public string dwzh { get; set; }
        /// <summary>
        /// 单位职工人数
        /// </summary>
        public int DWZGRS { get; set; }
        /// <summary>
        /// 单位封存人数
        /// </summary>
        public int DWFCRS { get; set; }
        /// <summary>
        /// 单位缴存人数
        /// </summary>
        public int DWJCRS { get; set; }
        /// <summary>
        /// 工资总数
        /// </summary>
        public decimal FACTINCOME { get; set; }
        /// <summary>
        /// 月缴存总额
        /// </summary>
        public decimal MONTHPAYTOTALAMT { get; set; }
    }
}
