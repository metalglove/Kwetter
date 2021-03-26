using System;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;
using Kwetter.Services.KweetService.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwetter.Services.KweetService.Tests.Aggregates
{
    [TestClass]
    public class KweetAggregateTests
    {
        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Kweet_Due_To_Empty_Message()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string message = "";
            
            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() =>  new KweetAggregate(userId, message));
            
            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The message is null, empty or contains only whitespaces."));
        }
        
        
        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Kweet_Due_To_Message_Length_Exceeding_140_Characters()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string message = "asdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsdfsadfasdfdasdfsd3434433";
            
            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() =>  new KweetAggregate(userId, message));
            
            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The length of the message exceeded 140 characters."));
        }
        
        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Kweet_Due_To_User_Id_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.Empty;
            const string message = "ssdsds";
            
            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() =>  new KweetAggregate(userId, message));
            
            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The user id is empty."));
        }
        
        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_KweetLike_Due_To_User_Id_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.Empty;
            KweetAggregate kweetAggregate = new(Guid.NewGuid(), "My kweet!");
            
            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => kweetAggregate.AddLike(userId));
            
            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The user id is empty."));
        }
        
        [TestMethod]
        public void Should_Like_Kweet()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            KweetAggregate kweetAggregate = new(Guid.NewGuid(), "My kweet!");
            
            // Act
            bool liked = kweetAggregate.AddLike(userId);

            // Assert
            Assert.IsTrue(liked);
        }
        
        [TestMethod]
        public void Should_After_Liking_The_Kweet_Contain_1_Likes()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            KweetAggregate kweetAggregate = new(Guid.NewGuid(), "My kweet!");
            
            // Act
            kweetAggregate.AddLike(userId);

            // Assert
            Assert.IsTrue(kweetAggregate.Likes.Count == 1);
        }
        
        [TestMethod]
        public void Should_Throw_Exception_While_Removing_KweetLike_Due_To_User_Id_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.Empty;
            KweetAggregate kweetAggregate = new(Guid.NewGuid(), "My kweet!");
            
            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => kweetAggregate.RemoveLike(userId));
            
            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The user id is empty."));
        }
        
        [TestMethod]
        public void Should_Unlike_Kweet()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            KweetAggregate kweetAggregate = new(Guid.NewGuid(), "My kweet!");
            kweetAggregate.AddLike(userId);
            
            // Act
            bool removed = kweetAggregate.RemoveLike(userId);
            
            // Assert
            Assert.IsTrue(removed);
        }
        
        [TestMethod]
        public void Should_After_Liking_And_UnLiking_The_Kweet_Contain_0_Likes()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            KweetAggregate kweetAggregate = new(Guid.NewGuid(), "My kweet!");
            kweetAggregate.AddLike(userId);
            
            // Act
            kweetAggregate.AddLike(userId);
            kweetAggregate.RemoveLike(userId);

            // Assert
            Assert.IsTrue(kweetAggregate.Likes.Count == 0);
        }
        
        [TestMethod]
        public void Should_Fail_To_Unlike_Kweet_Because_User_Did_Not_Like_Kweet_Before()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            KweetAggregate kweetAggregate = new(Guid.NewGuid(), "My kweet!");
            
            // Act
            bool removed = kweetAggregate.RemoveLike(userId);
            
            // Assert
            Assert.IsFalse(removed);
        }
    }
}