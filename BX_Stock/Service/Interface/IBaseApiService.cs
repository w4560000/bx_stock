using System.Collections.Generic;

namespace BX_Stock.Service
{
    public interface IBaseApiService
    {
        /// <summary>
        /// form post 方法
        /// </summary>
        /// <typeparam name="T">傳出物件型態</typeparam>
        /// <param name="model">傳出物件參數</param>
        /// <param name="url">url</param>
        /// <param name="headers">headers</param>
        /// <returns>回傳物件</returns>
        T FormPost<T>(object model, string url, Dictionary<string, string> headers = null);

        /// <summary>
        /// get 方法
        /// </summary>
        /// <typeparam name="T">回傳型態</typeparam>
        /// <param name="url">url</param>
        /// <param name="headers">header</param>
        /// <returns>回傳物件</returns>
        public T Get<T>(string url, Dictionary<string, string> headers = null);

        /// <summary>
        /// get 方法
        /// </summary>
        /// <typeparam name="T">回傳型態</typeparam>
        /// <typeparam name="RequestParam">Url傳入參數型態</typeparam>
        /// <param name="url">url</param>
        /// <param name="param">param</param>
        /// <param name="headers">header</param>
        /// <returns>回傳物件</returns>
        public T Get<T, RequestParam>(string url, RequestParam param, Dictionary<string, string> headers = null);

        /// <summary>
        /// post 方法
        /// </summary>
        /// <typeparam name="T">傳出物件型態</typeparam>
        /// <param name="model">傳出物件參數</param>
        /// <param name="url">url</param>
        /// <param name="headers">headers</param>
        /// <returns>傳出物件</returns>
        T Post<T>(object model, string url, Dictionary<string, string> headers = null);
    }
}