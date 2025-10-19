using BX_Stock.Models;

namespace BX_Stock.Extension
{
    public static class ConfigHelper
    {
        /// <summary>
        /// 設定物件
        /// </summary>
        public static readonly AppSetting AppSetting = new AppSetting();

        public static readonly string ProdEnv = "Prod";
    }
}
