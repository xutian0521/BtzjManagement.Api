using BtzjManagement.Api.Models.DBModel;
using BtzjManagement.Api.Models.ViewModel;
using BtzjManagement.Api.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using YamlDotNet.Core;
using System;
using BtzjManagement.Api.Models.QueryModel;

namespace BtzjManagement.Api.Services
{
    /// <summary>
    /// 科目业务层
    /// </summary>
    public class KMService
    {
        /// <summary>
        /// 获取科目树
        /// </summary>
        /// <param name="kmgrade"></param>
        /// <returns></returns>
        public List<v_KM> GetKMTreeList()
        {
            var allKms = SugarSimple.Instance().Queryable<D_KM>().Select(y => new v_KM
            {
                DT_JZRQ = y.DT_JZRQ,
                I_GRADE = y.I_GRADE,
                S_BM1 = y.S_BM1,
                S_BM2 = y.S_BM2,
                S_BM3 = y.S_BM3,
                S_BM4 = y.S_BM4,
                I_KMJD = y.I_KMJD,
                I_KMLX = y.I_KMLX,
                I_KMXZ = y.I_KMXZ,
                S_KMBM = y.S_KMBM,
                S_KMMC = y.S_KMMC,
                S_MC1 = y.S_MC1,
                S_MC2 = y.S_MC2,
                S_MC3 = y.S_MC3,
                S_MC4 = y.S_MC4,
                S_MEMO = y.S_MEMO,
                I_KMJD_display = y.I_KMJD == 0? "借方科目": "贷方科目"
            }).ToList();
            var list1 = allKms.Where(x => x.I_GRADE == 1).ToList();
            //循环一级科目
            for (int i = 0; i < list1.Count; i++)
            {
                v_KM item1 = list1[i];

                //查询所有一级科目名称为当前循环item1的科目名称 并且等于二级科目
                var list2 = allKms.Where(x => x.I_GRADE == 2 && x.S_BM1 == item1.S_BM1).ToList();
                item1.Subs.AddRange(list2);
                //循环所有二级科目
                for (int i1 = 0; i1 < list2.Count; i1++)
                {
                    v_KM item2 = list2[i1];
                    var list3 = allKms.Where(x => x.I_GRADE == 3 && x.S_BM2 == item2.S_BM2 && x.S_BM1 == item1.S_BM1).ToList();
                    item2.Subs.AddRange(list3);
                }
            }
            
            return list1;
        }

        /// <summary>
        /// 添加或修改科目
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<(int code, string message)> AddOrModifyKM(P_AddOrModifyKM p)
        {
            var existedKM = SugarSimple.Instance().Queryable<D_KM>().Where(x => x.S_KMBM == p.s_kmbh).FirstAsync();
            var model = new D_KM();
            model.S_KMBM = p.s_kmbm;
            model.I_GRADE = p.i_grade;
            model.S_BM1 = p.s_bm1;
            model.S_BM2 = p.s_bm2;
            model.S_BM3 = p.s_bm3;
            model.S_BM4 = p.s_bm4;
            model.S_MC1 = p.s_mc1;
            model.S_MC2 = p.s_mc2;
            model.S_MC3 = p.s_mc3;
            model.S_MC4 = p.s_mc4;
            model.I_KMXZ = p.i_kmxz;
            model.I_KMLX = p.i_kmlx;
            model.DT_JZRQ = p.dt_jzrq;
            model.S_MEMO = p.MEMO;
            if (p.add_or_modify == "add")
            {
                var insertResult = await SugarSimple.Instance().Insertable(model).ExecuteCommandAsync();
                return insertResult > 0 ? (ApiResultCodeConst.SUCCESS, "添加成功！") : (ApiResultCodeConst.ERROR, "添加失败!");
            }
            else if(p.add_or_modify == "modify")
            {
                var updateResult = await SugarSimple.Instance().Updateable(model).ExecuteCommandAsync();
                return updateResult > 0 ? (ApiResultCodeConst.SUCCESS, "修改成功！") : (ApiResultCodeConst.ERROR, "修改失败!");
            }
            else
            {
                return (ApiResultCodeConst.ERROR, "add_or_modify 参数错误！");
            }
            
        }

        /// <summary>
        /// 删除科目
        /// </summary>
        /// <param name="kmbm">科目编码</param>
        /// <returns></returns>
        public async Task<(int code, string message)> DeleteKM(string kmbm)
        {
            var existingKM = await SugarSimple.Instance().Queryable<D_KM>().Where(u => u.S_KMBM == kmbm).FirstAsync();
            if (existingKM == null)
            {
                return (ApiResultCodeConst.ERROR, "科目不存在");
            }

            var deleteResult = await SugarSimple.Instance().Deleteable<D_KM>().In(kmbm).ExecuteCommandAsync();
            if (deleteResult > 0)
            {
                return (ApiResultCodeConst.SUCCESS, "删除成功");
            }
            else
            {
                return (ApiResultCodeConst.ERROR, "删除失败");
            }
        }
        /// <summary>
        /// 载入修改菜单信息
        /// </summary>
        /// <param name="kmbm"></param>
        /// <returns></returns>
        public async Task<D_KM> LoadModifyKM(string kmbm)
        {
            var query = SugarSimple.Instance().Queryable<D_KM>();
            var one = await query.Where(u => u.S_KMBM == kmbm).FirstAsync();
            return one;
        }
    }
    
}

