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
        /// 根据KMJB 科目等级 返回对应科目等级的名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public (string , string) convertKMMC_display(v_KM model)
        {
            switch (model.KMJB)
            {
                case 1:
                    return (model.MC1, model.BH1);
                case 2:
                    return (model.MC2, model.BH2);
                case 3:
                    return (model.MC3, model.BH3);
                case 4:
                    return (model.MC4, model.BH4);
                default:
                    return (model.MC1, model.BH1);
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
                JZRQ = y.JZRQ,
                KMJB = y.KMJB,
                BH1 = y.BH1,
                BH2 = y.BH2,
                BH3 = y.BH3,
                BH4 = y.BH4,
                KMJD = y.KMJD,
                KMLX = y.KMLX,
                KMXZ = y.KMXZ,
                KMBH = y.KMBH,
                KMMC = y.KMMC,
                MC1 = y.MC1,
                MC2 = y.MC2,
                MC3 = y.MC3,
                MC4 = y.MC4,
                MEMO = y.MEMO,
                KMJD_display = y.KMJD == 0 ? "借方科目" : "贷方科目"
            }).OrderBy(x => x.KMBH).ToList();
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
                item.KMMC_display = this.convertKMMC_display(item).Item1;
                item.KMBH_display = this.convertKMMC_display(item).Item2;
                item.KMBH_KMMC_display = item.KMBH_display + "-" + item.KMMC_display;
                if (dict.FirstOrDefault(x => x.VAL == item.KMLX.ToString()) != null)
                {
                    var f = dict.FirstOrDefault(x => x.VAL == item.KMLX.ToString());
                    item.KMLX_display = f.LABEL;
                }
            }

            
            List<v_KM> root = new List<v_KM>();
            
            var list1 = allKms.Where(x => x.KMJB == 1).ToList();

            //循环一级科目
            for (int i = 0; i < list1.Count; i++)
            {
                v_KM item1 = list1[i];

                //查询所有一级科目名称为当前循环item1的科目名称 并且等于二级科目
                var list2 = allKms.Where(x => x.KMJB == 2 && x.BH1 == item1.BH1).ToList();
                item1.Subs.AddRange(list2);
                //循环所有二级科目
                for (int i1 = 0; i1 < list2.Count; i1++)
                {
                    v_KM item2 = list2[i1];
                    var list3 = allKms.Where(x => x.KMJB == 3 && x.BH2 == item2.BH2 && x.BH1 == item1.BH1).ToList();
                    item2.Subs.AddRange(list3);
                }
            }
            root.Add(new v_KM() { KMBH ="0", KMJB = 0, KMMC_display = "科目",
                KMBH_KMMC_display = "科目", KMBH_display = "科目", Subs = list1 });
            return root;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="KMBH"></param>
        /// <returns></returns>
        public List<v_KM> GetKMTreeListByKMBH(string KMBH)
        {
            var allKms = SugarSimple.Instance().Queryable<D_KM>().Select(y => new v_KM
            {
                JZRQ = y.JZRQ,
                KMJB = y.KMJB,
                BH1 = y.BH1,
                BH2 = y.BH2,
                BH3 = y.BH3,
                BH4 = y.BH4,
                KMJD = y.KMJD,
                KMLX = y.KMLX,
                KMXZ = y.KMXZ,
                KMBH = y.KMBH,
                KMMC = y.KMMC,
                MC1 = y.MC1,
                MC2 = y.MC2,
                MC3 = y.MC3,
                MC4 = y.MC4,
                MEMO = y.MEMO,
                KMJD_display = y.KMJD == 0 ? "借方科目" : "贷方科目"
            }).OrderBy(x => x.KMBH).ToList();
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
                item.KMMC_display = this.convertKMMC_display(item).Item1;
                item.KMBH_display = this.convertKMMC_display(item).Item2;
                item.KMBH_display = item.KMBH;
                if (dict.FirstOrDefault(x => x.VAL == item.KMLX.ToString()) != null)
                {
                    var f = dict.FirstOrDefault(x => x.VAL == item.KMLX.ToString());
                    item.KMLX_display = f.LABEL;
                }
            }
            var one = allKms.Where(x => x.KMBH == KMBH).FirstOrDefault();
            if (KMBH == "0")
            {
                one = new v_KM() { KMBH = "0", KMJB = 0, KMMC_display = "科目", KMBH_display = "科目" };
            }
            if (!string.IsNullOrWhiteSpace(KMBH))
            {
                if (one.KMJB == 1)
                {
                    allKms = allKms.Where(y => y.BH1 == one.BH1).ToList();
                }
                else if (one.KMJB == 2)
                {
                    allKms = allKms.Where(y => y.BH1 == one.BH1 && y.BH2 == one.BH2).ToList();
                }
                else if (one.KMJB == 3)
                {
                    allKms = allKms.Where(y => y.BH1 == one.BH1 && y.BH2 == one.BH2 & y.BH3 == one.BH3).ToList();
                }

            }
            List<v_KM> list1 = new List<v_KM>();

  
            list1 = allKms.Where(x => x.KMJB == (one.KMJB + 1)).ToList();
            
            

            //循环一级科目
            for (int i = 0; i < list1.Count; i++)
            {
                v_KM item1 = list1[i];

                //查询所有一级科目名称为当前循环item1的科目名称 并且等于二级科目
                var list2 = allKms.Where(x => x.KMJB == (one.KMJB + 2) && x.BH1 == item1.BH1).ToList();
                item1.Subs.AddRange(list2);
                //循环所有二级科目
                for (int i1 = 0; i1 < list2.Count; i1++)
                {
                    v_KM item2 = list2[i1];
                    var list3 = allKms.Where(x => x.KMJB == (one.KMJB + 3) && x.BH2 == item2.BH2 && x.BH1 == item1.BH1).ToList();
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
            D_KM model = await SugarSimple.Instance().Queryable<D_KM>().Where(x => x.KMBH == p.KMBH).FirstAsync();
            if (model == null)
            {
                model = new D_KM();
            }
            model.KMJB = p.KMJB;
            model.BH1 = p.BH1;
            model.BH2 = p.BH2;
            model.BH3 = p.BH3;
            model.BH4 = p.BH4;
            model.MC1 = p.MC1;
            model.MC2 = p.MC2;
            model.MC3 = p.MC3;
            model.MC4 = p.MC4;
            model.KMXZ = p.KMXZ;
            model.KMLX = p.KMLX;
            model.JZRQ = p.JZRQ;
            model.MEMO = p.MEMO;

            if (p.add_or_modify == "add")
            {
                model.KMMC += p.MC1;
                if (!string.IsNullOrEmpty(p.MC2))
                {
                    model.KMMC += "|" + p.MC2;
                }
                if (!string.IsNullOrEmpty(p.MC3))
                {
                    model.KMMC += "|" + p.MC3;
                }
                if (!string.IsNullOrEmpty(p.MC4))
                {
                    model.KMMC += "|" + p.MC4;
                }
                model.KMBH += p.BH1;
                if (!string.IsNullOrEmpty(p.BH2))
                {
                    model.KMBH += p.BH2;
                }
                if (!string.IsNullOrEmpty(p.BH3))
                {
                    model.KMBH += p.BH3;
                }
                if (!string.IsNullOrEmpty(p.BH4))
                {
                    model.KMBH += p.BH4;
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
        /// <param name="KMBH">科目编号</param>
        /// <returns></returns>
        public async Task<(int code, string message)> DeleteKM(string KMBH)
        {
            var existingKM = await SugarSimple.Instance().Queryable<D_KM>().Where(u => u.KMBH == KMBH).FirstAsync();
            if (existingKM == null)
            {
                return (ApiResultCodeConst.ERROR, "科目不存在");
            }

            var deleteResult = await SugarSimple.Instance().Deleteable<D_KM>().In(KMBH).ExecuteCommandAsync();
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
        /// <param name="KMBH"></param>
        /// <returns></returns>
        public async Task<D_KM> LoadModifyKM(string KMBH)
        {
            var query = SugarSimple.Instance().Queryable<D_KM>();
            var one = await query.Where(u => u.KMBH == KMBH).FirstAsync();
            return one;
        }
    }
    
}

