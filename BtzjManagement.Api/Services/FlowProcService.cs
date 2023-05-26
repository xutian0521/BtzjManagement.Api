using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Services
{
    public class FlowProcService
    {
        /// <summary>
        /// 添加业务操作流程
        /// </summary>
        /// <param name="ywlsh">业务流水号</param>
        /// <param name="ywid">业务id</param>
        /// <param name="dwzh">单位账号</param>
        /// <param name="procName">流程名称</param>
        /// <param name="execMan">操作人</param>
        /// <param name="status">状态-来自OptStatusConst</param>
        /// <param name="memo">备注</param>
        /// <returns></returns>
        public bool AddFlowProc(string ywlsh, int ywid, string dwzh, string procName, string execMan, string status, string memo = "", ISugarHelper sugarHelper=null)
        {
            D_FLOWPROC model = new D_FLOWPROC
            {
                YWLSH = ywlsh,
                YWID = ywid,
                DWZH = dwzh,
                PROC_NAME = procName,
                EXCEC_TIME = DateTime.Now,
                EXCEC_MAN = execMan,
                STATUS = status,
                MEMO = memo
            };
            if(sugarHelper == null)
            {
                return SugarHelper.Instance().AddReturnBool(model);
            }
            return sugarHelper.AddReturnBool(model);
        }
    }
}
