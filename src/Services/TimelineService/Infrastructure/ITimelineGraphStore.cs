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
        /// Deletes the follow asynchronously.
        /// </summary>
        /// <param name="followerId">The follower id.</param>
        /// <param name="followingId">The following id.</param>
        /// <returns>Returns a boolean indicating whether the follow is deleted.</returns>
        public Task<bool> DeleteFollowerAsync(Guid followerId, Guid followingId);

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
        /// Deletes the kweet like asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="kweetId">The kweet id.</param>
        /// <returns>Returns a boolean indicating whether the kweet like is deleted.</returns>
        public Task<bool> DeleteKweetLikeAsync(Guid userId, Guid kweetId);

        /// <summary>
        /// Creates the user asynchronously.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Returns a boolean indicating whether the user is created.</returns>
        public Task<bool> CreateUserAsync(User user);

        /// <summary>
        /// Updates the user display name asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userDisplayName">The user display name.</param>
        /// <returns>Returns a boolean indicating whether the user display name is updated.</returns>
        public Task<bool> UpdateUserDisplayNameAsync(Guid userId, string userDisplayName);

        /// <summary>
        /// Updates the user profile picture url asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userProfilePictureUrl">The user profile picture url.</param>
        /// <returns>Returns a boolean indicating whether the user profile picture url is updated.</returns>
        public Task<bool> UpdateUserProfilePictureUrlAsync(Guid userId, string userProfilePictureUrl);

        /// <summary>
        /// Gets the timeline for a specific user id using pagination asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>Returns a timeline.</returns>
        public Task<Timeline> GetPaginatedTimelineAsync(Guid userId, uint pageNumber, uint pageSize);
    }
}
