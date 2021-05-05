using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.UserService.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Kwetter.Services.UserService.Tests.Aggregates
{
    [TestClass]
    public class UserAggregateTests
    {
        [TestMethod]
        public void Should_Create_UserAggregate_With_UserCreatedDomainEvent()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string displayName = "Mario";
            const string userName = "supermario";
            const string profileDescription = "Hello everyone!";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            UserAggregate userAggregate = new(userId, displayName, userName, profileDescription, profilePictureUrl);

            // Assert
            Assert.IsTrue(userAggregate.DomainEvents.OfType<UserCreatedDomainEvent>().Count() == 1);
        }

        [TestMethod]
        public void Should_Throw_UserDomainException_With_Empty_DisplayName()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string displayName = "";
            const string userName = "supermario";
            const string profileDescription = "Hello everyone!";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act & Assert
            UserDomainException userDomainException = Assert.ThrowsException<UserDomainException>(() => new UserAggregate(userId, displayName, userName, profileDescription, profilePictureUrl));
            Assert.IsTrue(userDomainException.Message.Equals("The display name is null, empty or contains only whitespaces."));
        }

        [TestMethod]
        public void Should_Throw_UserDomainException_With_DisplayName_Exceeding_64_Length()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string displayName = "Hello super long display name that exceeds sixtyfour characters oh no!";
            const string userName = "supermario";
            const string profileDescription = "Hello everyone!";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            UserDomainException userDomainException = Assert.ThrowsException<UserDomainException>(() => new UserAggregate(userId, displayName, userName, profileDescription, profilePictureUrl));
            
            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The length of the display name exceeded 64 characters."));
        }

        [TestMethod]
        public void Should_Throw_UserDomainException_With_UserProfileDescription_Exceeding_512_Length()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string displayName = "Cool display name";
            const string userName = "supermario";
            const string profileDescription = "phdddNnnghhgeh5vkn4ycAoPgjjdAyTggggnDriKHEDGUhl0eOZzAdfJNk300F8klYe2hfffUTHIXhZF8gtYxqAWIt7lsdsGKYtdNH7zhyF399DqpNOOKUvIhfoGugnY48bWqMqGCHy1BZznramSn1TW007JQXALfYYdTBo9W3k3Fnk8dlfvhCuCU8FLlcEr8VvQfp8s6PGS6HiZH6hGBTOInbTPtZJmRSxO1UYZd0gw2u0HLbKE5Jk2R4Jx2i2f1Ga3f1yeFXRoYecDDFJ6Ff9hflSqhdRcnUyjjRatGC8Fu6G4WHLuWbmwr32thlc7PlHKeGEhdqT3TDQdIDKSZY89NhktDVXhzstS4RJnfaIwx2gDMVfiwbtcLAyTkGYKG5AMr3jVbAwsiWuOIewGHbCbUXRsp82IYpKWF2nywOjqmssUt29wKCJRdau6W2gTRON54pPbYS3lnDXojGwSN8pKGx068J4PFkosnIiE2eEQkeXF0GK0Vubk5yOjoOqhocfoLnWMnEQNZdrM0YWdo0CKWWdZgW3kZTxszdtpE0LXOEhPoo";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            UserDomainException userDomainException = Assert.ThrowsException<UserDomainException>(() => new UserAggregate(userId, displayName, userName, profileDescription, profilePictureUrl));
            
            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The length of the profile description exceeded 512."));
        }

        [TestMethod]
        public void Should_Throw_UserDomainException_With_UserProfilePictureUrl_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string displayName = "Cool display name";
            const string userName = "supermario";
            const string profileDescription = "sssssssssssssssssssssssssss";
            const string profilePictureUrl = "";

            // Act
            UserDomainException userDomainException = Assert.ThrowsException<UserDomainException>(() => new UserAggregate(userId, displayName, userName, profileDescription, profilePictureUrl));

            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The profile picture url is null, empty or contains only white spaces."));
        }

        [TestMethod]
        public void Should_Throw_UserDomainException_With_UserName_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string displayName = "Cool display name";
            const string userName = "";
            const string profileDescription = "sssssssssssssssssssssssssss";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            UserDomainException userDomainException = Assert.ThrowsException<UserDomainException>(() => new UserAggregate(userId, displayName, userName, profileDescription, profilePictureUrl));

            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The user name is null, empty or contains only whitespaces."));
        }

        [TestMethod]
        public void Should_Throw_UserDomainException_With_UserName_Length_Exceeded_32_Characters()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string displayName = "Cool display name";
            const string userName = "fsdffsdsdffsdfsdfdsfsdfdsfsdfsdfsdfsfdsfdsfsfsfffds";
            const string profileDescription = "sssssssssssssssssssssssssss";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            UserDomainException userDomainException = Assert.ThrowsException<UserDomainException>(() => new UserAggregate(userId, displayName, userName, profileDescription, profilePictureUrl));

            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The length of the user name exceeded 32 characters."));
        }

        [TestMethod]
        public void Should_Throw_UserDomainException_With_UserName_Being_Non_Alphanumeric()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string displayName = "Cool display name";
            const string userName = "sdfsf[][';/";
            const string profileDescription = "sssssssssssssssssssssssssss";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            UserDomainException userDomainException = Assert.ThrowsException<UserDomainException>(() => new UserAggregate(userId, displayName, userName, profileDescription, profilePictureUrl));

            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The user name is not alphanumeric."));
        }
    }
}
