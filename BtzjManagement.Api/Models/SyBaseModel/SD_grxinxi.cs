using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Models.SyBaseModel
{
    public class SD_grxinxi
    {
        private string dwzh { get; set; }
        private string grzh { get; set; }
        private string grxm { get; set; }
        private string grxb { get; set; }
        private string sfzhm { get; set; }
        private DateTime? csny { get; set; }
        private string s_lxdh { get; set; }
        private string s_sjh { get; set; }


        public string dwzh_gbk { get { return dwzh; } }
        public string grzh_gbk { get { return grzh; } }
        public string grxm_gbk { get { return Common.Cp850ToGBK(grxm); } }
        public string grxb_gbk { get { return grxb; } }
        public string sfzhm_gbk { get { return sfzhm; } }
        public DateTime? csny_gbk { get { return csny; } }
        public string s_lxdh_gbk { get { return Common.Cp850ToGBK(s_lxdh); } }
        public string s_sjh_gbk { get { return Common.Cp850ToGBK(s_sjh); } }
    }
}


//ID
//CUST_ID               客户编号_1	VARCHAR2(120)

//XINGMING            姓名	    VARCHAR2(120)
//ZJLX               证件类型	VARCHAR2(2)
//ZJHM                证件号码	VARCHAR2(18)
//XINGBIE               性别	VARCHAR2(1)
//CSNY               出生年月	DATE
//NATION2             民族	        CHAR(2)
//WORKDATE            参加工作时间_2	VARCHAR2(6)

//GDDHHM              固定电话号码	 VARCHAR2(20)
//SJHM                手机号码	   VARCHAR2(11)


//DWDZ               单位地址	    VARCHAR2(255)

//HYZK            婚姻状况	VARCHAR2(2)
//MATECUSTID      配偶客户编号_1	NVARCHAR2(120)

//CREATEUSER 创建用户	VARCHAR2(40)
//CREATETIME 创建时间_2	TIMESTAMP(6)

//UPDATEUSER 最后修改用户	VARCHAR2(40)
//UPDATETIME 最后修改时间_2	TIMESTAMP(6)
//OPERID 操作员	VARCHAR2(80)
//LASTDEALDATE 日期戳_1	TIMESTAMP(6)
//CITY_CENTNO 城市网点	        VARCHAR2(10)



























