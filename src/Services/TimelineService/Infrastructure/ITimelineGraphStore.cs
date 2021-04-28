using Kwetter.Services.TimelineService.Domain;
using System;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="ITimelineGraphStore"/> interface.
    /// </summary>
    public interface ITimelineGraphStore
    {
        /// <summary>
        /// Creates the follow asynchronously.
        /// </summary>
        /// <param name="follow">The follow.</param>
        /// <returns>Returns a boolean indicating whether the follow is created.</returns>
        public Task<bool> CreateFollowerAsync(Follow follow);

        /// <summary>
        /// Creates the kweet asynchronously.
        /// </summary>
        /// <param name="kweet">The kweet.</param>
        /// <returns>Returns a boolean indicating whether the kweet is created.</returns>
        public Task<bool> CreateKweetAsync(Kweet kweet);

        /// <summary>
        /// Creates the kweet like asynchronously.
        /// </summary>
        /// <param name="kweetLike">The kweet like.</param>
        /// <returns>Returns a boolean indicating whether the kweet like is created.</returns>
        public Task<bool> CreateKweetLikeAsync(KweetLike kweetLike);

        /// <summary>
        /// Creates the user asynchronously.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Returns a boolean indicating whether the user is created.</returns>
        public Task<bool> CreateUserAsync(User user);

        /// <summary>
        /// Creates the user profile asynchronously.
        /// </summary>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>Returns a boolean indicating whether the user profile is created.</returns>
        public Task<bool> CreateUserProfileAsync(UserProfile userProfile);

        /// <summary>
        /// Gets the timeline for a specific user id using pagination asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns a timeline.</returns>
        public Task<Timeline> GetPaginatedTimelineAsync(Guid userId, int pageNumber, int pageSize);
    }
   
}
