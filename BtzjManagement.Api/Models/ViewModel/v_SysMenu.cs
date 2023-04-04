using System;
using System.Collections.Generic;

namespace BtzjManagement.Api.Models.ViewModel
{
    public class v_SysMenu
    {
        public int menu_id { get; set; }
        public string menu_name { get; set; }
        public int parent_id { get; set; }
        public string menu_url { get; set; }
        public string menu_icon { get; set; }
        public int menu_order { get; set; }
        public int IsEnable { get; set; }
        public List<v_SysMenu> Subs { get; set; } = new List<v_SysMenu>();
    }
}
