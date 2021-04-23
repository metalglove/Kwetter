using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="IFactory{TResult}"/> interface for the Factory pattern.
    /// </summary>
    /// <typeparam name="TResult">Factory output result.</typeparam>
    public interface IFactory<out TResult>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TResult"/>.
        /// </summary>
        /// <returns>Returns a new instance of <see cref="TResult"/>.</returns>
        TResult Create();
    }

    /// <summary>
    /// Represents the <see cref="IAsyncFactory{TResult}"/> interface for the Factory pattern.
    /// </summary>
    /// <typeparam name="TResult">Factory output result.</typeparam>
    public interface IAsyncFactory<TResult>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TResult"/> asynchronously.
        /// </summary>
        /// <returns>Returns a new instance of <see cref="TResult"/> asynchronously.</returns>
        Task<TResult> CreateAsync(CancellationToken cancellationToken);
    }
}
