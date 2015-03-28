using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    internal static class AsyncHelper
    {
        public static TResult InvokeSync<TTarget, TResult>(TTarget target, Func<TTarget, Task<TResult>> asyncMethod)
        {
            return Task.Factory.StartNew(x => InvokeAsyncConfigured((TTarget)x, asyncMethod), target, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        static async Task<TResult> InvokeAsyncConfigured<TTarget, TResult>(TTarget target, Func<TTarget, Task<TResult>> asyncMethod)
        {
            return await asyncMethod(target).ConfigureAwait(false);
        }
    }
}