using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.KweetService.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Kwetter.Services.KweetService.Tests.Aggregates
{
    [TestClass]
    public class UserAggregateTests
    {
        public UserAggregate User { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            User = new(Guid.NewGuid(), "Glovali", "supermario");
        }

        [TestMethod]
        public void Should_Create_User()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "Glovali";
            const string userName = "supermario";

            // Act
            UserAggregate user = new(userId, userDisplayName, userName);

            // Assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_User_Due_To_Empty_Id()
        {
            // Arrange
            Guid userId = Guid.Empty;
            const string userDisplayName = "Glovali";
            const string userName = "supermario";

            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => new UserAggregate(userId, userDisplayName, userName));

            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The user id is empty."));
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Kweet_Due_To_Empty_Id()
        {
            // Arrange
            Guid kweetId = Guid.Empty;
            const string message = "hello world!";

            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => User.CreateKweet(kweetId, message));

            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The kweet id is empty."));
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Kweet_Due_To_Empty_Message()
        {
            // Arrange
            Guid kweetId = Guid.NewGuid();
            const string message = "";

            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => User.CreateKweet(kweetId, message));

            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The message is null, empty or contains only whitespaces."));
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Kweet_Due_To_Message_Length_Exceeding_140_Characters()
        {
            // Arrange
            Guid kweetId = Guid.NewGuid();
            const string message = "asdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsd3434433";

            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => User.CreateKweet(kweetId, message));

            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The length of the message exceeded 140 characters."));
        }

        [TestMethod]
        public void Should_Create_Kweet()
        {
            // Arrange
            Guid kweetId = Guid.NewGuid();
            const string message = "hello world!";

            // Act
            Kweet kweet = User.CreateKweet(kweetId, message);

            // Assert
            Assert.IsNotNull(kweet);
        }

        [TestMethod]
        public void Should_Like_Kweet()
        {
            // Arrange
            Kweet kweet = User.CreateKweet(Guid.NewGuid(), "hello world");

            // Act
            bool liked = User.LikeKweet(kweet);

            // Assert
            Assert.IsTrue(liked);
        }

        [TestMethod]
        public void Should_Unlike_Kweet()
        {
            // Arrange
            Kweet kweet = User.CreateKweet(Guid.NewGuid(), "hello world");
            User.LikeKweet(kweet);

            // Act
            bool unliked = User.UnlikeKweet(kweet);

            // Assert
            Assert.IsTrue(unliked);
        }

        [TestMethod]
        public void Should_After_Liking_The_Kweet_Contain_1_Like()
        {
            // Arrange
            Kweet kweet = User.CreateKweet(Guid.NewGuid(), "hello world");

            // Act
            User.LikeKweet(kweet);

            // Assert
            Assert.IsTrue(kweet.LikeCount == 1);
        }

        [TestMethod]
        public void Should_After_Liking_And_UnLiking_The_Kweet_Contain_0_Likes()
        {
            // Arrange
            Kweet kweet = User.CreateKweet(Guid.NewGuid(), "hello world");

            // Act
            User.LikeKweet(kweet);
            User.UnlikeKweet(kweet);

            // Assert
            Assert.IsTrue(kweet.LikeCount == 0);
        }
    }
}