using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Kwetter.Services.AuthorizationService.Domain.Exceptions;

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
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            IdentityAggregate identityAggregate = new(userId, openId, givenName, email, profilePictureUrl);

            // Assert
            Assert.AreEqual(userId, identityAggregate.Id);
            Assert.AreEqual(openId, identityAggregate.OpenId);
            Assert.AreEqual(givenName, identityAggregate.GivenName);
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
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual(exception.Message, "The user id is empty.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_Open_Id()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "";
            const string givenName = "Candy man";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual(exception.Message, "The open id is null, empty or contains only whitespaces.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_Email()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "Candy man";
            const string email = "";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual(exception.Message, "The email is null, empty or contains only whitespaces.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Invalid_Email()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "Candy man";
            const string email = "@";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual(exception.Message, "The email is not a valid email address.");
        }

        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_GivenName()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string openId = "123123";
            const string givenName = "";
            const string email = "candy.man@mail.kwetter";
            const string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual(exception.Message, "The given name is null, empty or contains only whitespaces.");
        }
    }
}
