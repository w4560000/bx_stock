using BX_Stock.Enum;
using BX_Stock.Extension;
using BX_Stock.Helper;
using BX_Stock.Models;
using FubonNeo.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace BX_Stock.Service.SDK
{
    public class Fobun
    {
        private FubonSDK sdk = null;

        private readonly SemaphoreSlim _connectLock = new SemaphoreSlim(1, 1); // lock連線流程
        private readonly ManualResetEventSlim _canTrade = new ManualResetEventSlim(true); // true = 可以下單


        /// <summary>
        /// Log
        /// </summary>
        private readonly ILogger<Fobun> _logger;

        /// <summary>
        /// 是否為正式環境
        /// </summary>
        private bool _isProd { get; set; }

        /// <summary>
        /// 連線主機名稱
        /// </summary>
        private string _serverName { get; set; }

        public Fobun(ILogger<Fobun> logger)
        {
            this._logger = logger;
            this._isProd = ConfigHelper.AppSetting.Env == ConfigHelper.ProdEnv;
            this._serverName = this._isProd ? "正式" : "測試" + "主機";
        }

        public async Task SDKInit()
        {
            // todo 待測
            await RetryHelper.RetryIfThrown<Exception, Fobun>(() => ConnectAsync(), 10, 500, _logger);
        }

        private async Task ConnectAsync()
        {
            try
            {
                // 暫停新下單
                _canTrade.Reset();

                // lock
                await _connectLock.WaitAsync();

                var setting = ConfigHelper.AppSetting.FobunSetting;

                _logger.LogInformation($"FobunSDK {_serverName} | 開始連線");

                sdk = _isProd ? new FubonSDK() : // 正式機
                                new FubonSDK(300, 3, "wss://neoapitest.fbs.com.tw/TASP/XCPXWS");    // 測試機

                _logger.LogInformation($"FobunSDK {_serverName} | 成功連線");

                var result = sdk.Login(setting.UserId, setting.UserPwd, setting.CAPath);
                if (!result.isSuccess)
                    throw new Exception($"FobunSDK {_serverName} | 登入異常, {result.message}");

                _logger.LogInformation($"FobunSDK {_serverName} | {setting.UserId} 成功登入");

                sdk.InitRealtime();
                EventInit();

                _logger.LogInformation($"FobunSDK {_serverName} | 初始化成功");

                // 連線完成，允許交易
                _canTrade.Set(); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"FobunSDK {_serverName} | 初始化SDK異常, Error: {ex}");
                throw;
            }
            finally
            {
                _connectLock.Release();
            }
        }

        private void EventInit()
        {
            // 事件通知
            sdk.OnEvent += async (code, msg) =>
            {
                this._logger.LogInformation($"FobunSDK {_serverName} | OnEvent [事件通知] 代碼:" + code + " | 內容:" + msg);
                if (code == ((int)FobunEventEnum.斷線).ToString())
                {
                    this._logger.LogInformation($"FobunSDK {_serverName} | 偵測到斷線（代碼{(int)FobunEventEnum.斷線}） | 啟動自動重連");

                    try
                    {
                        sdk.Logout();
                    }
                    catch (Exception e)
                    {
                        this._logger.LogInformation($"FobunSDK {_serverName} | 自動重連 登出異常, Error: {e}");
                    }

                    await SDKInit();
                }
            };

            // todo 委託回報
            sdk.OnOrder += async (code, ordeResult) =>
            {
                Console.WriteLine(code + ordeResult.ToString());
            };

            // todo 改價/改量/刪單回報
            sdk.OnOrderChanged += async (code, ordeResult) =>
            {
                Console.WriteLine(code + ordeResult.ToString());
            };

            // todo 成交回報
            sdk.OnFilled += async (code, filledData) =>
            {
                Console.WriteLine(code + filledData.ToString());
            };
        }

        // todo 下單
        //public async Task<OrderResult> PlaceOrderAsync(OrderRequest request)
        //{
        //    if (!_canTrade.IsSet)
        //        throw new Exception("交易暫停，正在重連");

        //    // 使用 sdk 下單
        //    return await Task.Run(() => sdk.PlaceOrder(request));
        //}
    }
}