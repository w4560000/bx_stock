namespace BX_Stock
{
    /// <summary>
    /// 個股API Url對應
    /// </summary>
    public static class StockApiUrl
    {
        /// <summary>
        /// 取得上市個股歷史資料
        /// </summary>
        public static string TwseStockDay = "https://www.twse.com.tw/exchangeReport/STOCK_DAY";

        /// <summary>
        /// 取得上市個股最新日資料
        /// </summary>
        public static string TwseStockNewDay = "https://openapi.twse.com.tw/v1/exchangeReport/STOCK_DAY_ALL?response=json";

        /// <summary>
        /// 取得上櫃個股歷史資料
        /// </summary>
        public static string TpexStockDay = "https://www.tpex.org.tw/www/zh-tw/afterTrading/tradingStock";

        /// <summary>
        /// 取得上櫃個股最新日資料
        /// </summary>
        public static string TpexStockNewDay = "https://www.tpex.org.tw/openapi/v1/tpex_mainboard_daily_close_quotes?response=json";


        /// <summary>
        /// 取得現有上市股票代號
        /// </summary>
        public static string GetAllListedStockNo = "https://isin.twse.com.tw/isin/C_public.jsp?strMode=2";

        /// <summary>
        /// 取得現有上櫃股票代號
        /// </summary>
        public static string GetAllCabinetStockNo = "https://isin.twse.com.tw/isin/C_public.jsp?strMode=4";
    }
}