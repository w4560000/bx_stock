using System;
using System.Linq.Expressions;

namespace BX_Stock.Service
{
    public interface ITaskProvider
    {
        /// <summary>
        /// 將指定之方法置入立即排程工作
        /// </summary>
        /// <param name="method">日標方法</param>
        void QueueTask(Expression<Action> method);

        /// <summary>
        /// 將指定介面之方法置入立即排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        void QueueTask<T>(Expression<Action<T>> method);

        /// <summary>
        /// 將指定之方法置入立即排程工作
        /// </summary>
        /// <param name="method">日標方法</param>
        void QueueTask(Expression<Func<System.Threading.Tasks.Task>> method);

        /// <summary>
        /// 將指定介面之方法置入立即排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        void QueueTask<T>(Expression<Func<T, System.Threading.Tasks.Task>> method);

        /// <summary>
        /// 將指定介面之方法設定定時排程
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        /// <param name="method">日標方法</param>
        /// <param name="cronExpression">CRON expression</param>
        /// <param name="priority">執行優先等級</param>
        void RecurringTask<T>(string jobId, Expression<Action<T>> method, string cronExpression, TaskPriorityEnum priority = TaskPriorityEnum.@default);

        /// <summary>
        /// 將指定之方法設定定時排程
        /// </summary>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        /// <param name="method">日標方法</param>
        /// <param name="cronExpression">CRON expression</param>
        /// <param name="priority">執行優先等級</param>
        void RecurringTask(string jobId, Expression<Action> method, string cronExpression, TaskPriorityEnum priority = TaskPriorityEnum.@default);

        /// <summary>
        /// 將指定介面之方法設定定時排程
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        /// <param name="method">日標方法</param>
        /// <param name="cronExpression">CRON expression</param>
        /// <param name="priority">執行優先等級</param>
        void RecurringTask<T>(string jobId, Expression<Func<T, System.Threading.Tasks.Task>> method, string cronExpression, TaskPriorityEnum priority = TaskPriorityEnum.@default);

        /// <summary>
        /// 將指定之方法設定定時排程
        /// </summary>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        /// <param name="method">日標方法</param>
        /// <param name="cronExpression">CRON expression</param>
        /// <param name="priority">執行優先等級</param>
        void RecurringTask(string jobId, Expression<Func<System.Threading.Tasks.Task>> method, string cronExpression, TaskPriorityEnum priority = TaskPriorityEnum.@default);

        /// <summary>
        /// 刪除指定 jobId 之排程工作
        /// </summary>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        void RemoveRecurringTask(string jobId);

        /// <summary>
        /// 將指定介面之方法置入延時排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        /// <param name="timeSpan">延後時間</param>
        void ScheduleTask<T>(Expression<Action<T>> method, TimeSpan timeSpan);

        /// <summary>
        /// 將指定介面之方法置入特定時間排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        /// <param name="datetime">指定時間</param>
        void ScheduleTask<T>(Expression<Action<T>> method, DateTime datetime);

        /// <summary>
        /// 將指定介面之方法置入延時排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        /// <param name="timeSpan">延後時間</param>
        void ScheduleTask<T>(Expression<Func<T, System.Threading.Tasks.Task>> method, TimeSpan timeSpan);

        /// <summary>
        /// 將指定介面之方法置入特定時間排程工作
        /// </summary>
        /// <typeparam name="T">服務介面</typeparam>
        /// <param name="method">日標方法</param>
        /// <param name="datetime">指定時間</param>
        void ScheduleTask<T>(Expression<Func<T, System.Threading.Tasks.Task>> method, DateTime datetime);

        /// <summary>
        /// 立即執行指定 jobId 之排程工作
        /// </summary>
        /// <param name="jobId">定時排程編號，可透過該編號，對定時排程做異動</param>
        void TriggerRecurringJob(string jobId);
    }
}