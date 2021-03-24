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
}
