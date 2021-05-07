namespace Kwetter.Services.Common.Application.CQRS
{
    /// <summary>
    /// Represents the <see cref="QueryResponse{T}"/> record.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    public sealed record QueryResponse<T> : Response where T : class
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public T Data { get; set; }
    }
}
