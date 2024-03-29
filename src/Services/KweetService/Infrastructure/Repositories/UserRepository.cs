﻿using Kwetter.Services.Common.Domain.Persistence;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the <see cref="UserRepository"/> class.
    /// </summary>
    public sealed class UserRepository : IUserRepository
    {
        private readonly KweetDbContext _kweetDbContext;

        /// <inheritdoc cref="IRepository{TAggregate}.UnitOfWork"/>
        public IUnitOfWork UnitOfWork => _kweetDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="kweetDbContext">The kweet database context.</param>
        public UserRepository(KweetDbContext kweetDbContext)
        {
            _kweetDbContext = kweetDbContext ?? throw new ArgumentNullException(nameof(kweetDbContext));
        }

        /// <inheritdoc cref="IUserRepository.Create(UserAggregate)" />
        public UserAggregate Create(UserAggregate user)
        {
            return _kweetDbContext.Users
                .Add(user).Entity;
        }

        /// <inheritdoc cref="IUserRepository.TrackKweet(Kweet)" />
        public Kweet TrackKweet(Kweet kweet)
        {
            return _kweetDbContext.Kweets.Add(kweet).Entity;
        }

        /// <inheritdoc cref="IUserRepository.FindAsync(Guid, CancellationToken)" />
        public async ValueTask<UserAggregate> FindAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _kweetDbContext.Users
                .FindAsync(new object[] { userId }, cancellationToken);
        }

        /// <inheritdoc cref="IUserRepository.FindKweetAsync(Guid, CancellationToken)" />
        public async ValueTask<Kweet> FindKweetAsync(Guid kweetId, CancellationToken cancellationToken)
        {
            return await _kweetDbContext.Kweets.FindAsync(new object[] { kweetId }, cancellationToken);
        }

        /// <inheritdoc cref="IUserRepository.FindUsersByUserNameAndTrackMentionsAsync(IEnumerable{Mention}, CancellationToken)" />
        public Task<IEnumerable<Mention>> FindUsersByUserNameAndTrackMentionsAsync(IEnumerable<Mention> mentions, CancellationToken cancellationToken)
        {
            List<Mention> returnMentions = new();
            List<UserMention> userMentions = mentions
                .Join(_kweetDbContext.Users, mention => mention.UserName, user => user.UserName, (mention, user) => new UserMention(user, mention))
                .ToList();
            foreach (UserMention item in userMentions)
            {
                Mention mention = item.Mention.ToTrackableMention(item.User);
                returnMentions.Add(_kweetDbContext.Add(mention).Entity);
            }
            return Task.FromResult(returnMentions.AsEnumerable());
        }
    }

    internal record UserMention(UserAggregate User, Mention Mention);
}
