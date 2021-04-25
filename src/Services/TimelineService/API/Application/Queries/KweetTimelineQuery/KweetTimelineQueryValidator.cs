using FluentValidation;

namespace Kwetter.Services.TimelineService.API.Application.Queries.KweetTimelineQuery
{
    /// <summary>
    /// Represents the <see cref="KweetTimelineQueryValidator"/> class.
    /// </summary>
    public sealed class KweetTimelineQueryValidator : AbstractValidator<KweetTimelineQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KweetTimelineQueryValidator"/> class.
        /// </summary>
        public KweetTimelineQueryValidator()
        {
            RuleFor(kweetTimelineQuery => kweetTimelineQuery.UserId)
                .NotEmpty()
                .WithMessage("The user id cannot be empty.");
        }
    }
}
