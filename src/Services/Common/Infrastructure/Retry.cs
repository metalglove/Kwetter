using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="Retry"/> class.
    /// Used for retrying async methods using exponential back-off.
    /// </summary>
    public static class Retry
    {
        /// <summary>
        /// Performs an actions asynchronously with exponential back-off until it is successfull.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="action">The action to perform asynchronously.</param>
        /// <param name="validateResult">The validation result.</param>
        /// <param name="maxRetries">The maximum retries the action is attempted.</param>
        /// <param name="maxDelayMilliseconds">The maximum delay in milliseconds.</param>
        /// <param name="delayMilliseconds">The startinf delay in milliseconds.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the result of the actions when it executed successfully.</returns>
        public static async Task<T> DoAsync<T>(Func<Task<T>> action,
            Func<T, bool> validateResult = null,
            int maxRetries = 10, int maxDelayMilliseconds = 2000, int delayMilliseconds = 200,
            CancellationToken cancellationToken = default)

        {
            ExponentialBackoff backoff = new(delayMilliseconds, maxDelayMilliseconds);
            List<Exception> exceptions = new();
            for (int retry = 0; retry < maxRetries; retry++)
            {
                try
                {
                    T result = await action().ConfigureAwait(false);
                    bool? isValid = validateResult?.Invoke(result);
                    if (isValid.HasValue && isValid.Value)
                        return result;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    cancellationToken.ThrowIfCancellationRequested();
                    await backoff.Delay().ConfigureAwait(false);
                }
            }
            throw new AggregateException(exceptions);
        }

        private struct ExponentialBackoff
        {
            private readonly int _delayMilliseconds;
            private readonly int _maxDelayMilliseconds;
            private int _retries;
            private int _pow;

            public ExponentialBackoff(int delayMilliseconds, int maxDelayMilliseconds)
            {
                _delayMilliseconds = delayMilliseconds;
                _maxDelayMilliseconds = maxDelayMilliseconds;
                _retries = 0;
                _pow = 1;
            }

            public Task Delay()
            {
                ++_retries;
                if (_retries < 31)
                    _pow <<= 1;

                var delay = Math.Min(_delayMilliseconds * (_pow - 1) / 2, _maxDelayMilliseconds);
                return Task.Delay(delay);
            }
        }
    }
}
