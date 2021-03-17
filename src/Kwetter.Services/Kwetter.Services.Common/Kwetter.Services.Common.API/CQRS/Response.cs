using System.Collections.Generic;

namespace Kwetter.Services.Common.API.CQRS
{
    /// <summary>
    /// Represents the <see cref="Response"/> record.
    /// </summary>
    public abstract record Response
    {
        /// <summary>
        /// Gets or sets the execution result success.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Gets or sets the error messages in case of an execution failure.
        /// </summary>
        public List<string> Errors { get; init; } = new();
    }
}
