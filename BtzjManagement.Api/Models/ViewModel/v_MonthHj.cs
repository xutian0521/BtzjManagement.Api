﻿using BtzjManagement.Api.Enum;
using BtzjManagement.Api.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.ViewModel
{
    public class v_MonthHjCreate
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string batchNo { get; set; }
    }

    public class v_MonthHj
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public string YWLSH { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BATCHNO { get; set; }

        /// <summary>
        /// 单位账号
        /// </summary>
        public string DWZH { get; set; }

        /// <summary>
        /// 单位缴存比例
        /// </summary>
        public decimal DWJCBL { get; set; }

        /// <summary>
        /// 个人缴存比例
        /// </summary>
        public decimal GRJCBL { get; set; }

        /// <summary>
        /// 汇缴月份
        /// </summary>
        public string PAYMTH { get; set; }

        /// <summary>
        /// 本月汇缴人数
        /// </summary>
        public int DWJCRS { get; set; }

        /// <summary>
        /// 本月汇缴金额
        /// </summary>
        public decimal MTHPAYAMT { get; set; }

        /// <summary>
        /// 上月汇缴人数
        /// </summary>
        public int LASTMTHPAYNUM { get; set; }

        /// <summary>
        /// 上月汇缴金额
        /// </summary>
        public decimal LASTMTHPAY { get; set; }

        /// <summary>
        /// 本月增加人数
        /// </summary>
        public int MTHPAYNUMPLS { get; set; }

        /// <summary>
        /// 本月增加金额
        /// </summary>
        public decimal MTHPAYAMTPLS { get; set; }

        /// <summary>
        /// 本月减少人数
        /// </summary>
        public int MTHPAYNUMMNS { get; set; }

        /// <summary>
        /// 本月减少金额
        /// </summary>
        public decimal MTHPAYAMTMNS { get; set; }

        /// <summary>
        /// 基数调整人数
        /// </summary>
        public int BASECHGNUM { get; set; }

        /// <summary>
        /// 基数调整金额
        /// </summary>
        public decimal BASECHGAMT { get; set; }

        /// <summary>
        /// 保存时间
        /// </summary>
        public DateTime? CREATE_TIME { get; set; }

        /// <summary>
        /// 保存人
        /// </summary>
        public string CREATE_MAN { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? SUBMIT_TIME { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public string SUBMIT_MAN { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? VERIFY_TIME { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string VERIFY_MAN { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        public string STATUS { get; set; }
        public string STATUS_DESC { get { return EnumHelper.GetEnumItemByValue<string>(typeof(OptStatusConst), STATUS).key; } }

        /// <summary>
        /// 凭证号字符串
        /// </summary>
        public string VCHRNOS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }
    }
}
