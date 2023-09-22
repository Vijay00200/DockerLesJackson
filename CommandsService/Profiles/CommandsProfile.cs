using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // source --> Target
            CreateMap<Platform, PlatfromReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();

            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(dest=> dest.ExternalId, opt=> opt.MapFrom(src=> src.Id));
                
        }
    }
}