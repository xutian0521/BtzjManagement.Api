using SqlSugar;

namespace BtzjManagement.Api.Utils
{
    public class SugarSimple
    {
        public static SqlSugarClient Instance() 
        {
            return BaseDbContext.Instance;
        }
        
    }
}
