using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.FollowService.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Kwetter.Services.FollowService.Tests.Aggregates
{
    [TestClass]
    public class UserAggregateTests
    {
        [TestMethod]
        public void Should_Create_User()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "HELLOMAN";
            const string userName = "someone";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            UserAggregate userAggregate = new(userId, userDisplayName, userName, userProfilePictureUrl);

            // Assert
            Assert.IsNotNull(userAggregate);
        }

        [TestMethod]
        public void Should_Follow_User()
        {
            // Arrange
            UserAggregate userAggregate = new(Guid.NewGuid(), "SuperMario", "supermario", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            UserAggregate otherUserAggregate = new(Guid.NewGuid(), "candyman67", "canyman", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            
            // Act
            bool followed = userAggregate.Follow(otherUserAggregate);

            // Assert
            Assert.IsTrue(followed);
        }
        
        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_UserId()
        {
            // Arrange
            Guid userId = Guid.Empty;
            const string userDisplayName = "HelloMan";
            const string userName = "someone";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => new UserAggregate(userId, userDisplayName, userName, userProfilePictureUrl));
            
            // Assert
            Assert.AreEqual(exception.Message, "The user id is empty.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_UserDisplayName()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "";
            const string userName = "someone";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => new UserAggregate(userId, userDisplayName, userName, userProfilePictureUrl));

            // Assert
            Assert.AreEqual(exception.Message, "The user display name is null, empty or contains only whitespaces.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_UserProfilePictureUrl()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "sdfgsDSD";
            const string userName = "someone";
            const string userProfilePictureUrl = "";

            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => new UserAggregate(userId, userDisplayName, userName, userProfilePictureUrl));

            // Assert
            Assert.AreEqual(exception.Message, "The user profile picture url is null, empty or contains only whitespaces.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Following_Due_To_Same_Follower_And_Following_Ids()
        {
            // Arrange
            UserAggregate userAggregate = new(Guid.NewGuid(), "SuperMario", "sdaddds", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");

            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => userAggregate.Follow(userAggregate));

            // Assert
            Assert.AreEqual(exception.Message, "The user to be followed can not be the same user.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Following_Due_To_Null()
        {
            // Arrange
            UserAggregate userAggregate = new(Guid.NewGuid(), "SuperMario", "dsdsdsd", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");

            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => userAggregate.Follow(null));

            // Assert
            Assert.AreEqual(exception.Message, "The user to be followed can not be null.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Unfollowing_Due_To_Same_Follower_And_Following_Ids()
        {
            // Arrange
            UserAggregate userAggregate = new(Guid.NewGuid(), "SuperMario", "dsdsdds", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");

            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => userAggregate.Unfollow(userAggregate));

            // Assert
            Assert.AreEqual(exception.Message, "The user to be unfollowed can not be the same user.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Unfollowing_Due_To_Null()
        {
            // Arrange
            UserAggregate userAggregate = new(Guid.NewGuid(), "SuperMario", "dsdsds", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");

            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => userAggregate.Unfollow(null));

            // Assert
            Assert.AreEqual(exception.Message, "The user to be unfollowed can not be null.");
        }
        
        [TestMethod]
        public void Should_Unfollow_User()
        {
            // Arrange
            UserAggregate userAggregate = new(Guid.NewGuid(), "SuperMario", "supermario", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            UserAggregate otherUserAggregate = new(Guid.NewGuid(), "candyman67", "candyman", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            userAggregate.Follow(otherUserAggregate);

            // Act
            bool unfollowed = userAggregate.Unfollow(otherUserAggregate);
            
            // Assert
            Assert.IsTrue(unfollowed);
        }
        
        [TestMethod]
        public void Should_Fail_To_Unfollow_Twice_In_Same_Context()
        {
            // Arrange
            UserAggregate userAggregate = new(Guid.NewGuid(), "SuperMario", "supermario", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            UserAggregate otherUserAggregate = new(Guid.NewGuid(), "candyman67", "candyman", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            userAggregate.Follow(otherUserAggregate);
            userAggregate.Unfollow(otherUserAggregate);

            // Act
            bool unfollowed = userAggregate.Unfollow(otherUserAggregate);

            // Assert
            Assert.IsFalse(unfollowed);
        }
    }
}