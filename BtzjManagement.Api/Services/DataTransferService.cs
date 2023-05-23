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
        #region 系统配置相关

        #region 枚举表相关
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
        public void SysEnumInitData(string city_cent)
        {
            #region 表数据初始化
            var sugarHelper = SugarHelper.Instance();
            sugarHelper.ExecuteCommand("delete from SYS_ENUM");

            //var city_cent = _configuration.GetValue<string>("CityCent");
            using (var syBaseConn = SybaseConnector.Conn())
            {
                //获取枚举信息表
                var org_journey_ConfigsSql = @" select * from journey_config order by i_flag asc ";
                List<SD_journey_config> journey_Configs = syBaseConn.Query<SD_journey_config>(org_journey_ConfigsSql).ToList();
                var listFirst = journey_Configs.GroupBy(x => x.i_flag_gbk, x => x.s_name_gbk).ToList();
                var modelsFather = new List<D_SYS_ENUM>();
                Action action = null;
                int sortFa = 0;
                int sortSon = 0;
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

                    D_SYS_ENUM tModel = new D_SYS_ENUM { CITY_CENTNO = city_cent, DESCRIPTION = name, LABEL = name, SORT = ++sortFa, PARENTID = 0, ORIGIN_FLAG = key, VAL = vlaue };
                    action += () => sugarHelper.AddReturnIdentity(tModel);
                }

                var tModelJsff = new D_SYS_ENUM { CITY_CENTNO = city_cent, DESCRIPTION = "计算方法", LABEL = "计算方法", SORT = ++sortFa, PARENTID = 0, ORIGIN_FLAG = "jsff", VAL = "jsff" };
                var tModelZjlx = new D_SYS_ENUM { CITY_CENTNO = city_cent, DESCRIPTION = "证件号码类型", LABEL = "证件号码类型", SORT = ++sortFa, PARENTID = 0, ORIGIN_FLAG = "zjhmlx", VAL = "zjhmlx" };

                action += () => sugarHelper.AddReturnIdentity(tModelJsff);
                action += () => sugarHelper.AddReturnIdentity(tModelZjlx);
                sugarHelper.InvokeTransactionScope(action);

                //计算方法相关
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "0", s_value_gbk = Common.GBKToCp850("舍入到分"), s_name_gbk = "", i_flag_gbk = "jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "1", s_value_gbk = Common.GBKToCp850("见分进角"), s_name_gbk = "", i_flag_gbk = "jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "2", s_value_gbk = Common.GBKToCp850("舍入到角"), s_name_gbk = "", i_flag_gbk = "jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "3", s_value_gbk = Common.GBKToCp850("见角进元"), s_name_gbk = "", i_flag_gbk = "jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "4", s_value_gbk = Common.GBKToCp850("舍入到元"), s_name_gbk = "", i_flag_gbk = "jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "5", s_value_gbk = Common.GBKToCp850("见厘进分"), s_name_gbk = "", i_flag_gbk = "jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "6", s_value_gbk = Common.GBKToCp850("四舍五入到分"), s_name_gbk = "", i_flag_gbk = "jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "7", s_value_gbk = Common.GBKToCp850("四舍五入到角"), s_name_gbk = "", i_flag_gbk = "jsff" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "8", s_value_gbk = Common.GBKToCp850("四舍五入到元"), s_name_gbk = "", i_flag_gbk = "jsff" });

                journey_Configs.Add(new SD_journey_config { i_value_gbk = "01", s_value_gbk = Common.GBKToCp850("身份证"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "02", s_value_gbk = Common.GBKToCp850("军官证"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "03", s_value_gbk = Common.GBKToCp850("护照"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "04", s_value_gbk = Common.GBKToCp850("外国人永居居留证"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                journey_Configs.Add(new SD_journey_config { i_value_gbk = "05", s_value_gbk = Common.GBKToCp850("其他"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });


                action = null;
                var listFather = sugarHelper.QueryList<D_SYS_ENUM>();

                foreach (var item in listFather)
                {
                    sortSon = 0;
                    var list = journey_Configs.Where(x => x.i_flag_gbk == item.ORIGIN_FLAG).Select(x => new D_SYS_ENUM
                    {
                        PARENTID = item.ID,
                        LABEL = x.s_value_gbk,
                        ORIGIN_FLAG = x.i_flag_gbk,
                        VAL = x.i_value_gbk,
                        CITY_CENTNO = city_cent,
                        TYPEKEY = item.VAL,
                        SORT = ++sortSon
                    }).ToList();
                    action += () => sugarHelper.Add(list);
                }
                sugarHelper.InvokeTransactionScope(action);

                #region 公积金业务操作名称
                var org_gjjop_configSql = @" select * from gjjop_config order by i_value asc ";
                List<SD_gjjop_config> gjjop_config = syBaseConn.Query<SD_gjjop_config>(org_gjjop_configSql).ToList();
                var tModelOpt = new D_SYS_ENUM { CITY_CENTNO = city_cent, DESCRIPTION = "公积金业务操作名称", LABEL = "公积金业务操作名称", SORT = ++sortFa, PARENTID = 0, ORIGIN_FLAG = "gjjopttype", VAL = "gjjopttype" };
                var faId = sugarHelper.AddReturnIdentity(tModelOpt);
                sortSon = 0;
                var listOpt = gjjop_config.Select(x => new D_SYS_ENUM
                {
                    CITY_CENTNO = city_cent,
                    DESCRIPTION = x.s_value_gbk,
                    LABEL = x.s_value_gbk,
                    ORIGIN_FLAG = x.i_flag_gbk,
                    PARENTID = faId,
                    SORT = ++sortSon,
                    TYPEKEY = tModelOpt.VAL,
                    VAL = x.i_value_gbk
                }).ToList();
                listOpt.Add(new D_SYS_ENUM
                {
                    CITY_CENTNO = city_cent,
                    DESCRIPTION = "单位开户",
                    LABEL = "单位开户",
                    PARENTID = faId,
                    SORT = ++sortSon,
                    TYPEKEY = tModelOpt.VAL,
                    VAL = "1000"
                });

                sugarHelper.Add(listOpt);
                #endregion
                //tModel = new D_SYS_ENUM { CITY_CENTNO = city_cent, DESCRIPTION = "公积金业务操作名称", LABEL = "公积金业务操作名称", SORT = 1, PARENTID = 0, ORIGIN_FLAG = key, VAL = vlaue };
            }
            #endregion
        }
        #endregion 枚举表相关
        internal void UserInfoInitData(string v)
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "NAME", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "用户名" });
            v_TableInits.Add(new v_TableInit { columnName = "REAL_NAME", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "真实姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "PASSWORD", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "密码" });
            v_TableInits.Add(new v_TableInit { columnName = "RULE_ID", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "权限ID" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_TIME", columnTypeAndLimit = "date", columnDesc = "创建时间" });
            v_TableInits.Add(new v_TableInit { columnName = "REMARK", columnTypeAndLimit = "nvarchar2(2000)", columnDesc = "备注" });

            this.TableInit("USER_INFO", v_TableInits, true, "用户信息表");
        }
        #region 菜单表相关
        /// <summary>
        /// 菜单表结构初始化
        /// </summary>
        public void SysMenuInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "NAME", columnTypeAndLimit = "VARCHAR2(50 BYTE) NOT NULL", columnDesc = "菜单名称" });
            v_TableInits.Add(new v_TableInit { columnName = "ALIAS", columnTypeAndLimit = "varchar2(20)", columnDesc = "菜单名称别名" });
            v_TableInits.Add(new v_TableInit { columnName = "PID", columnTypeAndLimit = "NUMBER(11, 0) NOT NULL", columnDesc = "分类节点id" });
            v_TableInits.Add(new v_TableInit { columnName = "PATH", columnTypeAndLimit = "VARCHAR2(100 BYTE)", columnDesc = "路由" });
            v_TableInits.Add(new v_TableInit { columnName = "ICON", columnTypeAndLimit = "VARCHAR2(50 BYTE)", columnDesc = "菜单图标" });
            v_TableInits.Add(new v_TableInit { columnName = "SORTID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "排序" });
            v_TableInits.Add(new v_TableInit { columnName = "ISENABLE", columnTypeAndLimit = "NUMBER(5,0)", columnDesc = "是否可用 0:不可用，1:可用" });
            v_TableInits.Add(new v_TableInit { columnName = "REMARK", columnTypeAndLimit = "VARCHAR2(200 BYTE)", columnDesc = "备注" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "varchar2(360byte)", columnDesc = "城市网点编号" });

            this.TableInit("SYS_MENU", v_TableInits, true, "菜单信息表");
        }

        /// <summary>
        /// 菜单信息表数据初始化
        /// </summary>
        public void SysMenuInitData(string city_cent)
        {
            var sqlHelper = SugarHelper.Instance();
            var faModel = new D_SysMenu { Name = "首页", IsEnable = 1, PId = 0, Remark = "首页", Path = "homepage.html", Alias = "Home", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            var faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);

            faModel = new D_SysMenu { Name = "单位管理", IsEnable = 1, PId = 0, Remark = "单位管理", Path = "", Alias = "CorporationManager", SortId = 20, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            List<D_SysMenu> sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "单位开户", IsEnable = 1, PId = faId, Remark = "单位开户", Path = "unitOpen.html", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "信息变更", IsEnable = 1, PId = faId, Remark = "信息变更", Path = "", Alias = "", SortId = 20, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "比例调整", IsEnable = 1, PId = faId, Remark = "比例调整", Path = "", Alias = "", SortId = 30, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "状态调整", IsEnable = 1, PId = faId, Remark = "状态调整", Path = "", Alias = "", SortId = 40, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "客户管理", IsEnable = 1, PId = 0, Remark = "客户管理", Path = "", Alias = "PersonInfoManager", SortId = 30, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "个人开户", IsEnable = 1, PId = faId, Remark = "个人开户", Path = "peopleOpen.html", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "个人一次性开户", IsEnable = 1, PId = faId, Remark = "个人一次性开户", Path = "", Alias = "", SortId = 20, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "个人信息变更", IsEnable = 1, PId = faId, Remark = "个人信息变更", Path = "", Alias = "", SortId = 30, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "缴存变更", IsEnable = 1, PId = 0, Remark = "缴存变更", Path = "", Alias = "JcbgManager", SortId = 40, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "缴存变更", IsEnable = 1, PId = faId, Remark = "缴存变更", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "转移", IsEnable = 1, PId = 0, Remark = "转移", Path = "", Alias = "TransferManager", SortId = 50, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "内部转移", IsEnable = 1, PId = faId, Remark = "内部转移", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "个人转出", IsEnable = 1, PId = faId, Remark = "个人转出", Path = "", Alias = "", SortId = 20, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "异地转入", IsEnable = 1, PId = faId, Remark = "异地转入", Path = "", Alias = "", SortId = 30, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "缴存核定", IsEnable = 1, PId = 0, Remark = "缴存核定", Path = "", Alias = "PaymentManager", SortId = 60, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "按月汇缴核定", IsEnable = 1, PId = faId, Remark = "按月汇缴核定", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "一次性汇缴", IsEnable = 1, PId = faId, Remark = "一次性汇缴", Path = "", Alias = "", SortId = 20, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "补缴", IsEnable = 1, PId = faId, Remark = "补缴", Path = "", Alias = "", SortId = 30, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "资金匹配", IsEnable = 1, PId = 0, Remark = "资金匹配", Path = "", Alias = "MatchManager", SortId = 70, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "资金进账录入", IsEnable = 1, PId = faId, Remark = "资金进账录入", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "资金进账匹配", IsEnable = 1, PId = faId, Remark = "资金进账匹配", Path = "", Alias = "", SortId = 20, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "记账", IsEnable = 1, PId = 0, Remark = "记账", Path = "", Alias = "BookManager", SortId = 80, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "单笔入账", IsEnable = 1, PId = faId, Remark = "单笔入账", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SysMenu { Name = "批量入账", IsEnable = 1, PId = faId, Remark = "批量入账", Path = "", Alias = "", SortId = 20, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "数据统计", IsEnable = 1, PId = 0, Remark = "数据统计", Path = "", Alias = "CountManager", SortId = 90, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "数据对比", IsEnable = 1, PId = faId, Remark = "数据对比", Path = "datastatistics.html", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "菜单管理", IsEnable = 1, PId = 0, Remark = "菜单管理", Path = "", Alias = "Menu", SortId = 100, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "菜单管理", IsEnable = 1, PId = faId, Remark = "菜单管理", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "用户管理", IsEnable = 1, PId = 0, Remark = "用户管理", Path = "", Alias = "User", SortId = 110, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "用户管理", IsEnable = 1, PId = faId, Remark = "用户管理", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "角色管理", IsEnable = 1, PId = 0, Remark = "角色管理", Path = "", Alias = "Role", SortId = 120, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "角色管理", IsEnable = 1, PId = faId, Remark = "角色管理", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "权限管理", IsEnable = 1, PId = 0, Remark = "权限管理", Path = "", Alias = "Permission", SortId = 130, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "权限管理", IsEnable = 1, PId = faId, Remark = "权限管理", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SysMenu { Name = "系统设置", IsEnable = 1, PId = 0, Remark = "系统设置", Path = "", Alias = "System", SortId = 140, Icon = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SysMenu>(faModel);
            sonList = new List<D_SysMenu> { };
            sonList.Add(new D_SysMenu { Name = "系统设置", IsEnable = 1, PId = faId, Remark = "系统设置", Path = "", Alias = "", SortId = 10, Icon = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);
        }

        #endregion 菜单表相关

        #region 影像表相关
        /// <summary>
        /// 影像类型表结构初始化
        /// </summary>
        public void ImageMenuInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "PID", columnTypeAndLimit = "NUMBER(10)  not null", columnDesc = "父级ID" });
            v_TableInits.Add(new v_TableInit { columnName = "MENUTYPE", columnTypeAndLimit = "NUMBER(10) not null", columnDesc = "影像类型-一级类型来自sys_enum表typekey为gjjopttype" });
            v_TableInits.Add(new v_TableInit { columnName = "MENUNAME", columnTypeAndLimit = "nvarchar2(400)", columnDesc = "影像类型名称" });
            v_TableInits.Add(new v_TableInit { columnName = "SORTID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "排序" });
            v_TableInits.Add(new v_TableInit { columnName = "ENABLED", columnTypeAndLimit = "NUMBER(10)", columnDesc = "是否启用(即前端能否获取到该节点) 0:不使用  1：使用" });
            v_TableInits.Add(new v_TableInit { columnName = "REQUIRED", columnTypeAndLimit = "NUMBER(10)", columnDesc = "是否必传(ENABLED为1时该字段才生效) 0:不是必传，1:是必传" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "varchar2(360byte)", columnDesc = "城市网点编号" });

            this.TableInit("IMAGE_MENU", v_TableInits, true, "影像类型表");
        }

        /// <summary>
        /// 影像数据表结构初始化
        /// </summary>
        public void ImageDataInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "MENUID", columnTypeAndLimit = "NUMBER(10)  not null", columnDesc = "影像类型id" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "varchar2(255) not null", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "PATH", columnTypeAndLimit = "varchar2(1000)", columnDesc = "影像路径" });
            this.TableInit("IMAGE_DATA", v_TableInits, true, "影像数据表");
        }
        #endregion 影像表相关

        #region 业务流程表
        /// <summary>
        /// 操作流程日志表结构初始化
        /// </summary>
        public void FlowProcInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "varchar2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "YWID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "业务id" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "varchar2(255)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "PROC_NAME", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "流程名称-来自sys_enum表typekey为gjjopttype" });
            v_TableInits.Add(new v_TableInit { columnName = "EXCEC_TIME", columnTypeAndLimit = "DATE", columnDesc = "执行时间" });
            v_TableInits.Add(new v_TableInit { columnName = "EXCEC_MAN", columnTypeAndLimit = "varchar2(255)", columnDesc = "执行人" });
            v_TableInits.Add(new v_TableInit { columnName = "STATUS", columnTypeAndLimit = "varchar2(255)", columnDesc = "状态-来自OptStatusConst" });
            v_TableInits.Add(new v_TableInit { columnName = "MEMO", columnTypeAndLimit = "nvarchar2(1000)", columnDesc = "备注" });

            this.TableInit("FLOWPROC", v_TableInits, true, "操作流程日志表");
        }
        #endregion 业务流程表

        #endregion 菜单表相关

        #region  单位管理相关
        /// <summary>
        /// 单位业务信息主表结构初始化
        /// </summary>
        public void BusiCorporationInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "varchar2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "USCCID", columnTypeAndLimit = "varchar2(255)", columnDesc = "统一社会信用代码" });
            v_TableInits.Add(new v_TableInit { columnName = "BUSITYPE", columnTypeAndLimit = "NUMBER(10)", columnDesc = "业务类型" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_TIME", columnTypeAndLimit = "date", columnDesc = "保存时间" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_MAN", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "保存人" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_TIME", columnTypeAndLimit = "date", columnDesc = "提交时间" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_MAN", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "提交人" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_TIME", columnTypeAndLimit = "date", columnDesc = "审核时间" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_MAN", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "审核人" });
            v_TableInits.Add(new v_TableInit { columnName = "STATUS", columnTypeAndLimit = "varchar2(255)", columnDesc = "业务状态" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "varchar2(360byte)", columnDesc = "城市网点编号" });
            v_TableInits.Add(new v_TableInit { columnName = "MEMO", columnTypeAndLimit = "nvarchar2(1000)", columnDesc = "备注" });

            this.TableInit("BUSI_CORPORATION", v_TableInits, true, "单位业务信息主表");
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
        public void CorporationBasicInfoInitData(string city_cent)
        {
            #region 表数据初始化
            //var city_cent = _configuration.GetValue<string>("CityCent");
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
        public void CorporationAcctInfoInitData(string city_cent)
        {
            #region 表数据初始化
            //var city_cent = _configuration.GetValue<string>("CityCent");
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
        #endregion

        #region 客户管理相关
        /// <summary>
        /// 个人开户业务表结构初始化
        /// </summary>
        public void GrkhInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "varchar2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "varchar2(20)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWMC", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "单位名称" });
            v_TableInits.Add(new v_TableInit { columnName = "KHTYPE", columnTypeAndLimit = "NUMBER(10)", columnDesc = "开户类型(按月1，一次性2)" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_TIME", columnTypeAndLimit = "date", columnDesc = "保存时间" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_MAN", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "保存人" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_TIME", columnTypeAndLimit = "date", columnDesc = "提交时间" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_MAN", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "提交人" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_TIME", columnTypeAndLimit = "date", columnDesc = "审核时间" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_MAN", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "审核人" });
            v_TableInits.Add(new v_TableInit { columnName = "STATUS", columnTypeAndLimit = "varchar2(30)", columnDesc = "业务状态" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "varchar2(360byte)", columnDesc = "城市网点编号" });
            v_TableInits.Add(new v_TableInit { columnName = "MEMO", columnTypeAndLimit = "nvarchar2(1000)", columnDesc = "备注" });

            this.TableInit("GRKH", v_TableInits, true, "个人开户业务主表");
        }

        /// <summary>
        /// 个人开户明细表结构初始化
        /// </summary>
        public void Grkh_ItemInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "varchar2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "varchar2(20)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "YWYD", columnTypeAndLimit = "varchar2(10)", columnDesc = "业务月度" });
            v_TableInits.Add(new v_TableInit { columnName = "XINGMING", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "ZJLX", columnTypeAndLimit = "varchar2(4byte)", columnDesc = "证件类型" });
            v_TableInits.Add(new v_TableInit { columnName = "ZJHM", columnTypeAndLimit = "varchar2(18byte)", columnDesc = "证件号码" });
            v_TableInits.Add(new v_TableInit { columnName = "XINGBIE", columnTypeAndLimit = "varchar2(2)", columnDesc = "性别" });
            v_TableInits.Add(new v_TableInit { columnName = "CSNY", columnTypeAndLimit = "date", columnDesc = "出生日期" });
            v_TableInits.Add(new v_TableInit { columnName = "SJHM", columnTypeAndLimit = "varchar2(20byte)", columnDesc = "手机号码" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCBL", columnTypeAndLimit = "NUMBER(4, 2)", columnDesc = "单位缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCJS", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人缴存基数" });
            v_TableInits.Add(new v_TableInit { columnName = "GRYJCE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人月缴存额" });
            v_TableInits.Add(new v_TableInit { columnName = "QJRQ", columnTypeAndLimit = "date", columnDesc = "起缴日期" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZHZT", columnTypeAndLimit = "VARCHAR2(2)", columnDesc = "个人账户状态" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHHM", columnTypeAndLimit = "varchar2(30)", columnDesc = "个人存款账户号码" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHKHMC", columnTypeAndLimit = "nvarchar2(255)", columnDesc = "个人存款账户开户名称" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHKHYHDM", columnTypeAndLimit = "varchar2(30)", columnDesc = "个人存款账户开户银行代码" });

            this.TableInit("GRKH_ITEM", v_TableInits, true, "个人开户业务明细表");
        }

        #endregion 客户管理相关





        #region 私有方法
        /// <summary>
        /// 表结构初始化语句
        /// </summary>
        /// <param name="tableName">需要创建的表名</param>
        /// <param name="v_TableInits">表字段对象列表</param>
        /// <param name="existTableThenDelete">已存在表是否删除</param>
        /// <param name="tableNameDesc">表描述</param>
        /// <returns></returns>
        private void TableInit(string tableName, List<v_TableInit> v_TableInits, bool existTableThenDelete = false, string tableNameDesc = "")
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
        #endregion 私有方法
    }
}


