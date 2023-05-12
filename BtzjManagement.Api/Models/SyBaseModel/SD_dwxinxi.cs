using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.SyBaseModel
{
    //[Table("dwxinxi")]
    public class SD_dwxinxi
    {
        private string dwzh { get; set; }
        private string dwmc { get; set; }
        private string dwdz { get; set; }
        private string yzbm { get; set; }
        private string dwxz { get; set; }
        private string dwdh { get; set; }
        private string dwlxr { get; set; }
        private string dwrshu { get; set; }
        private string maxz { get; set; }
        private string s_jzd_zh { get; set; }
        private string s_jzd_name { get; set; }
        private string s_jzd_yh_name { get; set; }
        private string dt_qjrq { get; set; }
        private string s_frdb { get; set; }
        private string s_zzjgdm { get; set; }
        private string dt_kaihu { get; set; }

        public string dwzh_gbk { get { return Common.Cp850ToGBK(dwzh); }set { } }
        public string dwmc_gbk { get { return Common.Cp850ToGBK(dwmc); } set { } }
        public string dwdz_gbk { get { return Common.Cp850ToGBK(dwdz); } set { } }
        public string yzbm_gbk { get { return Common.Cp850ToGBK(yzbm); } set { } }
        public string dwxz_gbk { get { return Common.Cp850ToGBK(dwxz); } set { } }
        public string dwdh_gbk { get { return Common.Cp850ToGBK(dwdh); } set { } }
        public string dwlxr_gbk { get { return Common.Cp850ToGBK(dwlxr); } set { } }
        public string dwrshu_gbk { get { return Common.Cp850ToGBK(dwrshu); } set { } }
        public string maxz_gbk { get { return Common.Cp850ToGBK(maxz); } set { } }
        public string s_jzd_zh_gbk { get { return Common.Cp850ToGBK(s_jzd_zh); } set { } }
        public string s_jzd_name_gbk { get { return Common.Cp850ToGBK(s_jzd_name); } set { } }
        public string s_jzd_yh_name_gbk { get { return Common.Cp850ToGBK(s_jzd_yh_name); } set { } }
        public string dt_qjrq_gbk { get { return Common.Cp850ToGBK(dt_qjrq); } set { } }
        public string s_frdb_gbk { get { return Common.Cp850ToGBK(s_frdb); } set { } }
        public string s_zzjgdm_gbk { get { return Common.Cp850ToGBK(s_zzjgdm); } set { } }
        public string dt_kaihu_gbk { get { return Common.Cp850ToGBK(dt_kaihu); } set { } }
    }
}

