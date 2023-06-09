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
using SqlSugar;

namespace BtzjManagement.Api.Services
{
    /// <summary>
    /// 科目业务层
    /// </summary>
    public class KMService
    {
        /// <summary>
        /// 根据I_GRADE 科目等级 返回对应科目等级的名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public (string , string) convertKMMC_display(v_KM model)
        {
            switch (model.I_GRADE)
            {
                case 1:
                    return (model.S_MC1, model.S_BM1);
                case 2:
                    return (model.S_MC2, model.S_BM2);
                case 3:
                    return (model.S_MC3, model.S_BM3);
                case 4:
                    return (model.S_MC4, model.S_BM4);
                default:
                    return (model.S_MC1, model.S_BM1);
            }
        }
        /// <summary>
        /// 获取科目树
        /// </summary>
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
                I_KMJD_display = y.I_KMJD == 0 ? "借方科目" : "贷方科目"
            }).ToList();
            var dict = SugarSimple.Instance().Queryable<D_SYS_DATA_DICTIONARY, D_SYS_DATA_DICTIONARY>((d1, d2) => new object[] {
                JoinType.Left, d1.ID ==d2.PARENT_ID  }).Where((d1, d2) => d1.TYPE_KEY == "kmlx")
    .Select((d1, d2) => new D_SYS_DATA_DICTIONARY
    {
        ID = d2.ID,
        LABEL = d2.LABEL,
        CITY_CENTNO = d1.CITY_CENTNO,
        DESCRIPTION = d2.DESCRIPTION,
        ORIGIN_FLAG = d2.ORIGIN_FLAG,
        PARENT_ID = d2.PARENT_ID,
        REMARK = d2.REMARK,
        SORT_ID = d2.SORT_ID,
        TYPE_KEY = d1.TYPE_KEY,
        VAL = d2.VAL
    })
    .ToList();
            foreach (var item in allKms)
            {
                item.S_KMMC_display = this.convertKMMC_display(item).Item1;
                item.S_KMBM_display = this.convertKMMC_display(item).Item2;
                if (dict.FirstOrDefault(x => x.VAL == item.I_KMXZ.ToString()) != null)
                {
                    var f = dict.FirstOrDefault(x => x.VAL == item.I_KMXZ.ToString());
                    item.I_KMLX_display = f.LABEL;
                }
            }

            
            List<v_KM> root = new List<v_KM>();
            
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
            root.Add(new v_KM() { S_KMBM ="0", I_GRADE = 0, S_KMMC_display = "科目", S_KMBM_display = "科目", Subs = list1 });
            return root;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s_kmbm"></param>
        /// <returns></returns>
        public List<v_KM> GetKMTreeListBykmbm(string s_kmbm)
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
                I_KMJD_display = y.I_KMJD == 0 ? "借方科目" : "贷方科目"
            }).ToList();
            foreach (var item in allKms)
            {
                item.S_KMMC_display = this.convertKMMC_display(item).Item1;
                item.S_KMBM_display = this.convertKMMC_display(item).Item2;
                item.S_KMBM_display = item.S_KMBM;
            }
            var one = allKms.Where(x => x.S_KMBM == s_kmbm).FirstOrDefault();
            if (s_kmbm == "0")
            {
                one = new v_KM() { S_KMBM = "0", I_GRADE = 0, S_KMMC_display = "科目", S_KMBM_display = "科目" };
            }
            if (!string.IsNullOrWhiteSpace(s_kmbm))
            {
                if (one.I_GRADE == 1)
                {
                    allKms = allKms.Where(y => y.S_BM1 == one.S_BM1).ToList();
                }
                else if (one.I_GRADE == 2)
                {
                    allKms = allKms.Where(y => y.S_BM1 == one.S_BM1 && y.S_BM2 == one.S_BM2).ToList();
                }
                else if (one.I_GRADE == 3)
                {
                    allKms = allKms.Where(y => y.S_BM1 == one.S_BM1 && y.S_BM2 == one.S_BM2 & y.S_BM3 == one.S_BM3).ToList();
                }

            }
            List<v_KM> list1 = new List<v_KM>();

  
            list1 = allKms.Where(x => x.I_GRADE == (one.I_GRADE + 1)).ToList();
            
            

            //循环一级科目
            for (int i = 0; i < list1.Count; i++)
            {
                v_KM item1 = list1[i];

                //查询所有一级科目名称为当前循环item1的科目名称 并且等于二级科目
                var list2 = allKms.Where(x => x.I_GRADE == (one.I_GRADE + 2) && x.S_BM1 == item1.S_BM1).ToList();
                item1.Subs.AddRange(list2);
                //循环所有二级科目
                for (int i1 = 0; i1 < list2.Count; i1++)
                {
                    v_KM item2 = list2[i1];
                    var list3 = allKms.Where(x => x.I_GRADE == (one.I_GRADE + 3) && x.S_BM2 == item2.S_BM2 && x.S_BM1 == item1.S_BM1).ToList();
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
            D_KM model = await SugarSimple.Instance().Queryable<D_KM>().Where(x => x.S_KMBM == p.s_kmbm).FirstAsync();
            if (model == null)
            {
                model = new D_KM();
            }
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
                model.S_KMMC += p.s_mc1;
                if (!string.IsNullOrEmpty(p.s_mc2))
                {
                    model.S_KMMC += "|" + p.s_mc2;
                }
                if (!string.IsNullOrEmpty(p.s_mc3))
                {
                    model.S_KMMC += "|" + p.s_mc3;
                }
                if (!string.IsNullOrEmpty(p.s_mc4))
                {
                    model.S_KMMC += "|" + p.s_mc4;
                }
                model.S_KMBM += p.s_bm1;
                if (!string.IsNullOrEmpty(p.s_bm2))
                {
                    model.S_KMBM += p.s_bm2;
                }
                if (!string.IsNullOrEmpty(p.s_bm3))
                {
                    model.S_KMBM += p.s_bm3;
                }
                if (!string.IsNullOrEmpty(p.s_bm4))
                {
                    model.S_KMBM += p.s_bm4;
                }
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
        public async Task<(int code, string message)> DeleteKM(string s_kmbm)
        {
            var existingKM = await SugarSimple.Instance().Queryable<D_KM>().Where(u => u.S_KMBM == s_kmbm).FirstAsync();
            if (existingKM == null)
            {
                return (ApiResultCodeConst.ERROR, "科目不存在");
            }

            var deleteResult = await SugarSimple.Instance().Deleteable<D_KM>().In(s_kmbm).ExecuteCommandAsync();
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

