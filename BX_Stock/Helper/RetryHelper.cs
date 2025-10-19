using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BX_Stock.Helper
{
    public static class RetryHelper
    {
        /// <summary>
        /// retry-pattern
        /// </summary>
        /// <typeparam name="TException">例外情況</typeparam>
        /// <param name="fuc">委派</param>
        /// <param name="numberOfTries">retry次數</param>
        /// <param name="delayBetweenTries">執行續等待時間</param>
        /// <returns>TModel資料</returns>
        public static async Task RetryIfThrown<TException, T>(Func<Task> func, int numberOfTries, int delayBetweenTries, ILogger<T> logger) where TException : Exception
        {
            TException lastException = null;

            for (var currentTry = 1; currentTry <= numberOfTries; currentTry++)
            {
                try
                {
                    await func();
                    return;
                }
                catch (TException e)
                {
                    logger.LogError($"RetryIfThrown 執行失敗, 次數: {currentTry}, 總次數: {numberOfTries}");
                    lastException = e;
                }
                Thread.Sleep(delayBetweenTries);
            }

            if (lastException != null)
                throw lastException;

            throw new Exception("No exception to rethrow");
        }

        /// <summary>
        /// retry-pattern
        /// </summary>
        /// <typeparam name="TException">例外情況</typeparam>
        /// <typeparam name="TModel">資料型別</typeparam>
        /// <param name="fuc">委派</param>
        /// <param name="numberOfTries">retry次數</param>
        /// <param name="delayBetweenTries">執行續等待時間</param>
        /// <returns>TModel資料</returns>
        public static TModel RetryIfThrown<TException, TModel, T>(Func<TModel> fuc, int numberOfTries, int delayBetweenTries, ILogger<T> logger) where TException : Exception
        {
            TException lastException = null;

            for (var currentTry = 1; currentTry <= numberOfTries; currentTry++)
            {
                try
                {
                    return fuc();
                }
                catch (TException e)
                {
                    logger.LogError($"RetryIfThrown 執行失敗, 次數: {currentTry}, 總次數: {numberOfTries}");
                    lastException = e;
                }
                Thread.Sleep(delayBetweenTries);
            }

            if (lastException != null)
                throw lastException;

            throw new Exception("No exception to rethrow");
        }
    }
}
