﻿using BtzjManagement.Api.Enum;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.ViewModel
{
    /// <summary>
    /// 用于筛选
    /// </summary>
    public class v_CorporationSelect
    {
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string DWMC { get; set; }
        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }
        /// <summary>
        /// 下次应缴日期
        /// </summary>
        public string NEXTPAYMTH { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string ZZJGDM { get; set; }
        /// <summary>
        /// 个人缴存比例
        /// </summary>
        public decimal GRJCBL { get; set; }
        /// <summary>
        /// 计算方法
        /// </summary>
        public string CALC_METHOD { get; set; }

        /// <summary>
        /// 缴至年月
        /// </summary>
        public string JZNY { get; set; }
    }

    /// <summary>
    /// 单位保存状态数据model
    /// </summary>
    public class v_CorporationCreated
    {
        /// <summary>
        /// 
        /// </summary>
        public v_BaseCorporatiorn BaseModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string YWLSH { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class v_BaseCorporatiorn
    {
        /// <summary>
        /// 单位名称
        /// </summary>
        public string DWMC { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        public string DWDZ { get; set; }
        /// <summary>
        /// 单位性质
        /// </summary>
        public string DWXZ { get; set; }
        /// <summary>
        /// 单位性质-DESC
        /// </summary>
        public string DWXZ_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(DwxzConst), DWXZ).desc;
            }
        }

        /// <summary>
        /// 单位邮编
        /// </summary>
        public string DWYB { get; set; }
        /// <summary>
        /// 单位发薪日
        /// </summary>
        public string DWFXR { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string ZZJGDM { get; set; }
        /// <summary>
        /// 单位设立日期
        /// </summary>
        public DateTime? DWSLRQ { get; set; }

        /// <summary>
        /// 单位起缴日期
        /// </summary>
        public DateTime? DWQJRQ { get; set; }
        /// <summary>
        /// 基本存款户开户行
        /// </summary>
        public string BASICACCTBRCH { get; set; }
        /// <summary>
        /// 基本存款户账号
        /// </summary>
        public string BASICACCTNO { get; set; }
        /// <summary>
        /// 基本存款户名称
        /// </summary>
        public string BASICACCTMC { get; set; }
        /// <summary>
        /// 单位法人代表姓名
        /// </summary>
        public string DWFRDBXM { get; set; }
        /// <summary>
        /// 单位法人代表证件类型
        /// </summary>
        public string DWFRDBZJLX { get; set; }
        /// <summary>
        /// 单位法人代表证件类型-DESC
        /// </summary>
        public string DWFRDBZJLX_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(ZjhmLxConst), DWFRDBZJLX).desc;
            }
        }
        /// <summary>
        /// 单位法人代表证件号码
        /// </summary>
        public string DWFRDBZJHM { get; set; }
        /// <summary>
        /// 经办人姓名
        /// </summary>
        public string JBRXM { get; set; }
        /// <summary>
        /// 经办人固定电话号码
        /// </summary>
        public string JBRGDDHHM { get; set; }
        /// <summary>
        /// 经办人手机号码
        /// </summary>
        public string JBRSJHM { get; set; }
        /// <summary>
        /// 经办人证件类型
        /// </summary>
        public string JBRZJLX { get; set; }
        /// <summary>
        /// 经办人证件类型-DESC
        /// </summary>
        public string JBRZJLX_DESC {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(ZjhmLxConst), JBRZJLX).desc;
            }
        }
        /// <summary>
        /// 经办人证件号码
        /// </summary>
        public string JBRZJHM { get; set; }
        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }
        /// <summary>
        /// 下次应缴日期
        /// </summary>
        public DateTime? NEXTPAYMTH { get; set; }
        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }
        /// <summary>
        /// 补贴资金来源
        /// </summary>
        public string FROMFLAG { get; set; }
        /// <summary>
        /// 补贴资金来源-desc
        /// </summary>
        public string FROMFLAG_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(FromFlagConst), FROMFLAG).desc;
            }
        }
        /// <summary>
        /// 受托银行名称
        /// </summary>
        public string STYHMC { get; set; }
        /// <summary>
        /// 受托银行代码
        /// </summary>
        public string STYHDM { get; set; }
        /// <summary>
        /// 受托银行账号
        /// </summary>
        public string STYHZH { get; set; }
        /// <summary>
        /// 计算方法
        /// </summary>
        public string CALC_METHOD { get; set; }
        /// <summary>
        /// 计算方法-desc
        /// </summary>
        public string CALC_METHOD_DESC
        {
            get
            {
                return EnumHelper.GetEnumItemByValue<string>(typeof(CalcMethodConst), CALC_METHOD).desc;
            }
        }

        /// <summary>
        /// 单位职工人数
        /// </summary>
        public int DWZGRS { get; set; }
        /// <summary>
        /// 单位缴存人数
        /// </summary>
        public int DWJCRS { get; set; }
        /// <summary>
        /// 单位封存人数
        /// </summary>
        public int DWFCRS { get; set; }
        /// <summary>
        /// 工资总数
        /// </summary>
        public decimal FACTINCOME { get; set; }
        /// <summary>
        /// 月缴存总额
        /// </summary>
        public decimal MONTHPAYTOTALAMT { get; set; }
        /// <summary>
        /// 缴至年月
        /// </summary>
        public string JZNY { get; set; }
        /// <summary>
        /// 个人缴存比例
        /// </summary>
        public decimal GRJCBL { get; set; }
        /// <summary>
        /// 单位账户余额
        /// </summary>
        public decimal DWZHYE { get; set; }
        /// <summary>
        /// 挂账户余额
        /// </summary>
        public decimal REGHANDBAL { get; set; }
    }

    /// <summary>
    /// 按月汇缴页面初始化数据
    /// </summary>
    public class v_PaymentMonthInit
    {
        /// <summary>
        /// 单位信息
        /// </summary>
        public v_BaseCorporatiorn corporatiornInfo { get; set; }
        /// <summary>
        /// 用户信息分页数据
        /// </summary>
        //public Pager<v_CustomerInfo> pager;
    }

}
