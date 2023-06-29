using System.Collections.Generic;

namespace BtzjManagement.Api.Models.ViewModel
{
    /// <summary>
    /// 凭证模板vm
    /// </summary>
    public class v_VoucherTemplate
    {
        public List<string> Headers { get; set; }
        public List<Dictionary<string, object>> Rows { get; set; }
    }
}
