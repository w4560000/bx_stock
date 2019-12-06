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
        /// <typeparam name="TModel">資料型別</typeparam>
        /// <param name="fuc">委派</param>
        /// <param name="numberOfTries">retry次數</param>
        /// <param name="delayBetweenTries">執行續等待時間</param>
        /// <returns>TModel資料</returns>
        public static TModel RetryIfThrown<TException, TModel>(Func<TModel> fuc, int numberOfTries, int delayBetweenTries) where TException : Exception
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
