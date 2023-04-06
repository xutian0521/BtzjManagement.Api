
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BtzjManagement.Api.Models.DBModel
{
    [Table("sys_menu")]
    public class D_SysMenu
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int PId { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int SortId { get; set; }
        public int IsEnable { get; set; }
        public string Remark { get; set; }
    }
}
