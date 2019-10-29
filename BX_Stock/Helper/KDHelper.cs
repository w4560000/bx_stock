using System;
using System.Collections;

namespace BX_Stock.Helper
{
    public class KDHelper
    {
        /// <summary>
        /// 取KD值_K值
        /// </summary>
        /// <param name="ary"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public string CalcKD_K(ArrayList ary, int rownum, int p)
        {
            //int p = 90;
            if (rownum + 1 - p < 0)
            {
                return "";
            }

            /*
                        第n天收盤價-最近n天內最低價
             RSV ＝────────────────────────────────── × 100
                      最近n天內最高價-最近n天內最低價

             計算出RSV之後，再來計算K值與D值。
             當日K值(%K)= 2/3 前一日 K值 + 1/3 RSV
             當日D值(%D)= 2/3 前一日 D值 + 1/3 當日K值
             */
            string val;
            string beforeK;
            // 取得前一天K值
            Hashtable map = (Hashtable)ary[rownum - 1];
            if (map["KD_9K"].ToString() == "")
            {
                beforeK = "50";
            }
            else
            {
                beforeK = map["KD_9K"].ToString();
            }

            // 取當天收盤價
            map = (Hashtable)ary[rownum];
            string nowClose = map["CLOSE_PRICE"].ToString();

            //取最近n天內最低價
            ArrayList aryMinPrice = new ArrayList();
            for (int i = rownum; i > rownum - p; i--)
            {
                map = (Hashtable)ary[i];
                aryMinPrice.Add(double.Parse(map["LOW_PRICE"].ToString()));
            }
            aryMinPrice.Sort();
            string minPrice = aryMinPrice[0].ToString();

            //取最近n天內最高價
            ArrayList aryMaxPrice = new ArrayList();
            for (int i = rownum; i > rownum - p; i--)
            {
                map = (Hashtable)ary[i];
                aryMaxPrice.Add(double.Parse(map["HIGH_PRICE"].ToString()));
            }
            aryMaxPrice.Sort();
            string maxPrice = aryMaxPrice[aryMaxPrice.Count - 1].ToString();

            double RSV;
            if (maxPrice != minPrice)
            {
                RSV = (Convert.ToDouble(nowClose) - Convert.ToDouble(minPrice)) / (Convert.ToDouble(maxPrice) - Convert.ToDouble(minPrice)) * 100;
            }
            else
            {
                RSV = 0;
            }

            val = ((Convert.ToDouble(beforeK) * 2 / 3) + (RSV / 3)).ToString();
            val = Math.Round(Convert.ToDouble(val), 2).ToString();

            return val;
        }

        /// <summary>
        /// 取KD值_D值
        /// </summary>
        /// <param name="ary"></param>
        /// <param name="rownum"></param>
        /// <param name="nowK"></param>
        /// <returns></returns>
        public string CalcKD_D(ArrayList ary, int rownum, string nowK, int p)
        {
            //int p = 90;
            if (rownum + 1 - p < 0)
            {
                return "";
            }

            /*
             當日D值(%D)= 2/3 前一日 D值＋ 1/3 當日K值
             */
            string val = "";
            string beforeD = "";
            // 取得前一天D值
            Hashtable map = (Hashtable)ary[rownum - 1];
            if (map["KD_9D"].ToString() == "")
            {
                beforeD = "50";
            }
            else
            {
                beforeD = map["KD_9D"].ToString();
            }

            val = ((Convert.ToDouble(beforeD) * 2 / 3) + (Convert.ToDouble(nowK) / 3)).ToString();
            val = Math.Round(Convert.ToDouble(val), 2).ToString();

            return val;
        }
    }
}