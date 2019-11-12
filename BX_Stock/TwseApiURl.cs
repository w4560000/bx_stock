namespace BX_Stock
{
    /// <summary>
    /// 台灣證交所API Url對應
    /// </summary>
    public static class TwseApiUrl
    {
        /// <summary>
        /// 個股查詢
        /// </summary>
        public static string StockDay => "https://www.twse.com.tw/exchangeReport/STOCK_DAY";

        /// <summary>
        /// 取得現有上市股票代號
        /// </summary>
        public static string GetAllListedStockNo => "https://isin.twse.com.tw/isin/C_public.jsp?strMode=2";

        /// <summary>
        /// 取得現有上櫃股票代號
        /// </summary>
        public static string GetAllCabinetStockNo => "https://isin.twse.com.tw/isin/C_public.jsp?strMode=4";
    }
}