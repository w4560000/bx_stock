using Hangfire;
using System;
using System.Linq.Expressions;

namespace BX_Stock.Service
{
    /// <summary>
    /// Hangfire排程
    /// </summary>
    public class TaskProvider : ITaskProvider
    {
        /// <summary>
        /// 排程時間固定設定在台灣時區
        /// </summary>
        private readonly TimeZoneInfo TaiwanTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");

        /// <summary>
        /// 將指定之方法置入立即排程工作
        /// </summary>
        /// <param name="method">日標方法</param>
        public void QueueTask(Expression<Action> method)
        {
            BackgroundJob.Enqueue(method);
        }

        /// <summary>
        /// 將指定介面之方法置入立即排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        public void QueueTask<T>(Expression<Action<T>> method)
        {
            BackgroundJob.Enqueue(method);
        }

        /// <summary>
        /// 將指定介面之方法置入立即排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        public void QueueTask<T>(Expression<Func<T, System.Threading.Tasks.Task>> method)
        {
            BackgroundJob.Enqueue(method);
        }

        /// <summary>
        /// 將指定之方法置入立即排程工作
        /// </summary>
        /// <param name="method">日標方法</param>
        public void QueueTask(Expression<Func<System.Threading.Tasks.Task>> method)
        {
            BackgroundJob.Enqueue(method);
        }

        /// <summary>
        /// 將指定介面之方法設定定時排程
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        /// <param name="method">日標方法</param>
        /// <param name="cronExpression">cron expression</param>
        /// <param name="priority">執行優先等級</param>
        public void RecurringTask<T>(string jobId, Expression<Action<T>> method, string cronExpression, TaskPriorityEnum priority = TaskPriorityEnum.@default)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            RecurringJob.AddOrUpdate(jobId, method, cronExpression, this.TaiwanTimeZoneInfo, priority.ToString());
        }

        /// <summary>
        /// 將指定之方法設定定時排程
        /// </summary>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        /// <param name="method">日標方法</param>
        /// <param name="cronExpression">cron expression</param>
        /// <param name="priority">執行優先等級</param>
        public void RecurringTask(string jobId, Expression<Action> method, string cronExpression, TaskPriorityEnum priority = TaskPriorityEnum.@default)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            RecurringJob.AddOrUpdate(jobId, method, cronExpression, this.TaiwanTimeZoneInfo, priority.ToString());
        }

        /// <summary>
        /// 將指定介面之方法設定定時排程
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        /// <param name="method">日標方法</param>
        /// <param name="cronExpression">cron expression</param>
        /// <param name="priority">執行優先等級</param>
        public void RecurringTask<T>(string jobId, Expression<Func<T, System.Threading.Tasks.Task>> method, string cronExpression, TaskPriorityEnum priority = TaskPriorityEnum.@default)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            RecurringJob.AddOrUpdate(jobId, method, cronExpression, this.TaiwanTimeZoneInfo, priority.ToString());
        }

        /// <summary>
        /// 將指定之方法設定定時排程
        /// </summary>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        /// <param name="method">日標方法</param>
        /// <param name="cronExpression">cron expression</param>
        /// <param name="priority">執行優先等級</param>
        public void RecurringTask(string jobId, Expression<Func<System.Threading.Tasks.Task>> method, string cronExpression, TaskPriorityEnum priority = TaskPriorityEnum.@default)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            RecurringJob.AddOrUpdate(jobId, method, cronExpression, this.TaiwanTimeZoneInfo, priority.ToString());
        }

        /// <summary>
        /// 刪除指定 jobId 之排程工作
        /// </summary>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        public void RemoveRecurringTask(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        /// <summary>
        /// 將指定介面之方法置入延時排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        /// <param name="timeSpan">延後時間</param>
        public void ScheduleTask<T>(Expression<Action<T>> method, TimeSpan timeSpan)
        {
            BackgroundJob.Schedule(method, timeSpan);
        }

        /// <summary>
        /// 將指定介面之方法置入特定時間排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        /// <param name="datetime">指定時間</param>
        public void ScheduleTask<T>(Expression<Action<T>> method, DateTime datetime)
        {
            DateTimeOffset offset = new DateTimeOffset(datetime.ToUniversalTime());
            BackgroundJob.Schedule(method, offset);
        }

        /// <summary>
        /// 將指定介面之方法置入延時排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        /// <param name="timeSpan">延後時間</param>
        public void ScheduleTask<T>(Expression<Func<T, System.Threading.Tasks.Task>> method, TimeSpan timeSpan)
        {
            BackgroundJob.Schedule(method, timeSpan);
        }

        /// <summary>
        /// 將指定介面之方法置入特定時間排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        /// <param name="datetime">指定時間</param>
        public void ScheduleTask<T>(Expression<Func<T, System.Threading.Tasks.Task>> method, DateTime datetime)
        {
            DateTimeOffset offset = new DateTimeOffset(datetime.ToUniversalTime());
            BackgroundJob.Schedule(method, offset);
        }

        /// <summary>
        /// 立即執行指定 jobId 之排程工作
        /// </summary>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        public void TriggerRecurringJob(string jobId)
        {
            RecurringJob.Trigger(jobId);
        }
    }
}