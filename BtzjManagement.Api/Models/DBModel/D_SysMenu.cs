
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtzjManagement.Api.Models.DBModel
{
    [Table("sys_menu")]
    public class D_SysMenu
    {
        [Key]
        public int menu_id { get; set; }
        public string menu_name { get; set; }
        public int parent_id { get; set; }
        public string menu_url { get; set; }
        public string menu_icon { get; set; }
        public int menu_order { get; set; }
        public int IsEnable { get; set; }
    }
}
