using System;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;
using Kwetter.Services.FollowService.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwetter.Services.FollowService.Tests.Aggregates
{
    [TestClass]
    public class FollowAggregateTests
    {
        [TestMethod]
        public void Should_Follow_User()
        {
            // Arrange
            Guid followingId = Guid.NewGuid();
            Guid followerId = Guid.NewGuid();
            
            // Act
            FollowAggregate followAggregate = new(followingId, followerId);
            
            // Assert
            Assert.AreEqual(followingId, followAggregate.FollowingId);
            Assert.AreEqual(followerId, followAggregate.FollowerId);
        }
        
        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_Following_Id()
        {
            // Arrange
            Guid followingId = Guid.Empty;
            Guid followerId = Guid.NewGuid();
            
            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => new FollowAggregate(followingId, followerId));
            
            // Assert
            Assert.AreEqual(exception.Message, "The following id is empty.");
        }
        
        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Empty_Follower_Id()
        {
            // Arrange
            Guid followingId = Guid.NewGuid();
            Guid followerId = Guid.Empty;
            
            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => new FollowAggregate(followingId, followerId));
            
            // Assert
            Assert.AreEqual(exception.Message, "The follower id is empty.");
        }
        
        [TestMethod]
        public void Should_Throw_Exception_While_Constructing_Due_To_Same_Follower_And_Following_Ids()
        {
            // Arrange
            Guid followingId = Guid.NewGuid();
            Guid followerId = followingId;
            
            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => new FollowAggregate(followingId, followerId));
            
            // Assert
            Assert.AreEqual(exception.Message, "The follow and following id are the same. One can not follow themself.");
        }
        
        [TestMethod]
        public void Should_Unfollow_User()
        {
            // Arrange
            Guid followingId = Guid.NewGuid();
            Guid followerId = Guid.NewGuid();
            FollowAggregate followAggregate = new(followingId, followerId);
            
            // Act
            bool unfollowed = followAggregate.Unfollow();
            
            // Assert
            Assert.IsTrue(unfollowed);
        }
        
        [TestMethod]
        public void Should_Throw_Exception_When_Unfollowing_Twice_In_Same_Context()
        {
            // Arrange
            Guid followingId = Guid.NewGuid();
            Guid followerId = Guid.NewGuid();
            FollowAggregate followAggregate = new(followingId, followerId);
            followAggregate.Unfollow();
            
            // Act
            FollowDomainException exception = Assert.ThrowsException<FollowDomainException>(() => followAggregate.Unfollow());
            
            // Assert
            Assert.AreSame(exception.Message, "The following is already unfollowed.");
        }
    }
}