using AutoMapper;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Objects;

namespace Hive.Gig.Application.GigScopes
{
    public class GigScopesProfile : Profile
    {
        public GigScopesProfile()
        {
            CreateMap<GigScope, GigScopeDto>();
        }
    }
}