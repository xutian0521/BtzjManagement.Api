using System;
using System.Collections.Generic;

namespace BtzjManagement.Api.Models.QueryModel
{
	public class P_SettingRoleMenu
	{
        public int roleId { get; set; }
        public List<int> menuIds { get; set; }
    }
}

