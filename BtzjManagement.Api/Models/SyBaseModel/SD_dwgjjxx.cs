using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.SyBaseModel
{
    public class SD_dwgjjxx
    {
        private string dwzh { get; set; }
        private string yjje { get; set; }
        private string xcrq { get; set; }
        private string fcbj { get; set; }
        private string dwjj_bili { get; set; }
        private string grjj_bili { get; set; }
        private string czbt_bili { get; set; }
        private string i_yh_type { get; set; }
        private string s_yh_dwzh { get; set; }
        private string i_jsff { get; set; }
        private string i_btly { get; set; }
        private string i_can_dk { get; set; }
        private string i_zjbj { get; set; }
        private string dc_gj_dwbl { get; set; }
        private string dc_gj_grbl { get; set; }
        private string dc_gj_czbl { get; set; }
        private string i_gj_jsff { get; set; }
        private string dt_scdy { get; set; }
        private string dt_bj_scdy { get; set; }


        public string dwzh_gbk { get { return Common.Cp850ToGBK(dwzh); } }
        public string yjje_gbk { get { return Common.Cp850ToGBK(yjje); } }
        public string xcrq_gbk { get { return Common.Cp850ToGBK(xcrq); } }
        public string fcbj_gbk { get { return Common.Cp850ToGBK(fcbj); } }
        public string dwjj_bili_gbk { get { return Common.Cp850ToGBK(dwjj_bili); } }
        public string grjj_bili_gbk { get { return Common.Cp850ToGBK(grjj_bili); } }
        public string czbt_bili_gbk { get { return Common.Cp850ToGBK(czbt_bili); } }
        public string i_yh_type_gbk { get { return Common.Cp850ToGBK(i_yh_type); } }
        public string s_yh_dwzh_gbk { get { return Common.Cp850ToGBK(s_yh_dwzh); } }
        public string i_jsff_gbk { get { return Common.Cp850ToGBK(i_jsff); } }
        public string i_btly_gbk { get { return Common.Cp850ToGBK(i_btly); } }
        public string i_can_dk_gbk { get { return Common.Cp850ToGBK(i_can_dk); } }
        public string i_zjbj_gbk { get { return Common.Cp850ToGBK(i_zjbj); } }
        public string dc_gj_dwbl_gbk { get { return Common.Cp850ToGBK(dc_gj_dwbl); } }
        public string dc_gj_grbl_gbk { get { return Common.Cp850ToGBK(dc_gj_grbl); } }
        public string dc_gj_czbl_gbk { get { return Common.Cp850ToGBK(dc_gj_czbl); } }
        public string i_gj_jsff_gbk { get { return Common.Cp850ToGBK(i_gj_jsff); } }
        public string dt_scdy_gbk { get { return Common.Cp850ToGBK(dt_scdy); } }
        public string dt_bj_scdy_gbk { get { return Common.Cp850ToGBK(dt_bj_scdy); } }
    }
}



