using AutoMapper;
using Kwetter.Services.TimelineService.API.Application.Queries;
using Kwetter.Services.TimelineService.Domain;

namespace Kwetter.Services.TimelineService.API.Application.Mapping
{
    /// <summary>
    /// Represents the <see cref="TimelineKweetProfile"/> class.
    /// </summary>
    public class TimelineKweetProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public TimelineKweetProfile()
        {
            CreateMap<KweetDto, TimelineKweet>();
            CreateMap<TimelineKweet, KweetDto>();
        }
    }
}
