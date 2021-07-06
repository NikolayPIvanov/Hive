﻿using AutoMapper;
using Hive.Gig.Contracts.Objects;
using Hive.Gig.Domain.Entities;

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