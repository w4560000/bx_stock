using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BX_Stock
{
    /// <summary>
    /// 全域變數
    /// </summary>
    public class GlobalParam
    {
        /// <summary>
        /// 資料庫連線字串
        /// </summary>
        public static string DbConnection => "Server=bxsqlserver.database.windows.net;Database=bxstock;User Id=bingxiang;Password=Aa334567;"; //AzureHelper.GetAzureKeyVault("test");
    }
}
