using BX_Stock.Extension;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BX_Stock.Service
{
    /// <summary>
    /// client 連線物件
    /// </summary>
    public class BaseApiService : IBaseApiService
    {
        /// <summary>
        /// FormPost 方法
        /// </summary>
        /// <typeparam name="T">傳出物件型態</typeparam>
        /// <param name="model">傳出物件參數</param>
        /// <param name="url">url</param>
        /// <param name="headers">headers</param>
        /// <returns>傳出物件</returns>
        public T FormPost<T>(object model, string url, Dictionary<string, string> headers = null)
        {
            string json = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                this.AddHeader(headers, client);

                // form post data
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("userProfile", JsonConvert.SerializeObject(model))
                });

                HttpResponseMessage response = client.PostAsync(url, formUrlEncodedContent).Result;

                // 將回應結果內容取出並轉為 string 再透過 linqpad 輸出
                json = response.Content.ReadAsStringAsync().Result;
            }

            T result = json.ToTypedObject<T>();

            return result;
        }

        /// <summary>
        /// get 方法
        /// </summary>
        /// <typeparam name="T">回傳型態</typeparam>
        /// <param name="url">url</param>
        /// <param name="headers">header</param>
        /// <returns>回傳物件</returns>
        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            string json = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                this.AddHeader(headers, client);

                // 發出 post 並取得結果
                HttpResponseMessage response = client.GetAsync(url).Result;

                // 將回應結果內容取出並轉為 string 再透過 linqpad 輸出
                json = await response.Content.ReadAsStringAsync();
            }

            T result = json.ToTypedObject<T>();

            return result;
        }

        /// <summary>
        /// get 方法
        /// </summary>
        /// <typeparam name="T">回傳型態</typeparam>
        /// <param name="url">url</param>
        /// <param name="param">param</param>
        /// <param name="headers">header</param>
        /// <returns>回傳物件</returns>
        public async Task<T> GetAsync<T,RequestParam>(string url, RequestParam param, Dictionary<string, string> headers = null)
        {
            string json = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                this.AddHeader(headers, client);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string requestParam = param.ConvertToGetMethodUrlParam();

                // 發出 post 並取得結果
                HttpResponseMessage response = await client.GetAsync($"{url}?{requestParam}");

                // 將回應結果內容取出並轉為 string 再透過 linqpad 輸出
                json = await response.Content.ReadAsStringAsync();
            }

            T result = json.ToTypedObject<T>();

            return result;
        }

        /// <summary>
        /// post 方法
        /// </summary>
        /// <typeparam name="T">傳出物件型態</typeparam>
        /// <param name="model">傳出物件參數</param>
        /// <param name="url">url</param>
        /// <param name="headers">headers</param>
        /// <returns>傳出物件</returns>
        public T Post<T>(object model, string url, Dictionary<string, string> headers = null)
        {
            string json = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                this.AddHeader(headers, client);

                string requestModel = JsonConvert.SerializeObject(model);

                // 將轉為 string 的 json 依編碼並指定 content type 存為 httpcontent
                HttpContent contentPost = new StringContent(requestModel, Encoding.UTF8, "application/json");

                // 發出 post 並取得結果
                HttpResponseMessage response = client.PostAsync(url, contentPost).Result;

                // 將回應結果內容取出並轉為 string 再透過 linqpad 輸出
                json = response.Content.ReadAsStringAsync().Result;
            }

            T result = json.ToTypedObject<T>();

            return result;
        }

        /// <summary>
        /// 加入 http header
        /// </summary>
        /// <param name="headers">headers</param>
        /// <param name="client">client 連線物件</param>
        private void AddHeader(Dictionary<string, string> headers, HttpClient client)
        {
            if (headers == null)
            {
                return;
            }

            foreach (KeyValuePair<string, string> header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}