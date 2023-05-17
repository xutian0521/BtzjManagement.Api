using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using BtzjManagement.Api.Models.ViewModel;
using System.Text;
using BtzjManagement.Api.Models.SyBaseModel;
using BtzjManagement.Api.Models.DBModel;
using Dapper.Contrib.Extensions;
using System.Transactions;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace BtzjManagement.Api.Services
{
    public class DataTransferService
    {
        IConfiguration _configuration;
        public DataTransferService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// 单位基本信息表结构初始化
        /// </summary>
        /// <returns></returns>
        public void CorporationBasicInfoInitStructure()
        {
            #region 表结构初始化
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "CUSTID", columnTypeAndLimit = "nvarchar2(120) UNIQUE", columnDesc = "客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWMC", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "单位名称" });
            v_TableInits.Add(new v_TableInit { columnName = "DWDZ", columnTypeAndLimit = "nvarchar2(400)", columnDesc = "单位地址" });
            v_TableInits.Add(new v_TableInit { columnName = "DWXZ", columnTypeAndLimit = "varchar2(3byte)", columnDesc = "单位性质" });
            v_TableInits.Add(new v_TableInit { columnName = "DWYB", columnTypeAndLimit = "varchar2(6byte)", columnDesc = "单位邮编" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFXR", columnTypeAndLimit = "varchar2(2byte)", columnDesc = "单位发薪日" });
            v_TableInits.Add(new v_TableInit { columnName = "USCCID", columnTypeAndLimit = "varchar2(20byte)", columnDesc = "统一社会信用代码" });
            v_TableInits.Add(new v_TableInit { columnName = "DWSLRQ", columnTypeAndLimit = "DATE", columnDesc = "单位设立日期" });
            v_TableInits.Add(new v_TableInit { columnName = "DWKHRQ", columnTypeAndLimit = "DATE", columnDesc = "单位开户日期" });
            v_TableInits.Add(new v_TableInit { columnName = "DWQJRQ", columnTypeAndLimit = "DATE", columnDesc = "单位起缴日期" });
            v_TableInits.Add(new v_TableInit { columnName = "BASICACCTBRCH", columnTypeAndLimit = "nvarchar2(360)", columnDesc = "基本存款户开户行" });
            v_TableInits.Add(new v_TableInit { columnName = "BASICACCTNO", columnTypeAndLimit = "nvarchar2(30)", columnDesc = "基本存款户账号" });
            v_TableInits.Add(new v_TableInit { columnName = "BASICACCTMC", columnTypeAndLimit = "nvarchar2(360)", columnDesc = "基本存款户名称" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFRDBXM", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "单位法人代表姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFRDBZJLX", columnTypeAndLimit = "varchar2(4byte)", columnDesc = "单位法人代表证件类型" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFRDBZJHM", columnTypeAndLimit = "varchar2(18byte)", columnDesc = "单位法人代表证件号码" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRXM", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "经办人姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRGDDHHM", columnTypeAndLimit = "varchar2(20byte)", columnDesc = "经办人固定电话号码" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRSJHM", columnTypeAndLimit = "varchar2(20byte)", columnDesc = "经办人手机号码" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRZJLX", columnTypeAndLimit = "varchar2(4byte)", columnDesc = "经办人证件类型" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRZJHM", columnTypeAndLimit = "varchar2(18byte)", columnDesc = "经办人证件号码" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "varchar2(360byte)", columnDesc = "城市网点编号" });
            v_TableInits.Add(new v_TableInit { columnName = "OPERID", columnTypeAndLimit = "varchar2(360byte)", columnDesc = "操作员" });
            this.TableInit("CORPORATION_BASICINFO", v_TableInits, true, "单位基本信息表");
            #endregion
        }


        /// <summary>
        /// 单位基本信息表数据初始化
        /// </summary>
        /// <returns></returns>
        public void CorporationBasicInfoInitData()
        {
            #region 表数据初始化
            var city_cent = _configuration.GetValue<string>("CityCent");
            using (var syBaseConn = SybaseConnector.Conn())
            {
                //获取原基本信息表
                var org_dwXinxiSql = @" select * from dwxinxi order by dwzh asc ";
                List<SD_dwxinxi> sD_Dwxinxis = syBaseConn.Query<SD_dwxinxi>(org_dwXinxiSql).ToList();
                var models = sD_Dwxinxis.Select(x =>
                new D_CORPORATION_BASICINFO
                {
                    CUSTID = Common.PaddingDwzh(Convert.ToInt32(x.dwzh_gbk), 8),
                    USCCID = x.s_zzjgdm_gbk,
                    DWDZ = x.dwdz_gbk,
                    DWFRDBXM = x.s_frdb_gbk,
                    DWKHRQ = string.IsNullOrEmpty(x.dt_kaihu_gbk) ? null : Convert.ToDateTime(x.dt_kaihu_gbk),
                    DWMC = x.dwmc_gbk,
                    DWXZ = x.dwxz_gbk,
                    DWQJRQ = string.IsNullOrEmpty(x.dt_qjrq_gbk) ? null : Convert.ToDateTime(x.dt_qjrq_gbk),
                    DWFXR = null,
                    DWYB = x.yzbm_gbk,
                    JBRXM = x.dwlxr_gbk,
                    OPERID = null,
                    JBRZJLX = null,
                    JBRGDDHHM = x.dwdh_gbk,
                    DWSLRQ = null,
                    JBRSJHM = null,
                    JBRZJHM = null,
                    DWFRDBZJLX = null,
                    DWFRDBZJHM = null,
                    BASICACCTMC = x.s_jzd_name_gbk,
                    BASICACCTNO = x.s_jzd_zh_gbk,
                    BASICACCTBRCH = x.s_jzd_yh_name_gbk,
                    CITY_CENTNO = city_cent,
                }).ToList();

                SugarHelper.Instance().Add(models);
            }

            #endregion
        }

        /// <summary>
        /// 单位账户信息表结构初始化
        /// </summary>
        /// <returns></returns>
        public void CorporationAcctInfoInitStructure()
        {
            #region 表结构初始化
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };

            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "CUSTID", columnTypeAndLimit = "nvarchar2(120) UNIQUE", columnDesc = "客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "VARCHAR2(20) UNIQUE", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "JZNY", columnTypeAndLimit = "VARCHAR2(6)", columnDesc = "缴至年月" });
            v_TableInits.Add(new v_TableInit { columnName = "NEXTPAYMTH", columnTypeAndLimit = "DATE", columnDesc = "下次应缴日期" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCBL", columnTypeAndLimit = "NUMBER(4, 2)", columnDesc = "单位缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZGRS", columnTypeAndLimit = "NUMBER(10, 0)", columnDesc = "单位职工人数" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCRS", columnTypeAndLimit = "NUMBER(10, 0)", columnDesc = "单位缴存人数" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFCRS", columnTypeAndLimit = "NUMBER(10, 0)", columnDesc = "单位封存人数" });
            v_TableInits.Add(new v_TableInit { columnName = "FACTINCOME", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "工资总数" });
            v_TableInits.Add(new v_TableInit { columnName = "MONTHPAYTOTALAMT", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "月缴存总额" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZHZT", columnTypeAndLimit = "VARCHAR2(2)", columnDesc = "单位账户状态" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZHYE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "单位账户余额" });
            v_TableInits.Add(new v_TableInit { columnName = "REGHANDBAL", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "挂账户余额" });
            v_TableInits.Add(new v_TableInit { columnName = "DWXHRQ", columnTypeAndLimit = "DATE", columnDesc = "单位销户日期 " });
            v_TableInits.Add(new v_TableInit { columnName = "DWXHYY", columnTypeAndLimit = "nvarchar2(360)", columnDesc = "单位销户原因" });
            v_TableInits.Add(new v_TableInit { columnName = "FROMFLAG", columnTypeAndLimit = "VARCHAR2(2)", columnDesc = "补贴资金来源" });
            v_TableInits.Add(new v_TableInit { columnName = "OPENPERSALREADY", columnTypeAndLimit = "VARCHAR2(2)", columnDesc = "员工户已开立标" });
            v_TableInits.Add(new v_TableInit { columnName = "STYHMC", columnTypeAndLimit = "nvarchar2(360)", columnDesc = "受托银行名称" });
            v_TableInits.Add(new v_TableInit { columnName = "STYHDM", columnTypeAndLimit = "nvarchar2(30)", columnDesc = "受托银行代码" });
            v_TableInits.Add(new v_TableInit { columnName = "STYHZH", columnTypeAndLimit = "nvarchar2(30)", columnDesc = "受托银行账号" });
            v_TableInits.Add(new v_TableInit { columnName = "CALC_METHOD", columnTypeAndLimit = "VARCHAR2(2)", columnDesc = "计算方法" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "varchar2(360byte)", columnDesc = "城市网点编号" });

            this.TableInit("CORPORATION_ACCTINFO", v_TableInits, true, "单位账户信息表");
            #endregion
        }

        /// <summary>
        /// 单位账户信息表数据初始化
        /// </summary>
        public void CorporationAcctInfoInitData()
        {
            #region 表数据初始化
            var city_cent = _configuration.GetValue<string>("CityCent");
            using (var syBaseConn = SybaseConnector.Conn())
            {
                //获取原基本信息表
                var org_dwxinxiSql = @" select * from dwgjjxx order by dwzh asc ";
                List<SD_dwgjjxx> sD_dwgjjxxs = syBaseConn.Query<SD_dwgjjxx>(org_dwxinxiSql).ToList();
                var models = sD_dwgjjxxs.Select(x =>
                new D_CORPORATION_ACCTINFO
                {
                    CUSTID = Common.PaddingDwzh(Convert.ToInt32(x.dwzh_gbk), 8),
                    CITY_CENTNO = city_cent,
                    DWZH = x.dwzh_gbk,
                    DWZHZT = x.fcbj_gbk,
                    DWJCBL = Convert.ToDecimal(x.dwjj_bili_gbk),
                    STYHDM = x.i_yh_type_gbk,
                    STYHMC = "",
                    STYHZH = x.s_yh_dwzh_gbk,
                    DWFCRS = 0,
                    DWJCRS = 0,
                    DWZGRS = 0,
                    DWXHRQ = null,
                    DWXHYY = null,
                    DWZHYE = 0,
                    FACTINCOME = 0,
                    CALC_METHOD = x.i_jsff_gbk,
                    REGHANDBAL = 0,
                    MONTHPAYTOTALAMT = Convert.ToDecimal(x.yjje_gbk),
                    OPENPERSALREADY = "0",
                    FROMFLAG = x.i_btly_gbk,
                    NEXTPAYMTH = Convert.ToDateTime(x.xcrq_gbk),
                    JZNY = Convert.ToDateTime(x.xcrq_gbk).AddMonths(-1).ToString("yyyyMM")
                }).ToList();

                SugarHelper.Instance().Add(models);
            }
            #endregion
        }

        /// <summary>
        /// 枚举表结构初始化
        /// </summary>
        public void SysEnumInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "TYPEKEY", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "VAL", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "键" });
            v_TableInits.Add(new v_TableInit { columnName = "LABEL", columnTypeAndLimit = "nvarchar2(360)", columnDesc = "值" });
            v_TableInits.Add(new v_TableInit { columnName = "REMARK", columnTypeAndLimit = "nvarchar2(360)", columnDesc = "备注" });
            v_TableInits.Add(new v_TableInit { columnName = "PARENTID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "父类ID" });
            v_TableInits.Add(new v_TableInit { columnName = "SORT", columnTypeAndLimit = "NUMBER(10)", columnDesc = "排序" });
            v_TableInits.Add(new v_TableInit { columnName = "DESCRIPTION", columnTypeAndLimit = "nvarchar2(360)", columnDesc = "描述" });
            v_TableInits.Add(new v_TableInit { columnName = "ORIGIN_FLAG", columnTypeAndLimit = "nvarchar2(20)", columnDesc = "原sybase i_flag 字段留存" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "varchar2(360byte)", columnDesc = "城市网点编号" });

            this.TableInit("SYS_ENUM", v_TableInits, true, "枚举类型表");
        }

        /// <summary>
        /// 枚举表数据初始化
        /// </summary>
        public void SysEnumInitData()
        {
            #region 表数据初始化
            var sugarHelper = SugarHelper.Instance();
            sugarHelper.ExecuteCommand("delete from SYS_ENUM");

            var city_cent = _configuration.GetValue<string>("CityCent");
            using (var syBaseConn = SybaseConnector.Conn())
            {
                //获取原基本信息表
                var org_journey_ConfigsSql = @" select * from journey_config order by i_flag asc ";
                List<SD_journey_config> journey_Configs = syBaseConn.Query<SD_journey_config>(org_journey_ConfigsSql).ToList();
                var listFirst = journey_Configs.GroupBy(x => x.i_flag_gbk, x => x.s_name_gbk).ToList();
                var modelsFather = new List<D_SYS_ENUM>();
                Action action = null;
                int sort = 0;
                foreach (var item in listFirst)
                {
                    var key = item.Key;
                    var name = item.FirstOrDefault();
                    var vlaue = name;
                    switch (name)
                    {
                        case "单位性质":
                            vlaue = "danweixingzhi";
                            break;
                        case "补贴资金来源":
                            vlaue = "butiezijinlaiyuan";
                            break;
                        case "银行":
                            vlaue = "bank";
                            break;
                    }

                    D_SYS_ENUM tModel = new D_SYS_ENUM { CITY_CENTNO = city_cent, DESCRIPTION = name, LABEL = name, SORT = ++sort, PARENTID = 0, ORIGIN_FLAG = key, VAL = vlaue };
                    action+=() => sugarHelper.AddReturnIdentity(tModel);
                }

                var tModelJsff = new D_SYS_ENUM { CITY_CENTNO = city_cent, DESCRIPTION = "计算方法", LABEL = "计算方法", SORT = ++sort, PARENTID = 0, ORIGIN_FLAG = "jsff", VAL = "jsff" };
                var tModelZjlx = new D_SYS_ENUM { CITY_CENTNO = city_cent, DESCRIPTION = "证件号码类型", LABEL = "证件号码类型", SORT = ++sort, PARENTID = 0, ORIGIN_FLAG = "zjhmlx", VAL = "zjhmlx" };

                action += () => sugarHelper.AddReturnIdentity(tModelJsff);
                action += () => sugarHelper.AddReturnIdentity(tModelZjlx);
                sugarHelper.InvokeTransactionScope(action);
                
                //计算方法相关
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "0", s_value_gbk = Common.GBKToCp850("舍入到分"), s_name_gbk = "", i_flag_gbk="jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "1", s_value_gbk = Common.GBKToCp850("见分进角"), s_name_gbk = "", i_flag_gbk="jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "2", s_value_gbk = Common.GBKToCp850("舍入到角"), s_name_gbk = "", i_flag_gbk="jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "3", s_value_gbk = Common.GBKToCp850("见角进元"), s_name_gbk = "", i_flag_gbk="jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "4", s_value_gbk = Common.GBKToCp850("舍入到元"), s_name_gbk = "", i_flag_gbk="jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "5", s_value_gbk = Common.GBKToCp850("见厘进分"), s_name_gbk = "", i_flag_gbk="jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "6", s_value_gbk = Common.GBKToCp850("四舍五入到分"), s_name_gbk = "", i_flag_gbk="jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "7", s_value_gbk = Common.GBKToCp850("四舍五入到角"), s_name_gbk = "", i_flag_gbk="jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "8", s_value_gbk = Common.GBKToCp850("四舍五入到元"), s_name_gbk = "", i_flag_gbk="jsff" });

                journey_Configs.Add(new SD_journey_config { i_value_gbk = "01", s_value_gbk = Common.GBKToCp850("身份证"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "02", s_value_gbk = Common.GBKToCp850("军官证"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "03", s_value_gbk = Common.GBKToCp850("护照"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "04", s_value_gbk = Common.GBKToCp850("外国人永居居留证"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "05", s_value_gbk = Common.GBKToCp850("其他"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });


                action = null;
                var listFather = sugarHelper.QueryList<D_SYS_ENUM>();
                
                foreach (var item in listFather)
                {
                    sort = 0;
                    var list = journey_Configs.Where(x => x.i_flag_gbk == item.ORIGIN_FLAG).Select(x => new D_SYS_ENUM
                    {
                        PARENTID = item.ID,
                        LABEL = x.s_value_gbk,
                        ORIGIN_FLAG = x.i_flag_gbk,
                        VAL = x.i_value_gbk,
                        CITY_CENTNO = city_cent,
                        TYPEKEY = item.VAL,
                        SORT = ++sort
                    }).ToList();
                    action += () => sugarHelper.Add(list);
                }
                sugarHelper.InvokeTransactionScope(action);
            }
            #endregion
        }



        /// <summary>
        /// 表结构初始化语句
        /// </summary>
        /// <param name="tableName">需要创建的表名</param>
        /// <param name="v_TableInits">表字段对象列表</param>
        /// <param name="existTableThenDelete">已存在表是否删除</param>
        /// <param name="tableNameDesc">表描述</param>
        /// <returns></returns>
        public void TableInit(string tableName, List<v_TableInit> v_TableInits, bool existTableThenDelete = false, string tableNameDesc = "")
        {
            var oracleHelper = SugarHelper.Instance();

            var SEQUENCE_Fmt = $"{tableName}_SEQ";//序列名称

            var createTableSql = $@"  --创建表
            CREATE TABLE {tableName} ({{0}}) ";

            {
                //创建序列
                var sequenceSql = $@"
                        --判断序列是否存在-不存在就创建
                        declare
                            num number;
                        begin
                                    select count(*) into num from user_sequences where SEQUENCE_NAME = upper('{SEQUENCE_Fmt}');
                        if   num = 0   then
                               execute immediate 'create sequence {SEQUENCE_Fmt} minvalue 1 maxvalue 9999999999 start with 1 increment by 1 nocache order ';      
                        end if ;
                        end;";
                oracleHelper.ExecuteCommand(sequenceSql);

                if (existTableThenDelete)//如果表已存在就删除
                {
                    var existTableThenDeleteSql = $@"
                        --判断表是否存在-存在就删除
                        declare
                              num  number;
                        begin
                              select count(*) into num from user_tables where table_name = upper('{tableName}');
                        if   num = 1   then
                            execute immediate 'DROP TABLE {tableName}';
                        end if;
                        end; ";

                    oracleHelper.ExecuteCommand(existTableThenDeleteSql);

                    sequenceSql = $@"
                        --判断序列是否存在-存在就删除再建(因为表删除了所以序列也需要删除)
                        declare
                            num number;
                        begin
                                    select count(*) into num from user_sequences where SEQUENCE_NAME = upper('{SEQUENCE_Fmt}');
                        if   num = 1   then
                                execute immediate 'DROP SEQUENCE {SEQUENCE_Fmt} ';
                                execute immediate 'create sequence {SEQUENCE_Fmt} minvalue 1 maxvalue 9999999999 start with 1 increment by 1 nocache order ';
                        end if ;
                        end;";
                    oracleHelper.ExecuteCommand(sequenceSql);
                }

                StringBuilder sb_table = new StringBuilder();
                List<string> columnDesList = new List<string>();//列字段的描述

                for (int i = 0; i < v_TableInits.Count; i++)
                {
                    var item = v_TableInits[i];

                    if (i == v_TableInits.Count - 1)
                    {
                        sb_table.Append($" {item.columnName}  {item.columnTypeAndLimit} ");
                    }
                    else
                    {
                        sb_table.Append($" {item.columnName}  {item.columnTypeAndLimit} ,");
                    }

                    columnDesList.Add($" COMMENT ON COLUMN \"{tableName}\".\"{item.columnName}\" IS '{item.columnDesc}' ");
                }

                if (sb_table.Length > 0)
                {
                    var crTableSql = string.Format(createTableSql, sb_table.ToString());
                    oracleHelper.ExecuteCommand(crTableSql);//创建表

                    if (!string.IsNullOrEmpty(tableNameDesc))
                    {
                        oracleHelper.ExecuteCommand($" COMMENT ON TABLE \"{tableName}\" IS '{tableNameDesc}' ");//添加表描述
                    }
                }

                foreach (var item in columnDesList)//创建列字段注释
                {
                    oracleHelper.ExecuteCommand(item);
                }
            }
        }
    }
}


