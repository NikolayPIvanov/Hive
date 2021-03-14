using System;
using AutoMapper;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Orders;

namespace Hive.Application.Orders.Queries.GetOrderRequirements
{
    public class RequirementDto : IMapFrom<Requirement>
    {
        public int Id { get; set; }
        
        public string Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Requirement, RequirementDto>(MemberList.Destination);
        }
    }
}