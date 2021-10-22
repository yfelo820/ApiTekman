using Api.Entities.Content;
using Api.DTO.Backoffice;
using AutoMapper;

public class MappingProfile : Profile {
    public MappingProfile() {
        // Add as many of these lines as you need to map your objects
        CreateMap<Activity, ActivityDTO>();
        CreateMap<ActivityDTO, Activity>();
        CreateMap<ProblemResolution, ProblemResolutionDTO>();
        CreateMap<ProblemResolutionDTO, ProblemResolution>();
    }
}