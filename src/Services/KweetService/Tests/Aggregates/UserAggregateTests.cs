using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.KweetService.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.Tests.Aggregates
{
    [TestClass]
    public class UserAggregateTests
    {
        public UserAggregate User { get; set; }
        private List<UserAggregate> ExistingUsers { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            User = new(Guid.NewGuid(), "Glovali", "supermario", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            ExistingUsers = new List<UserAggregate>()
            {
                new UserAggregate(Guid.NewGuid(), "Omegaguy1", "helloguy1", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg"),
                new UserAggregate(Guid.NewGuid(), "Megaaaa", "megaman2", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg"),
                new UserAggregate(Guid.NewGuid(), "WAIKOOO", "waiko", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg"),
                new UserAggregate(Guid.NewGuid(), "SUPERMAN", "superman", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg"),
                new UserAggregate(Guid.NewGuid(), "PIKAAAAA", "pikachu", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg"),
                User
            };
        }

        private Task<IEnumerable<Mention>> FindUsersByUserNameAndTrackMentionsAsync(IEnumerable<Mention> userNames, CancellationToken ct)
        {
            List<Mention> returnMentions = new();
            var result = ExistingUsers.Join(userNames, u => u.UserName, mention => mention.UserName, (user, mention) => new { user, mention }).ToList();
            foreach (var item in result)
            {
                Mention mention = item.mention;
                mention.User = item.user;
                returnMentions.Add(mention);
            }
            return Task.FromResult(returnMentions.AsEnumerable());
        }

        [TestMethod]
        public void Should_Create_User()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "Glovali";
            const string userName = "supermario";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            UserAggregate user = new(userId, userDisplayName, userName, userProfilePictureUrl);

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
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";

            // Act
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => new UserAggregate(userId, userDisplayName, userName, userProfilePictureUrl));

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
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => User.CreateKweetAsync(kweetId, message, FindUsersByUserNameAndTrackMentionsAsync).GetAwaiter().GetResult());

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
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => User.CreateKweetAsync(kweetId, message, FindUsersByUserNameAndTrackMentionsAsync).GetAwaiter().GetResult());

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
            KweetDomainException kweetDomainException = Assert.ThrowsException<KweetDomainException>(() => User.CreateKweetAsync(kweetId, message, FindUsersByUserNameAndTrackMentionsAsync).GetAwaiter().GetResult());

            // Assert
            Assert.IsTrue(kweetDomainException.Message.Equals("The length of the message exceeded 140 characters."));
        }

        [TestMethod]
        public async Task Should_Create_Kweet()
        {
            // Arrange
            Guid kweetId = Guid.NewGuid();
            const string message = "hello world!";

            // Act
            Kweet kweet = await User.CreateKweetAsync(kweetId, message, FindUsersByUserNameAndTrackMentionsAsync);

            // Assert
            Assert.IsNotNull(kweet);
        }

        [TestMethod]
        public async Task Should_Like_Kweet()
        {
            // Arrange
            Kweet kweet = await User.CreateKweetAsync(Guid.NewGuid(), "hello world", FindUsersByUserNameAndTrackMentionsAsync);

            // Act
            bool liked = User.LikeKweet(kweet);

            // Assert
            Assert.IsTrue(liked);
        }

        [TestMethod]
        public async Task Should_Unlike_Kweet()
        {
            // Arrange
            Kweet kweet = await User.CreateKweetAsync(Guid.NewGuid(), "hello world", FindUsersByUserNameAndTrackMentionsAsync);
            User.LikeKweet(kweet);

            // Act
            bool unliked = User.UnlikeKweet(kweet);

            // Assert
            Assert.IsTrue(unliked);
        }

        [TestMethod]
        public async Task Should_After_Liking_The_Kweet_Contain_1_Like()
        {
            // Arrange
            Kweet kweet = await User.CreateKweetAsync(Guid.NewGuid(), "hello world", FindUsersByUserNameAndTrackMentionsAsync);

            // Act
            User.LikeKweet(kweet);

            // Assert
            Assert.IsTrue(kweet.LikeCount == 1);
        }

        [TestMethod]
        public async Task Should_After_Liking_And_UnLiking_The_Kweet_Contain_0_Likes()
        {
            // Arrange
            Kweet kweet = await User.CreateKweetAsync(Guid.NewGuid(), "hello world", FindUsersByUserNameAndTrackMentionsAsync);

            // Act
            User.LikeKweet(kweet);
            User.UnlikeKweet(kweet);

            // Assert
            Assert.IsTrue(kweet.LikeCount == 0);
        }

        [TestMethod]
        public async Task Should_Mention_User_Glovali_With_UserName_supermario_When_Creating_Kweet()
        {
            // Arrange & Act
            const string message = "Hello @supermario!";

            // Act
            Kweet kweet = await User.CreateKweetAsync(Guid.NewGuid(), message, FindUsersByUserNameAndTrackMentionsAsync);

            // Assert
            Assert.IsTrue(kweet.Mentions.Contains(new Mention(User.UserName, kweet)));
        }

        [TestMethod]
        public async Task Should_Tag_Fun_When_Creating_Kweet()
        {
            // Arrange & Act
            const string fun = "#Fun";
            string message = $"Programming {fun}!";
            Guid kweetId = Guid.NewGuid();
            HashTag expectedTag = new("#fun", kweetId);

            // Act
            Kweet kweet = await User.CreateKweetAsync(kweetId, message, FindUsersByUserNameAndTrackMentionsAsync);

            // Assert
            Assert.IsTrue(kweet.HashTags.Contains(expectedTag));
        }
    }
}