using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.SyBaseModel
{
    public class SD_journey_config
    {
        private string i_flag { get; set; }
        private string s_name { get; set; }
        private string i_value { get; set; }
        private string s_value { get; set; }

        public string i_flag_gbk { get { return Common.Cp850ToGBK(i_flag); } set { i_flag = value; } }
        public string s_name_gbk { get { return Common.Cp850ToGBK(s_name); } set { s_name = value; } }
        public string i_value_gbk { get { return Common.Cp850ToGBK(i_value); } set { i_value = value; } }
        public string s_value_gbk { get { return Common.Cp850ToGBK(s_value); } set { s_value = value; } }

    }
}
