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
            string openId = "123123131331";
            string givenName = "Candy man";
            string email = "candy.man@mail.kwetter";
            string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

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
            string openId = "123123131331";
            string givenName = "Candy man";
            string email = "candy.man@mail.kwetter";
            string profilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            AuthorizationDomainException exception = Assert.ThrowsException<AuthorizationDomainException>(() => new IdentityAggregate(userId, openId, givenName, email, profilePictureUrl));

            // Assert
            Assert.AreEqual(exception.Message, "The user id is empty.");
        }
    }
}
