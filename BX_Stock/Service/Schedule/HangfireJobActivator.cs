using System;

namespace BX_Stock.Service
{
    /// <summary>
    /// Hangfire驅動
    /// </summary>
    public class HangfireJobActivator : Hangfire.JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireJobActivator(IServiceProvider serviceProvider) => this._serviceProvider = serviceProvider;

        public override object ActivateJob(Type jobType)
        {
            return this._serviceProvider.GetService(jobType);
        }
    }
}