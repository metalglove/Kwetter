using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Kwetter.Services.AuthorizationService.Tests.Aggregates
{
    [TestClass]
    public class IdentityAggregateTests
    {
        [TestMethod]
        public void Should_Create_Identity()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123131331";
            const string givenName = "Candy man";
            const string userName = "candyman";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            IdentityAggregate identityAggregate = new(userId, openId, givenName, userName, email, profilePictureUrl);

            // Assert
            Assert.AreEqual(userId, identityAggregate.Id);
            Assert.AreEqual(openId, identityAggregate.OpenId);
            Assert.AreEqual(givenName, identityAggregate.GivenName);
            Assert.AreEqual(userName, identityAggregate.UserName);
            Assert.AreEqual(email, identityAggregate.Email);
            Assert.AreEqual(profilePictureUrl, identityAggregate.ProfilePictureUrl);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_User_Id()
        {
            // Arrange
            Guid userId = Guid.Empty;
            const string openId = "123123131331";
            const string givenName = "Candy man";
            const string userName = "candyman";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, userName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual("The user id is empty.", exception.Message);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_Open_Id()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "";
            const string givenName = "Candy man";
            const string userName = "candyman";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, userName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual("The open id is null, empty or contains only whitespaces.", exception.Message);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_Email()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "Candy man";
            const string userName = "candyman";
            const string email = "";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, userName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual("The email is null, empty or contains only whitespaces.", exception.Message);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Invalid_Email()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "Candy man";
            const string userName = "candyman";
            const string email = "@";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, userName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual("The email is not a valid email address.", exception.Message);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_GivenName()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "";
            const string userName = "candyman";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, userName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual("The given name is null, empty or contains only whitespaces.", exception.Message);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_UserName()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "Candy Man";
            const string userName = "";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, userName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual("The user name is null, empty or contains only whitespaces.", exception.Message);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_UserName_Length_Exceeding_32_Characters()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "Candy Man";
            const string userName = "dsfsddfdsfdsfddsfsdfdsfdsfdsfsfsdsffdfsfdsfsdfsd";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, userName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual("The user name length exceeded the maximum length of 32.", exception.Message);
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_UserName_Not_Being_Alphanumeric()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "Candy Man";
            const string userName = "';/'3";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, userName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual("The user name is not alphanumeric.", exception.Message);
        }
    }
}
