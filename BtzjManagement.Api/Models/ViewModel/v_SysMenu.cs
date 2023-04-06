using System;
using System.Collections.Generic;

namespace BtzjManagement.Api.Models.ViewModel
{
    public class v_SysMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int PId { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int SortId { get; set; }
        public int IsEnable { get; set; }
        public string Remark { get; set; }
        public List<v_SysMenu> Subs { get; set; } = new List<v_SysMenu>();
    }
}
