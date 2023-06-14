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
using BtzjManagement.Api.Enum;

namespace BtzjManagement.Api.Services
{
    public class DataTransferService
    {
        #region 系统配置相关
        #region 系统用户，角色，权限，字典相关
        #region 用户相关
        /// <summary>
        /// 用户信息表结构初始化
        /// </summary>
        /// <param name="v"></param>
        internal void UserInfoInitData(string v)
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "NAME", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "用户名" });
            v_TableInits.Add(new v_TableInit { columnName = "REAL_NAME", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "真实姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "PASSWORD", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "密码" });
            v_TableInits.Add(new v_TableInit { columnName = "SALT", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "盐" });
            v_TableInits.Add(new v_TableInit { columnName = "ROLE_ID", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "权限ID" });
            v_TableInits.Add(new v_TableInit { columnName = "LAST_LOGIN_IP", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "最后登录IP" });
            v_TableInits.Add(new v_TableInit { columnName = "LAST_LOGIN_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "最后登录时间" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "创建时间" });
            v_TableInits.Add(new v_TableInit { columnName = "REMARK", columnTypeAndLimit = "NVARCHAR2(2000)", columnDesc = "备注" });

            this.TableInit("USER_INFO", v_TableInits, true, "用户信息表");
        }
        #endregion

        #region 角色相关
        /// <summary>
        /// 角色表结构初始化
        /// </summary>
        /// <param name="v"></param>
        internal void SysRoleInitData(string v)
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "ROLE_NAME", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "角色名称" });
            v_TableInits.Add(new v_TableInit { columnName = "SORT_ID", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "排序号" });
            v_TableInits.Add(new v_TableInit { columnName = "REMARK", columnTypeAndLimit = "NVARCHAR2(2000)", columnDesc = "备注" });

            this.TableInit("SYS_ROLE", v_TableInits, true, "角色表");
        }
        #endregion

        #region 权限相关
        /// <summary>
        /// 角色菜单表数据初始化
        /// </summary>
        /// <param name="v"></param>
        internal void SysRoleMenuInitData(string v)
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "ROLE_ID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "角色id" });
            v_TableInits.Add(new v_TableInit { columnName = "MENU_ID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "菜单id" });
            v_TableInits.Add(new v_TableInit { columnName = "CAN_ADD", columnTypeAndLimit = "NUMBER(10)", columnDesc = "是否有添加权限" });
            v_TableInits.Add(new v_TableInit { columnName = "CAN_EDIT", columnTypeAndLimit = "NUMBER(10)", columnDesc = "是否有编辑权限" });
            v_TableInits.Add(new v_TableInit { columnName = "CAN_DELETE", columnTypeAndLimit = "NUMBER(10)", columnDesc = "是否有删除权限" });
            v_TableInits.Add(new v_TableInit { columnName = "CAN_AUDIT", columnTypeAndLimit = "NUMBER(10)", columnDesc = "是否有查看权限" });

            this.TableInit("SYS_ROLE_MENU", v_TableInits, true, "角色菜单表");
        }
        #endregion

        #region 字典相关
        /// <summary>
        /// 数据字典表结构初始化
        /// </summary>
        internal void SysDataDictionaryInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "TYPE_KEY", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "VAL", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "键" });
            v_TableInits.Add(new v_TableInit { columnName = "LABEL", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "值" });
            v_TableInits.Add(new v_TableInit { columnName = "REMARK", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "备注" });
            v_TableInits.Add(new v_TableInit { columnName = "PARENT_ID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "父类ID" });
            v_TableInits.Add(new v_TableInit { columnName = "SORT_ID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "排序" });
            v_TableInits.Add(new v_TableInit { columnName = "DESCRIPTION", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "描述" });
            v_TableInits.Add(new v_TableInit { columnName = "ORIGIN_FLAG", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "原sybase i_flag 字段留存" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "城市网点编号" });

            this.TableInit("SYS_DATA_DICTIONARY", v_TableInits, true, "数据字典表");
        }

        /// <summary>
        /// 数据字典表数据初始化
        /// </summary>
        internal void SysDataDictionaryInitData(string city_cent)
        {
            #region 表数据初始化
            var sugarHelper = SugarHelper.Instance();
            sugarHelper.ExecuteCommand("delete from SYS_DATA_DICTIONARY");

            //var city_cent = _configuration.GetValue<string>("CityCent");
            using (var syBaseConn = SybaseConnector.Conn())
            {
                //获取枚举信息表
                var org_journey_ConfigsSql = @" select * from journey_config order by i_flag asc ";
                List<SD_journey_config> journey_Configs = syBaseConn.Query<SD_journey_config>(org_journey_ConfigsSql).ToList();
                var listFirst = journey_Configs.GroupBy(x => x.i_flag_gbk, x => x.s_name_gbk).ToList();
                var modelsFather = new List<D_SYS_DATA_DICTIONARY>();
                Action action = null;
                int sortFa = 0;
                int sortSon = 0;
                foreach (var item in listFirst)
                {
                    var key = item.Key;
                    var name = item.FirstOrDefault();
                    var value = name;
                    switch (name)
                    {
                        case "单位性质":
                            value = "danweixingzhi";
                            break;
                        case "补贴资金来源":
                            value = "butiezijinlaiyuan";
                            break;
                        case "银行":
                            value = "bank";
                            break;
                        case "性别":
                            value = "xingbie";
                            journey_Configs.RemoveAll(x => x.i_flag_gbk == item.Key);
                            //journey_Configs.Add(new SD_journey_config { i_value_gbk = "0", s_value_gbk = Common.GBKToCp850("未知的性别"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "2", s_value_gbk = Common.GBKToCp850("女性"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "1", s_value_gbk = Common.GBKToCp850("男性"), s_name_gbk = "", i_flag_gbk = key });
                            //journey_Configs.Add(new SD_journey_config { i_value_gbk = "9", s_value_gbk = Common.GBKToCp850("未说明的性别"), s_name_gbk = "", i_flag_gbk = key });
                            break;
                        case "封存状态":
                            value = "lockreason";
                            break;
                        case "封存标记":
                            value = "grzhzt";
                            journey_Configs.RemoveAll(x => x.i_flag_gbk == item.Key);
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "01", s_value_gbk = Common.GBKToCp850("正常"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "02", s_value_gbk = Common.GBKToCp850("封存"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "03", s_value_gbk = Common.GBKToCp850("合并销户"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "04", s_value_gbk = Common.GBKToCp850("外部转出销户"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "05", s_value_gbk = Common.GBKToCp850("提取销户"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "06", s_value_gbk = Common.GBKToCp850("冻结"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "99", s_value_gbk = Common.GBKToCp850("其他"), s_name_gbk = "", i_flag_gbk = key });

                            break;
                        case "单位状态":
                            value = "dwzhzt";
                            journey_Configs.RemoveAll(x => x.i_flag_gbk == item.Key);
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "01", s_value_gbk = Common.GBKToCp850("正常"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "02", s_value_gbk = Common.GBKToCp850("开户"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "03", s_value_gbk = Common.GBKToCp850("缓缴"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "04", s_value_gbk = Common.GBKToCp850("销户"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "05", s_value_gbk = Common.GBKToCp850("封存"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "99", s_value_gbk = Common.GBKToCp850("其他"), s_name_gbk = "", i_flag_gbk = key });
                            break;
                        case "婚姻状态":
                            value = "hyzk";
                            journey_Configs.RemoveAll(x => x.i_flag_gbk == item.Key);
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "10", s_value_gbk = Common.GBKToCp850("未婚"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "20", s_value_gbk = Common.GBKToCp850("已婚"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "30", s_value_gbk = Common.GBKToCp850("丧偶"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "40", s_value_gbk = Common.GBKToCp850("离婚"), s_name_gbk = "", i_flag_gbk = key });
                            journey_Configs.Add(new SD_journey_config { i_value_gbk = "90", s_value_gbk = Common.GBKToCp850("未说明的婚姻状况"), s_name_gbk = "", i_flag_gbk = key });
                            break;
                        case "住房补贴方式":
                            value = "khtype";
                            break;
                        case "支取原因":
                            value = "drawreason";
                            break;
                        case "销户原因":
                            value = "xiaohureason";
                            break;
                    }

                    D_SYS_DATA_DICTIONARY tModel = new D_SYS_DATA_DICTIONARY
                    {
                        CITY_CENTNO = city_cent,
                        DESCRIPTION = name,
                        LABEL = name,
                        SORT_ID = ++sortFa,
                        PARENT_ID = 0,
                        ORIGIN_FLAG = key,
                        VAL = "",
                        TYPE_KEY = value
                    };
                    action += () => sugarHelper.AddReturnIdentity(tModel);
                }

                #region 计算方法，身份证类型
                var tModelJsff = new D_SYS_DATA_DICTIONARY { CITY_CENTNO = city_cent, DESCRIPTION = "计算方法", LABEL = "计算方法", SORT_ID = ++sortFa, PARENT_ID = 0, ORIGIN_FLAG = "jsff", VAL = "", TYPE_KEY = "jsff" };
                var tModelZjlx = new D_SYS_DATA_DICTIONARY { CITY_CENTNO = city_cent, DESCRIPTION = "证件号码类型", LABEL = "证件号码类型", SORT_ID = ++sortFa, PARENT_ID = 0, ORIGIN_FLAG = "zjhmlx", VAL = "", TYPE_KEY = "zjhmlx" };

                action += () => sugarHelper.AddReturnIdentity(tModelJsff);
                action += () => sugarHelper.AddReturnIdentity(tModelZjlx);
                sugarHelper.InvokeTransactionScope(action);


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
                //journey_Configs.Add(new SD_journey_config { i_value_gbk = "02", s_value_gbk = Common.GBKToCp850("军官证"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                //journey_Configs.Add(new SD_journey_config { i_value_gbk = "03", s_value_gbk = Common.GBKToCp850("护照"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                //journey_Configs.Add(new SD_journey_config { i_value_gbk = "04", s_value_gbk = Common.GBKToCp850("外国人永居居留证"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                //journey_Configs.Add(new SD_journey_config { i_value_gbk = "05", s_value_gbk = Common.GBKToCp850("其他"), s_name_gbk = "", i_flag_gbk = "zjhmlx" });
                #endregion

                action = null;
                var listFather = sugarHelper.QueryList<D_SYS_DATA_DICTIONARY>();//一级节点

                foreach (var item in listFather)
                {
                    sortSon = 0;
                    var list = journey_Configs.Where(x => x.i_flag_gbk == item.ORIGIN_FLAG).Select(x => new D_SYS_DATA_DICTIONARY //二级节点
                    {
                        PARENT_ID = item.ID,
                        LABEL = x.s_value_gbk,
                        ORIGIN_FLAG = x.i_flag_gbk,
                        VAL = x.i_value_gbk,
                        CITY_CENTNO = city_cent,
                        TYPE_KEY = "",
                        SORT_ID = ++sortSon
                    }).ToList();
                    action += () => sugarHelper.Add(list);
                }
                sugarHelper.InvokeTransactionScope(action);

                #region 公积金业务操作名称
                var org_gjjop_configSql = @" select * from gjjop_config order by i_value asc ";
                List<SD_gjjop_config> gjjop_config = syBaseConn.Query<SD_gjjop_config>(org_gjjop_configSql).ToList();
                var tModelOpt = new D_SYS_DATA_DICTIONARY { CITY_CENTNO = city_cent, DESCRIPTION = "公积金业务操作名称", LABEL = "公积金业务操作名称", SORT_ID = ++sortFa, PARENT_ID = 0, ORIGIN_FLAG = "gjjopttype", VAL = "", TYPE_KEY = "gjjopttype" };
                var faId = sugarHelper.AddReturnIdentity(tModelOpt);
                sortSon = 0;
                var listOpt = gjjop_config.Select(x => new D_SYS_DATA_DICTIONARY
                {
                    CITY_CENTNO = city_cent,
                    DESCRIPTION = x.s_value_gbk,
                    LABEL = x.s_value_gbk,
                    ORIGIN_FLAG = x.i_flag_gbk,
                    PARENT_ID = faId,
                    SORT_ID = ++sortSon,
                    TYPE_KEY = "",
                    VAL = x.i_value_gbk
                }).ToList();
                listOpt.Add(new D_SYS_DATA_DICTIONARY
                {
                    CITY_CENTNO = city_cent,
                    DESCRIPTION = "单位开户",
                    LABEL = "单位开户",
                    PARENT_ID = faId,
                    SORT_ID = ++sortSon,
                    TYPE_KEY = "",
                    VAL = "1000"
                });
                listOpt.Add(new D_SYS_DATA_DICTIONARY
                {
                    CITY_CENTNO = city_cent,
                    DESCRIPTION = "个人开户",
                    LABEL = "个人开户",
                    PARENT_ID = faId,
                    SORT_ID = ++sortSon,
                    TYPE_KEY = "",
                    VAL = "1001"
                });

                sugarHelper.Add(listOpt);
                #endregion

                //相关限制配置参数
                {
                    sortSon = 0;
                    var tModelLimit = new D_SYS_DATA_DICTIONARY { CITY_CENTNO = city_cent, DESCRIPTION = "公积金业务限制配置", LABEL = "公积金业务限制配置", SORT_ID = ++sortFa, PARENT_ID = 0, ORIGIN_FLAG = "gjjlimitconfig", VAL = "", TYPE_KEY = "gjjlimitconfig" };
                    faId = sugarHelper.AddReturnIdentity(tModelLimit);
                    List<D_SYS_DATA_DICTIONARY> listLimit = new List<D_SYS_DATA_DICTIONARY>();
                    listLimit.Add(new D_SYS_DATA_DICTIONARY
                    {
                        CITY_CENTNO = city_cent,
                        DESCRIPTION = "单位开户统一信用代码长度限制",
                        LABEL = "18",
                        PARENT_ID = faId,
                        SORT_ID = ++sortSon,
                        TYPE_KEY = "",
                        VAL = "1",
                    });

                    sugarHelper.Add(listLimit);
                }
                //科目类型
                {
                    sortSon = 0;
                    var tModelLimit = new D_SYS_DATA_DICTIONARY { CITY_CENTNO = city_cent, DESCRIPTION = "", LABEL = "科目类型", SORT_ID = ++sortFa, PARENT_ID = 0, ORIGIN_FLAG = "", VAL = "", TYPE_KEY = "kmlx" };
                    faId = sugarHelper.AddReturnIdentity(tModelLimit);
                    List<D_SYS_DATA_DICTIONARY> listLimit = new List<D_SYS_DATA_DICTIONARY>();
                    listLimit.Add(new D_SYS_DATA_DICTIONARY
                    {
                        CITY_CENTNO = city_cent,
                        DESCRIPTION = "",
                        LABEL = "资产",
                        PARENT_ID = faId,
                        SORT_ID = ++sortSon,
                        TYPE_KEY = "",
                        VAL = "0",
                    });
                    listLimit.Add(new D_SYS_DATA_DICTIONARY
                    {
                        CITY_CENTNO = city_cent,
                        DESCRIPTION = "",
                        LABEL = "费用",
                        PARENT_ID = faId,
                        SORT_ID = ++sortSon,
                        TYPE_KEY = "",
                        VAL = "100",
                    });
                    listLimit.Add(new D_SYS_DATA_DICTIONARY
                    {
                        CITY_CENTNO = city_cent,
                        DESCRIPTION = "",
                        LABEL = "负债",
                        PARENT_ID = faId,
                        SORT_ID = ++sortSon,
                        TYPE_KEY = "",
                        VAL = "200",
                    });
                    listLimit.Add(new D_SYS_DATA_DICTIONARY
                    {
                        CITY_CENTNO = city_cent,
                        DESCRIPTION = "",
                        LABEL = "权益",
                        PARENT_ID = faId,
                        SORT_ID = ++sortSon,
                        TYPE_KEY = "",
                        VAL = "300",
                    });
                    listLimit.Add(new D_SYS_DATA_DICTIONARY
                    {
                        CITY_CENTNO = city_cent,
                        DESCRIPTION = "",
                        LABEL = "收入",
                        PARENT_ID = faId,
                        SORT_ID = ++sortSon,
                        TYPE_KEY = "",
                        VAL = "400",
                    });
                    listLimit.Add(new D_SYS_DATA_DICTIONARY
                    {
                        CITY_CENTNO = city_cent,
                        DESCRIPTION = "",
                        LABEL = "收支结余",
                        PARENT_ID = faId,
                        SORT_ID = ++sortSon,
                        TYPE_KEY = "",
                        VAL = "301",
                    });
                    sugarHelper.Add(listLimit);
                }

                //tModel = new D_SYS_DATA_DICTIONARY { CITY_CENTNO = city_cent, DESCRIPTION = "公积金业务操作名称", LABEL = "公积金业务操作名称", SORT_ID = 1, PARENT_ID = 0, ORIGIN_FLAG = key, VAL = vlaue };
            }
            #endregion
        }
        #endregion


        #endregion

        #region 菜单表相关
        /// <summary>
        /// 菜单表结构初始化
        /// </summary>
        public void SysMenuInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "NAME", columnTypeAndLimit = "NVARCHAR2(50) NOT NULL", columnDesc = "菜单名称" });
            v_TableInits.Add(new v_TableInit { columnName = "ALIAS", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "菜单名称别名" });
            v_TableInits.Add(new v_TableInit { columnName = "PID", columnTypeAndLimit = "NUMBER(11, 0) NOT NULL", columnDesc = "分类节点id" });
            v_TableInits.Add(new v_TableInit { columnName = "PATH", columnTypeAndLimit = "NVARCHAR2(100)", columnDesc = "路由" });
            v_TableInits.Add(new v_TableInit { columnName = "ICON", columnTypeAndLimit = "NVARCHAR2(50)", columnDesc = "菜单图标" });
            v_TableInits.Add(new v_TableInit { columnName = "SORT_ID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "排序" });
            v_TableInits.Add(new v_TableInit { columnName = "IS_ENABLE", columnTypeAndLimit = "NUMBER(5,0)", columnDesc = "是否可用 0:不可用，1:可用" });
            v_TableInits.Add(new v_TableInit { columnName = "REMARK", columnTypeAndLimit = "NVARCHAR2(200)", columnDesc = "备注" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "城市网点编号" });

            this.TableInit("SYS_MENU", v_TableInits, true, "菜单信息表");
        }

        /// <summary>
        /// 菜单信息表数据初始化
        /// </summary>
        public void SysMenuInitData(string city_cent)
        {
            var sqlHelper = SugarHelper.Instance();
            sqlHelper.ExecuteCommand(" delete from SYS_MENU ");

            var faModel = new D_SYS_MENU { NAME = "首页", IS_ENABLE = 1, PID = 0, REMARK = "首页", PATH = "homepage.html", ALIAS = "Home", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            var faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);

            faModel = new D_SYS_MENU { NAME = "单位管理", IS_ENABLE = 1, PID = 0, REMARK = "单位管理", PATH = "", ALIAS = "CorporationManager", SORT_ID = 20, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            List<D_SYS_MENU> sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "单位开户", IS_ENABLE = 1, PID = faId, REMARK = "单位开户", PATH = "unitOpen.html", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "信息变更", IS_ENABLE = 1, PID = faId, REMARK = "信息变更", PATH = "", ALIAS = "", SORT_ID = 20, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "比例调整", IS_ENABLE = 1, PID = faId, REMARK = "比例调整", PATH = "", ALIAS = "", SORT_ID = 30, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "状态调整", IS_ENABLE = 1, PID = faId, REMARK = "状态调整", PATH = "", ALIAS = "", SORT_ID = 40, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "客户管理", IS_ENABLE = 1, PID = 0, REMARK = "客户管理", PATH = "", ALIAS = "PersonInfoManager", SORT_ID = 30, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "个人开户", IS_ENABLE = 1, PID = faId, REMARK = "个人开户", PATH = "peopleOpen.html", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "个人一次性开户", IS_ENABLE = 1, PID = faId, REMARK = "个人一次性开户", PATH = "", ALIAS = "", SORT_ID = 20, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "个人信息变更", IS_ENABLE = 1, PID = faId, REMARK = "个人信息变更", PATH = "", ALIAS = "", SORT_ID = 30, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "缴存变更", IS_ENABLE = 1, PID = 0, REMARK = "缴存变更", PATH = "", ALIAS = "JcbgManager", SORT_ID = 40, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "缴存变更", IS_ENABLE = 1, PID = faId, REMARK = "缴存变更", PATH = "", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "转移", IS_ENABLE = 1, PID = 0, REMARK = "转移", PATH = "", ALIAS = "TransferManager", SORT_ID = 50, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "内部转移", IS_ENABLE = 1, PID = faId, REMARK = "内部转移", PATH = "", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "个人转出", IS_ENABLE = 1, PID = faId, REMARK = "个人转出", PATH = "", ALIAS = "", SORT_ID = 20, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "异地转入", IS_ENABLE = 1, PID = faId, REMARK = "异地转入", PATH = "", ALIAS = "", SORT_ID = 30, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "缴存核定", IS_ENABLE = 1, PID = 0, REMARK = "缴存核定", PATH = "", ALIAS = "PaymentManager", SORT_ID = 60, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "按月汇缴核定", IS_ENABLE = 1, PID = faId, REMARK = "按月汇缴核定", PATH = "", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "一次性汇缴", IS_ENABLE = 1, PID = faId, REMARK = "一次性汇缴", PATH = "", ALIAS = "", SORT_ID = 20, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "补缴", IS_ENABLE = 1, PID = faId, REMARK = "补缴", PATH = "", ALIAS = "", SORT_ID = 30, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "资金匹配", IS_ENABLE = 1, PID = 0, REMARK = "资金匹配", PATH = "", ALIAS = "MatchManager", SORT_ID = 70, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "资金进账录入", IS_ENABLE = 1, PID = faId, REMARK = "资金进账录入", PATH = "", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "资金进账匹配", IS_ENABLE = 1, PID = faId, REMARK = "资金进账匹配", PATH = "", ALIAS = "", SORT_ID = 20, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "记账", IS_ENABLE = 1, PID = 0, REMARK = "记账", PATH = "", ALIAS = "BookManager", SORT_ID = 80, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "单笔入账", IS_ENABLE = 1, PID = faId, REMARK = "单笔入账", PATH = "", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "批量入账", IS_ENABLE = 1, PID = faId, REMARK = "批量入账", PATH = "", ALIAS = "", SORT_ID = 20, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "系统管理", IS_ENABLE = 1, PID = 0, REMARK = "系统设置", PATH = "", ALIAS = "System", SORT_ID = 90, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "字典管理", IS_ENABLE = 1, PID = faId, REMARK = "字典管理", PATH = "systemData.html", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "菜单管理", IS_ENABLE = 1, PID = faId, REMARK = "菜单管理", PATH = "systemMenu.html", ALIAS = "", SORT_ID = 20, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "用户管理", IS_ENABLE = 1, PID = faId, REMARK = "用户管理", PATH = "systemUser.html", ALIAS = "", SORT_ID = 30, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "角色管理", IS_ENABLE = 1, PID = faId, REMARK = "角色管理", PATH = "systemRole.html", ALIAS = "", SORT_ID = 40, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sonList.Add(new D_SYS_MENU { NAME = "权限管理", IS_ENABLE = 1, PID = faId, REMARK = "权限管理", PATH = "", ALIAS = "", SORT_ID = 50, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "财务管理", IS_ENABLE = 1, PID = 0, REMARK = "财务管理", PATH = "", ALIAS = "Finance", SORT_ID = 90, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "科目管理", IS_ENABLE = 1, PID = faId, REMARK = "科目管理", PATH = "page/business/km.html", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);

            faModel = new D_SYS_MENU { NAME = "数据统计", IS_ENABLE = 1, PID = 0, REMARK = "数据统计", PATH = "", ALIAS = "CountManager", SORT_ID = 100, ICON = "el-icon-location", CITY_CENTNO = city_cent };
            faId = sqlHelper.AddReturnIdentity<D_SYS_MENU>(faModel);
            sonList = new List<D_SYS_MENU> { };
            sonList.Add(new D_SYS_MENU { NAME = "数据对比", IS_ENABLE = 1, PID = faId, REMARK = "数据对比", PATH = "datastatistics.html", ALIAS = "", SORT_ID = 10, ICON = "el-icon-location", CITY_CENTNO = city_cent });
            sqlHelper.Add(sonList);


            #region 处理菜单权限
            var list = sqlHelper.QueryWhereList<D_SYS_MENU>(x => x.CITY_CENTNO == city_cent);
            foreach (var item in list)
            {
                var modelRo = new D_SYS_ROLE_MENU
                {
                    CAN_ADD = 1,
                    CAN_AUDIT = 1,
                    CAN_DELETE = 1,
                    CAN_EDIT = 1,
                    MENU_ID = item.ID,
                    ROLE_ID = 1
                };
                sqlHelper.Add(modelRo);
            }
            #endregion
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
            v_TableInits.Add(new v_TableInit { columnName = "MENU_TYPE", columnTypeAndLimit = "NUMBER(10) not null", columnDesc = "影像类型-一级类型来自sys_enum表typekey为gjjopttype" });
            v_TableInits.Add(new v_TableInit { columnName = "MENU_NAME", columnTypeAndLimit = "NVARCHAR2(400)", columnDesc = "影像类型名称" });
            v_TableInits.Add(new v_TableInit { columnName = "SORT_ID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "排序" });
            v_TableInits.Add(new v_TableInit { columnName = "ENABLED", columnTypeAndLimit = "NUMBER(10)", columnDesc = "是否启用(即前端能否获取到该节点) 0:不使用  1：使用" });
            v_TableInits.Add(new v_TableInit { columnName = "REQUIRED", columnTypeAndLimit = "NUMBER(10)", columnDesc = "是否必传(ENABLED为1时该字段才生效) 0:不是必传，1:是必传" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "城市网点编号" });

            this.TableInit("IMAGE_MENU", v_TableInits, true, "影像类型表");
        }

        /// <summary>
        /// 影像数据表结构初始化
        /// </summary>
        public void ImageDataInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "MENU_ID", columnTypeAndLimit = "NUMBER(10)  not null", columnDesc = "影像类型id" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "NVARCHAR2(255) not null", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "PATH", columnTypeAndLimit = "NVARCHAR2(1000)", columnDesc = "影像路径" });
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
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "YWID", columnTypeAndLimit = "NUMBER(10)", columnDesc = "业务id" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "PROC_NAME", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "流程名称-来自SYS_DATA_DICTIONARY表type_key为gjjopttype" });
            v_TableInits.Add(new v_TableInit { columnName = "EXCEC_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "执行时间" });
            v_TableInits.Add(new v_TableInit { columnName = "EXCEC_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "执行人" });
            v_TableInits.Add(new v_TableInit { columnName = "STATUS", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "状态-来自OptStatusConst" });
            v_TableInits.Add(new v_TableInit { columnName = "MEMO", columnTypeAndLimit = "NVARCHAR2(1000)", columnDesc = "备注" });

            this.TableInit("FLOWPROC", v_TableInits, true, "操作流程日志表");
        }
        #endregion 业务流程表

        #region 系统配置表相关
        /// <summary>
        /// 系统配置表结构初始化
        /// </summary>
        public void SysConfigInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "CALC_METHOD", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "计算方法" });
            v_TableInits.Add(new v_TableInit { columnName = "MIN_JJ", columnTypeAndLimit = "NUMBER(18, 8)", columnDesc = "最小缴交" });
            v_TableInits.Add(new v_TableInit { columnName = "SNJZ_LIXI", columnTypeAndLimit = "NUMBER(18, 8)", columnDesc = "上年结转利息" });
            v_TableInits.Add(new v_TableInit { columnName = "JNHJ_LIXI", columnTypeAndLimit = "NUMBER(18, 8)", columnDesc = "今年汇缴利息" });
            v_TableInits.Add(new v_TableInit { columnName = "DT_CREATE", columnTypeAndLimit = "TIMESTAMP", columnDesc = "系统开始时间" });
            v_TableInits.Add(new v_TableInit { columnName = "DT_SYSTEM", columnTypeAndLimit = "TIMESTAMP", columnDesc = "系统账户日期" });
            v_TableInits.Add(new v_TableInit { columnName = "SYS_FLAG", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "系统标志" });
            v_TableInits.Add(new v_TableInit { columnName = "DKYLL", columnTypeAndLimit = "NUMBER(18, 8)", columnDesc = "贷款月利率" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "城市网点" });

            this.TableInit("SYS_CONFIG", v_TableInits, true, "系统配置表");
        }

        /// <summary>
        /// 系统配置表数据初始化
        /// </summary>
        /// <param name="city_cent"></param>
        public void SysConfigInitData(string city_cent)
        {
            using (var syBaseConn = SybaseConnector.Conn())
            {
                //获取原基本信息表
                var org_dwXinxiSql = @" select * from system_config ";
                List<SD_system_config> sD_Dwxinxis = syBaseConn.Query<SD_system_config>(org_dwXinxiSql).ToList();
                var models = sD_Dwxinxis.Select(x =>
                                       new D_SYS_CONFIG
                                       {
                                           CALC_METHOD = x.jsff,
                                           CITY_CENTNO = city_cent,
                                           DKYLL = x.dc_dk_yll,
                                           DT_CREATE = x.dt_create,
                                           DT_SYSTEM = x.dt_system,
                                           JNHJ_LIXI = x.jnhjlixi,
                                           MIN_JJ = x.minjj,
                                           SNJZ_LIXI = x.snjzlixi,
                                           SYS_FLAG = x.i_flag
                                       }).ToList();
                SugarHelper.Instance().Add(models);
            }
        }
        #endregion

        #endregion 系统配置相关

        #region  单位管理相关
        /// <summary>
        /// 单位业务信息主表结构初始化
        /// </summary>
        public void BusiCorporationInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "UNIQUE_KEY", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "标识符号-单位账号/统一信用代码" });
            v_TableInits.Add(new v_TableInit { columnName = "BUSITYPE", columnTypeAndLimit = "NUMBER(10)", columnDesc = "业务类型" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_TIME", columnTypeAndLimit = "TIMESTAMP ", columnDesc = "保存时间" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "保存人" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_TIME", columnTypeAndLimit = "TIMESTAMP ", columnDesc = "提交时间" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "提交人" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_TIME", columnTypeAndLimit = "TIMESTAMP ", columnDesc = "审核时间" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "审核人" });
            v_TableInits.Add(new v_TableInit { columnName = "SYSTEM_TIME", columnTypeAndLimit = "TIMESTAMP ", columnDesc = "账务时间" });
            v_TableInits.Add(new v_TableInit { columnName = "STATUS", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "业务状态" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "城市网点编号" });
            v_TableInits.Add(new v_TableInit { columnName = "MEMO", columnTypeAndLimit = "NVARCHAR2(1000)", columnDesc = "备注" });

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
            v_TableInits.Add(new v_TableInit { columnName = "CUSTID", columnTypeAndLimit = "NVARCHAR2(120) UNIQUE", columnDesc = "客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWMC", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "单位名称" });
            v_TableInits.Add(new v_TableInit { columnName = "DWMCSX", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "单位名称缩写" });
            v_TableInits.Add(new v_TableInit { columnName = "DWDZ", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "单位地址" });
            v_TableInits.Add(new v_TableInit { columnName = "DWXZ", columnTypeAndLimit = "NVARCHAR2(4)", columnDesc = "单位性质" });
            v_TableInits.Add(new v_TableInit { columnName = "DWYB", columnTypeAndLimit = "NVARCHAR2(6)", columnDesc = "单位邮编" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFXR", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "单位发薪日" });
            v_TableInits.Add(new v_TableInit { columnName = "ZZJGDM", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "统一社会信用代码" });
            v_TableInits.Add(new v_TableInit { columnName = "DWSLRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "单位设立日期" });
            v_TableInits.Add(new v_TableInit { columnName = "DWKHRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "单位开户日期" });
            v_TableInits.Add(new v_TableInit { columnName = "DWQJRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "单位起缴日期" });
            v_TableInits.Add(new v_TableInit { columnName = "BASICACCTBRCH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "基本存款户开户行" });
            v_TableInits.Add(new v_TableInit { columnName = "BASICACCTNO", columnTypeAndLimit = "NVARCHAR2(30)", columnDesc = "基本存款户账号" });
            v_TableInits.Add(new v_TableInit { columnName = "BASICACCTMC", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "基本存款户名称" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFRDBXM", columnTypeAndLimit = "NVARCHAR2(120)", columnDesc = "单位法人代表姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFRDBZJLX", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "单位法人代表证件类型" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFRDBZJHM", columnTypeAndLimit = "NVARCHAR2(18)", columnDesc = "单位法人代表证件号码" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRXM", columnTypeAndLimit = "NVARCHAR2(120)", columnDesc = "经办人姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRGDDHHM", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "经办人固定电话号码" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRSJHM", columnTypeAndLimit = "NVARCHAR2(11)", columnDesc = "经办人手机号码" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRZJLX", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "经办人证件类型" });
            v_TableInits.Add(new v_TableInit { columnName = "JBRZJHM", columnTypeAndLimit = "NVARCHAR2(18)", columnDesc = "经办人证件号码" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "城市网点编号" });
            v_TableInits.Add(new v_TableInit { columnName = "OPERID", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "操作员" });
            this.TableInit("CORPORATION_BASICINFO", v_TableInits, true, "单位基本信息表");
            #endregion
        }


        /// <summary>
        /// 单位基本信息表数据初始化
        /// </summary>
        /// <returns></returns>
        public void CorporationBasicInfoInitData(string city_cent)
        {
            SugarHelper.Instance().ExecuteCommand(" delete from CORPORATION_BASICINFO ");
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
                    CUSTID = Common.PaddingLeftZero(Convert.ToInt32(x.dwzh_gbk), 8),
                    ZZJGDM = x.s_zzjgdm_gbk,
                    DWDZ = x.dwdz_gbk,
                    DWFRDBXM = x.s_frdb_gbk,
                    DWKHRQ = string.IsNullOrEmpty(x.dt_kaihu_gbk) ? null : Convert.ToDateTime(x.dt_kaihu_gbk),
                    DWMC = x.dwmc_gbk,
                    DWMCSX = Common.ConvertChineseToPinYinShouZiMu(x.dwmc_gbk, true),
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
            v_TableInits.Add(new v_TableInit { columnName = "CUSTID", columnTypeAndLimit = "NVARCHAR2(120) UNIQUE", columnDesc = "客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "NVARCHAR2(20) UNIQUE", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "JZNY", columnTypeAndLimit = "NVARCHAR2(6)", columnDesc = "缴至年月" });
            v_TableInits.Add(new v_TableInit { columnName = "NEXTPAYMTH", columnTypeAndLimit = "TIMESTAMP", columnDesc = "下次应缴日期" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCBL", columnTypeAndLimit = "NUMBER(4, 2)", columnDesc = "单位缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCBL", columnTypeAndLimit = "NUMBER(4, 2)", columnDesc = "个人缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZGRS", columnTypeAndLimit = "NUMBER(18, 0)", columnDesc = "单位职工人数" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCRS", columnTypeAndLimit = "NUMBER(18, 0)", columnDesc = "单位缴存人数" });
            v_TableInits.Add(new v_TableInit { columnName = "DWFCRS", columnTypeAndLimit = "NUMBER(18, 0)", columnDesc = "单位封存人数" });
            v_TableInits.Add(new v_TableInit { columnName = "FACTINCOME", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "工资总数" });
            v_TableInits.Add(new v_TableInit { columnName = "MONTHPAYTOTALAMT", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "月缴存总额" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZHZT", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "单位账户状态" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZHYE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "单位账户余额" });
            v_TableInits.Add(new v_TableInit { columnName = "REGHANDBAL", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "挂账户余额" });
            v_TableInits.Add(new v_TableInit { columnName = "DWXHRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "单位销户日期 " });
            v_TableInits.Add(new v_TableInit { columnName = "DWXHYY", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "单位销户原因" });
            v_TableInits.Add(new v_TableInit { columnName = "FROMFLAG", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "补贴资金来源" });
            v_TableInits.Add(new v_TableInit { columnName = "OPENPERSALREADY", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "员工户已开立标" });
            v_TableInits.Add(new v_TableInit { columnName = "STYHMC", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "受托银行名称" });
            v_TableInits.Add(new v_TableInit { columnName = "STYHDM", columnTypeAndLimit = "NVARCHAR2(3)", columnDesc = "受托银行代码" });
            v_TableInits.Add(new v_TableInit { columnName = "STYHZH", columnTypeAndLimit = "NVARCHAR2(30)", columnDesc = "受托银行账号" });
            v_TableInits.Add(new v_TableInit { columnName = "CALC_METHOD", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "计算方法" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "城市网点编号" });

            this.TableInit("CORPORATION_ACCTINFO", v_TableInits, true, "单位账户信息表");
            #endregion
        }

        /// <summary>
        /// 单位账户信息表数据初始化
        /// </summary>
        public void CorporationAcctInfoInitData(string city_cent)
        {
            SugarHelper.Instance().ExecuteCommand(" delete from CORPORATION_ACCTINFO ");
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
                    CUSTID = Common.PaddingLeftZero(Convert.ToInt32(x.dwzh_gbk), 8),
                    CITY_CENTNO = city_cent,
                    DWZH = x.dwzh_gbk,
                    DWZHZT = Common.DwzhztConstSwitch(x.fcbj_gbk),
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
                    OPENPERSALREADY = OpenPerSalReadyConst.未开户,
                    FROMFLAG = x.i_btly_gbk,
                    NEXTPAYMTH = Convert.ToDateTime(x.xcrq_gbk),
                    JZNY = Convert.ToDateTime(x.xcrq_gbk).AddMonths(-1).ToString("yyyyMM"),
                    GRJCBL = 0,
                }).ToList();

                var sqlHelper = SugarHelper.Instance();
                sqlHelper.Add(models);

                var dwAccList = sqlHelper.QueryWhereList<D_CORPORATION_ACCTINFO>(x => x.CITY_CENTNO == city_cent);

                var sql = string.Empty;
                foreach (var item in dwAccList)
                {
                    //单位挂账余额
                    sql = $"select dc_ye  from zm where s_kmbm = '219666{item.DWZH}' order by l_incode desc ";
                    item.REGHANDBAL = syBaseConn.QueryFirstOrDefault<decimal>(sql);

                    //单位账户余额
                    sql = $"select ljye from kmmanager where kmbm = '201{item.DWZH}' ";
                    item.DWZHYE = syBaseConn.QueryFirstOrDefault<decimal>(sql);
                    sqlHelper.Update(item);
                }
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
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWMC", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "单位名称" });
            v_TableInits.Add(new v_TableInit { columnName = "KHTYPE", columnTypeAndLimit = "NUMBER(10)", columnDesc = "开户类型(按月1，一次性2)" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "保存时间" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "保存人" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "提交时间" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "提交人" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "审核时间" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "审核人" });
            v_TableInits.Add(new v_TableInit { columnName = "SYSTEM_TIME", columnTypeAndLimit = "TIMESTAMP ", columnDesc = "账务时间" });
            v_TableInits.Add(new v_TableInit { columnName = "STATUS", columnTypeAndLimit = "NVARCHAR2(30)", columnDesc = "业务状态" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "城市网点编号" });
            v_TableInits.Add(new v_TableInit { columnName = "MEMO", columnTypeAndLimit = "NVARCHAR2(1000)", columnDesc = "备注" });

            this.TableInit("GRKH", v_TableInits, true, "个人开户业务主表");
        }

        /// <summary>
        /// 个人开户明细表结构初始化
        /// </summary>
        public void Grkh_ItemInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "YWYD", columnTypeAndLimit = "NVARCHAR2(10)", columnDesc = "业务月度" });
            v_TableInits.Add(new v_TableInit { columnName = "XINGMING", columnTypeAndLimit = "NVARCHAR2(120)", columnDesc = "姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "ZJLX", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "证件类型" });
            v_TableInits.Add(new v_TableInit { columnName = "ZJHM", columnTypeAndLimit = "NVARCHAR2(18)", columnDesc = "证件号码" });
            v_TableInits.Add(new v_TableInit { columnName = "XINGBIE", columnTypeAndLimit = "NVARCHAR2(1)", columnDesc = "性别" });
            v_TableInits.Add(new v_TableInit { columnName = "CSNY", columnTypeAndLimit = "TIMESTAMP", columnDesc = "出生日期" });
            v_TableInits.Add(new v_TableInit { columnName = "SJHM", columnTypeAndLimit = "NVARCHAR2(11)", columnDesc = "手机号码" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCBL", columnTypeAndLimit = "NUMBER(4, 2)", columnDesc = "单位缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCBL", columnTypeAndLimit = "NUMBER(4, 2)", columnDesc = "个人缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCJS", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人缴存基数" });
            v_TableInits.Add(new v_TableInit { columnName = "DWYJCE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "单位月缴存额" });
            v_TableInits.Add(new v_TableInit { columnName = "GRYJCE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人月缴存额" });
            v_TableInits.Add(new v_TableInit { columnName = "QJRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "起缴日期" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZHZT", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "个人账户状态" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHHM", columnTypeAndLimit = "NVARCHAR2(30)", columnDesc = "个人存款账户号码" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHKHMC", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "个人存款账户开户名称" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHKHYHDM", columnTypeAndLimit = "NVARCHAR2(30)", columnDesc = "个人存款账户开户银行代码" });
            v_TableInits.Add(new v_TableInit { columnName = "WORK_DATE", columnTypeAndLimit = "TIMESTAMP", columnDesc = "工作时间" });
            v_TableInits.Add(new v_TableInit { columnName = "GDDHHM", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "固定电话号码" });

            this.TableInit("GRKH_ITEM", v_TableInits, true, "个人开户业务明细表");
        }

        /// <summary>
        /// 个人基本信息表结构初始化
        /// </summary>
        public void CustomerBasicInfoInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "CUSTID", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "XINGMING", columnTypeAndLimit = "NVARCHAR2(120)", columnDesc = "姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "XMQP", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "姓名全拼" });
            v_TableInits.Add(new v_TableInit { columnName = "ZJLX", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "证件类型" });
            v_TableInits.Add(new v_TableInit { columnName = "ZJHM", columnTypeAndLimit = "NVARCHAR2(18)", columnDesc = "证件号码" });
            v_TableInits.Add(new v_TableInit { columnName = "XINGBIE", columnTypeAndLimit = "NVARCHAR2(1)", columnDesc = "性别" });
            v_TableInits.Add(new v_TableInit { columnName = "CSNY", columnTypeAndLimit = "TIMESTAMP", columnDesc = "出生年月" });
            v_TableInits.Add(new v_TableInit { columnName = "WORK_DATE", columnTypeAndLimit = "TIMESTAMP", columnDesc = "参加工作时间" });
            v_TableInits.Add(new v_TableInit { columnName = "GDDHHM", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "固定电话号码" });
            v_TableInits.Add(new v_TableInit { columnName = "SJHM", columnTypeAndLimit = "NVARCHAR2(11)", columnDesc = "手机号码" });
            v_TableInits.Add(new v_TableInit { columnName = "HYZK", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "婚姻状况" });
            v_TableInits.Add(new v_TableInit { columnName = "MATE_CUSTID", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "配偶客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "创建用户" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "创建时间" });
            v_TableInits.Add(new v_TableInit { columnName = "UPDATE_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "最后修改用户" });
            v_TableInits.Add(new v_TableInit { columnName = "UPDATE_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "最后修改时间" });
            v_TableInits.Add(new v_TableInit { columnName = "OPER_ID", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "操作员" });
            v_TableInits.Add(new v_TableInit { columnName = "LASTDEAL_DATE", columnTypeAndLimit = "TIMESTAMP", columnDesc = "日期戳-有变化就更新" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "城市网点" });

            this.TableInit("CUSTOMER_BASICINFO", v_TableInits, true, "个人基本信息表");
        }

        /// <summary>
        /// 个人账户信息表结构初始化
        /// </summary>
        public void CustomerAcctInfoInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "CUSTID", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "客户编号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZH", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "个人账号" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZHZT", columnTypeAndLimit = "NVARCHAR2(2)", columnDesc = "个人账户状态" });
            v_TableInits.Add(new v_TableInit { columnName = "KHRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "开户日期" });
            v_TableInits.Add(new v_TableInit { columnName = "LOCK_DATE", columnTypeAndLimit = "TIMESTAMP", columnDesc = "封存日期" });
            v_TableInits.Add(new v_TableInit { columnName = "LOCK_REASON", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "封存原因" });
            v_TableInits.Add(new v_TableInit { columnName = "UNLOCK_DATE", columnTypeAndLimit = "TIMESTAMP", columnDesc = "启封日期" });
            v_TableInits.Add(new v_TableInit { columnName = "XHRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "销户日期" });
            v_TableInits.Add(new v_TableInit { columnName = "XHYY", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "销户原因" });
            v_TableInits.Add(new v_TableInit { columnName = "ACCT_TYPE", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "账户类型 0：按月 1:一次性" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCBL", columnTypeAndLimit = "NUMBER(4, 2)", columnDesc = "单位缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCBL", columnTypeAndLimit = "NUMBER(4, 2)", columnDesc = "个人缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCJS", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人缴存基数" });
            v_TableInits.Add(new v_TableInit { columnName = "DWYJCE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "单位月缴存额" });
            v_TableInits.Add(new v_TableInit { columnName = "GRYJCE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人月缴存额" });
            v_TableInits.Add(new v_TableInit { columnName = "MONTHPAYAMT", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "月汇缴额" });
            v_TableInits.Add(new v_TableInit { columnName = "LASTPAYMONTH", columnTypeAndLimit = "TIMESTAMP", columnDesc = "末次汇缴月" });
            v_TableInits.Add(new v_TableInit { columnName = "QJRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "起缴日期" });
            v_TableInits.Add(new v_TableInit { columnName = "YCXBTJE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "一次性补贴金额" });
            v_TableInits.Add(new v_TableInit { columnName = "YCX_CHECK_FLAG", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = " 一次性缴存状态 0：未缴交 1：生成汇缴业务未记账 2：已复核" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZHYE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人账户余额" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZHSNJZYE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人账户上年结转余额" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZHSNJZRQ", columnTypeAndLimit = "TIMESTAMP", columnDesc = "个人账户上年结转日期" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZHDNGJYE", columnTypeAndLimit = "NUMBER(18, 2)", columnDesc = "个人账户当年归集余额" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHHM", columnTypeAndLimit = "NVARCHAR2(30)", columnDesc = "个人存款账户号码" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHKHMC", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "个人存款账户开户名称" });
            v_TableInits.Add(new v_TableInit { columnName = "GRCKZHKHYHDM", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "个人存款账户开户银行代码" });
            v_TableInits.Add(new v_TableInit { columnName = "CHECK_FLAG", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "核定标志" });
            v_TableInits.Add(new v_TableInit { columnName = "LASTDEALDATE", columnTypeAndLimit = "TIMESTAMP", columnDesc = "日期戳" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "城市网点" });

            this.TableInit("CUSTOMER_ACCTINFO", v_TableInits, true, "个人账户信息表");
        }

        /// <summary>
        /// 个人基本信息/个人账户信息数据初始化
        /// </summary>
        /// <param name="city_cent"></param>
        public void CustomerInfoInitData(string city_cent)
        {
            var sugarHelper = SugarHelper.Instance();
            sugarHelper.ExecuteCommand("delete from CUSTOMER_BASICINFO ");
            sugarHelper.ExecuteCommand("delete from CUSTOMER_ACCTINFO ");

            using (var syBaseConn = SybaseConnector.Conn())
            {

                //获取单位基本信息
                var dwInfoList = sugarHelper.QueryList<D_CORPORATION_ACCTINFO>();

                //获取个人基本信息
                var org_grgjjxxSql = @" select a.*,b.ljye grzhye from grgjjxx a left join kmmanager b on b.bm1 = '201' and b.bm2 = a.dwzh and b.bm3=a.grzh order by dwzh asc,grzh asc  ";
                var org_grgjjxxSqlList = syBaseConn.Query<SD_grgjjxx>(org_grgjjxxSql).ToList();
                List<D_CUSTOMER_ACCTINFO> D_CUSTOMER_ACCTINFO = org_grgjjxxSqlList
                    .Select(x => new D_CUSTOMER_ACCTINFO
                    {
                        CITY_CENTNO = city_cent,
                        CUSTID = Common.PersonCustIDGenerate(string.Empty, x.dwzh_gbk, x.grzh_gbk, true),
                        ACCT_TYPE = x.i_btfs_gbk,
                        CHECK_FLAG = null,
                        DWJCBL = dwInfoList.FirstOrDefault(y => y.DWZH == x.dwzh_gbk).DWJCBL,
                        DWZH = x.dwzh_gbk,
                        DWYJCE = x.dwgjj_gbk ?? 0,
                        GRCKZHHM = x.s_yh_grzh_gbk,
                        GRJCBL = 0,
                        GRJCJS = x.gze_gbk ?? 0,
                        GRYJCE = 0,
                        GRZH = $"{x.dwzh_gbk}{x.grzh_gbk}",
                        GRZHYE = x.grzhye,
                        GRZHSNJZYE = x.dc_snje_gbk ?? 0,
                        GRZHDNGJYE = 0,
                        GRZHZT = Common.GrzhztConstSwitch(x.fcbj_gbk),
                        KHRQ = x.dt_kaihu_gbk,
                        MONTHPAYAMT = x.dwgjj_gbk ?? 0,
                        YCX_CHECK_FLAG = x.i_jj_flag_gbk,
                        YCXBTJE = x.dc_btje_gbk ?? 0,
                        QJRQ = x.dt_qjrq_gbk,
                        GRZHSNJZRQ = x.dt_snjzrq_gbk,
                        LASTDEALDATE = DateTime.Now,
                        LOCK_REASON = x.i_fc_status_gbk,
                        LASTPAYMONTH = x.xcrq_gbk,
                        XHYY = Common.GrXiaoHuReasonConstSwitch(x.fcbj_gbk),
                    }).ToList();

                sugarHelper.Add(D_CUSTOMER_ACCTINFO);

                //获取个人基本信息
                var org_grxinxiSql = @" select * from grxinxi  order by dwzh asc,grzh asc  ";
                List<D_CUSTOMER_BASICINFO> D_CUSTOMER_BASICINFO = syBaseConn.Query<SD_grxinxi>(org_grxinxiSql)
                    .Select(x => new D_CUSTOMER_BASICINFO
                    {
                        CITY_CENTNO = city_cent,
                        CSNY = x.csny_gbk,
                        GDDHHM = x.s_lxdh_gbk,
                        ZJHM = string.IsNullOrEmpty(x.sfzhm_gbk) ? x.sfzhm_gbk : x.sfzhm_gbk.Trim(),
                        XINGMING = x.grxm_gbk,
                        XMQP = Common.ConvertChineseToPinYin(x.grxm_gbk),
                        XINGBIE = Common.XingBieConstSwitch(x.grxb_gbk),
                        CUSTID = Common.PersonCustIDGenerate(x.sfzhm_gbk, x.dwzh_gbk, x.grzh_gbk, true),
                        SJHM = x.s_sjh_gbk,
                        WORK_DATE = org_grgjjxxSqlList.FirstOrDefault(y => y.dwzh_gbk == x.dwzh_gbk && y.grzh_gbk == x.grzh_gbk).dt_work_gbk,
                        OPER_ID = org_grgjjxxSqlList.FirstOrDefault(y => y.dwzh_gbk == x.dwzh_gbk && y.grzh_gbk == x.grzh_gbk).s_shouliren_gbk,
                    }).ToList();

                sugarHelper.Add(D_CUSTOMER_BASICINFO);

                var sql = @" SELECT DWZH,SUM(GRJCJS) FACTINCOME,SUM(MONTHPAYAMT) MONTHPAYTOTALAMT,SUM(grzhztall) DWZGRS,SUM(grzhzt02) DWFCRS,SUM(grzhzt01_02) DWJCRS
                            FROM 
                            (
	                            SELECT DWZH,
				                             CASE 
						                             WHEN GRZHZT in ('01','02') AND ACCT_TYPE = 0 THEN grjcjs 
						                             ELSE 0 
				                             END AS grjcjs,MONTHPAYAMT,1 grzhztall,
				                             CASE 
						                             WHEN GRZHZT = '02' AND ACCT_TYPE = 0 THEN 1 
						                             ELSE 0 
				                             END AS grzhzt02,
				                             CASE 
						                             WHEN GRZHZT in('01','02') AND ACCT_TYPE = 0 THEN 1 
						                             ELSE 0 
				                             END AS grzhzt01_02
	                            FROM CUSTOMER_ACCTINFO where CITY_CENTNO = :CITY_CENTNO
                            ) tt
                            GROUP BY dwzh ";

                var dwPersonInfo = sugarHelper.QueryDataTable<v_CorporationPersonInfo>(sql, new List<Microsoft.Data.SqlClient.SqlParameter> { new Microsoft.Data.SqlClient.SqlParameter("CITY_CENTNO", city_cent) });

                //获取单位账户信息表
                var dwAcctList = sugarHelper.QueryWhereList<D_CORPORATION_ACCTINFO>(x => x.CITY_CENTNO == city_cent);

                Action action = null;
                foreach (var item in dwPersonInfo)
                {
                    var dwAcct = dwAcctList.FirstOrDefault(x => x.DWZH == item.dwzh);
                    if (dwAcct != null)
                    {
                        dwAcct.DWZGRS = item.DWZGRS;
                        dwAcct.DWFCRS = item.DWFCRS;
                        dwAcct.DWJCRS = item.DWJCRS;
                        dwAcct.FACTINCOME = item.FACTINCOME;
                        dwAcct.MONTHPAYTOTALAMT = item.MONTHPAYTOTALAMT;
                        dwAcct.OPENPERSALREADY = OpenPerSalReadyConst.已开户;
                        action += () => sugarHelper.Update(dwAcct);
                    }
                }

                sugarHelper.InvokeTransactionScope(action);
            }
        }


        #endregion 客户管理相关

        #region 科目相关
        /// <summary>
        /// 菜单表结构初始化
        /// </summary>
        public void KMInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "S_KMBM", columnTypeAndLimit = "varchar2(128) not null", columnDesc = "科目编码" });
            v_TableInits.Add(new v_TableInit { columnName = "I_GRADE", columnTypeAndLimit = "number(5) not null", columnDesc = "等级" });
            v_TableInits.Add(new v_TableInit { columnName = "S_BM1", columnTypeAndLimit = "varchar2(6) not null", columnDesc = "编码1" });
            v_TableInits.Add(new v_TableInit { columnName = "S_BM2", columnTypeAndLimit = "varchar2(6)", columnDesc = "编码2" });
            v_TableInits.Add(new v_TableInit { columnName = "S_BM3", columnTypeAndLimit = "varchar2(6)", columnDesc = "编码3" });
            v_TableInits.Add(new v_TableInit { columnName = "S_BM4", columnTypeAndLimit = "varchar2(6)", columnDesc = "编码4" });
            v_TableInits.Add(new v_TableInit { columnName = "S_MC1", columnTypeAndLimit = "varchar2(18)", columnDesc = "名称1" });
            v_TableInits.Add(new v_TableInit { columnName = "S_MC2", columnTypeAndLimit = "varchar2(36)", columnDesc = "名称2" });
            v_TableInits.Add(new v_TableInit { columnName = "S_MC3", columnTypeAndLimit = "varchar2(36)", columnDesc = "名称3" });
            v_TableInits.Add(new v_TableInit { columnName = "S_MC4", columnTypeAndLimit = "varchar2(20)", columnDesc = "名称4" });
            v_TableInits.Add(new v_TableInit { columnName = "S_KMMC", columnTypeAndLimit = "varchar2(60)", columnDesc = "科目名称" });
            v_TableInits.Add(new v_TableInit { columnName = "I_KMXZ", columnTypeAndLimit = "number(5) not null", columnDesc = "科目性质" });
            v_TableInits.Add(new v_TableInit { columnName = "I_KMLX", columnTypeAndLimit = "number(5) not null", columnDesc = "科目类型" });
            v_TableInits.Add(new v_TableInit { columnName = "DT_JZRQ", columnTypeAndLimit = "TIMESTAMP not null", columnDesc = "截止日期" });
            v_TableInits.Add(new v_TableInit { columnName = "I_KMJD", columnTypeAndLimit = "number(5) not null", columnDesc = "科目精度" });
            v_TableInits.Add(new v_TableInit { columnName = "S_MEMO", columnTypeAndLimit = "varchar2(100)", columnDesc = "备注" });


            this.TableInit("KM", v_TableInits, true, "科目表");
        }
        #endregion

        #region 缴存核定相关
        /// <summary>
        /// 按月汇缴业务表
        /// </summary>
        public void MonthDWJCInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "BATCHNO", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "批次号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCBL", columnTypeAndLimit = "NUMBER(4,2)", columnDesc = "单位缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCBL", columnTypeAndLimit = "NUMBER(4,2)", columnDesc = "个人缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "PAYMTH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "汇缴月份" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCRS", columnTypeAndLimit = "NUMBER(18)", columnDesc = "本月汇缴人数" });
            v_TableInits.Add(new v_TableInit { columnName = "MTHPAYAMT", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "本月汇缴金额" });
            v_TableInits.Add(new v_TableInit { columnName = "LASTMTHPAYNUM", columnTypeAndLimit = "NUMBER(18)", columnDesc = "上月汇缴人数" });
            v_TableInits.Add(new v_TableInit { columnName = "LASTMTHPAY", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "上月汇缴金额" });
            v_TableInits.Add(new v_TableInit { columnName = "MTHPAYNUMPLS", columnTypeAndLimit = "NUMBER(18)", columnDesc = "本月增加人数" });
            v_TableInits.Add(new v_TableInit { columnName = "MTHPAYAMTPLS", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "本月增加金额" });
            v_TableInits.Add(new v_TableInit { columnName = "MTHPAYNUMMNS", columnTypeAndLimit = "NUMBER(18)", columnDesc = "本月减少人数" });
            v_TableInits.Add(new v_TableInit { columnName = "MTHPAYAMTMNS", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "本月减少金额" });
            v_TableInits.Add(new v_TableInit { columnName = "BASECHGNUM", columnTypeAndLimit = "NUMBER(18)", columnDesc = "基数调整人数" });
            v_TableInits.Add(new v_TableInit { columnName = "BASECHGAMT", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "基数调整金额" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "保存时间" });
            v_TableInits.Add(new v_TableInit { columnName = "CREATE_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "保存人" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "提交时间" });
            v_TableInits.Add(new v_TableInit { columnName = "SUBMIT_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "提交人" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_TIME", columnTypeAndLimit = "TIMESTAMP", columnDesc = "审核时间" });
            v_TableInits.Add(new v_TableInit { columnName = "VERIFY_MAN", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "审核人" });
            v_TableInits.Add(new v_TableInit { columnName = "STATUS", columnTypeAndLimit = "NVARCHAR2(30)", columnDesc = "业务状态" });
            v_TableInits.Add(new v_TableInit { columnName = "VCHRNOS", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "凭证号字符串" });
            v_TableInits.Add(new v_TableInit { columnName = "MEMO", columnTypeAndLimit = "NVARCHAR2(1000)", columnDesc = "备注" });
            v_TableInits.Add(new v_TableInit { columnName = "CITY_CENTNO", columnTypeAndLimit = "NVARCHAR2(360)", columnDesc = "城市网点编号" });
            this.TableInit("MONTH_DWJC", v_TableInits, true, "按月汇缴业务表");
        }

        /// <summary>
        /// 按月汇缴清册表
        /// </summary>
        public void MonthDWJCQCInitStructure()
        {
            List<v_TableInit> v_TableInits = new List<v_TableInit> { };
            v_TableInits.Add(new v_TableInit { columnName = "ID", columnTypeAndLimit = "NUMBER(10) primary key", columnDesc = "ID" });
            v_TableInits.Add(new v_TableInit { columnName = "YWLSH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "业务流水号" });
            v_TableInits.Add(new v_TableInit { columnName = "DWZH", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "单位账号" });
            v_TableInits.Add(new v_TableInit { columnName = "PAYMTH", columnTypeAndLimit = "NVARCHAR2(255)", columnDesc = "汇缴月份" });
            v_TableInits.Add(new v_TableInit { columnName = "ZJHM", columnTypeAndLimit = "NVARCHAR2(18)", columnDesc = "证件号码" });
            v_TableInits.Add(new v_TableInit { columnName = "XINGMING", columnTypeAndLimit = "NVARCHAR2(120)", columnDesc = "个人姓名" });
            v_TableInits.Add(new v_TableInit { columnName = "GRZH", columnTypeAndLimit = "NVARCHAR2(20)", columnDesc = "个人账号" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCJS", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "个人缴存基数" });
            v_TableInits.Add(new v_TableInit { columnName = "DWJCBL", columnTypeAndLimit = "NUMBER(4,2)", columnDesc = "单位缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "GRJCBL", columnTypeAndLimit = "NUMBER(4,2)", columnDesc = "个人缴存比例" });
            v_TableInits.Add(new v_TableInit { columnName = "REMITPAYAMT", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "汇缴金额" });
            v_TableInits.Add(new v_TableInit { columnName = "DWYJCE", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "单位月缴存额" });
            v_TableInits.Add(new v_TableInit { columnName = "GRYJCE", columnTypeAndLimit = "NUMBER(18,2)", columnDesc = "个人月缴存额" });
            this.TableInit("MONTH_DWJCQC", v_TableInits, true, "按月汇缴业务清册表");
        }

        #endregion




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


