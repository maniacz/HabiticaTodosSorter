using AutoMapper;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.Models.Tags;

namespace HabiticaTodosSorter.Mappings;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<GetAllTagsResponse.TagData, Tag>()
            .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
            .ForMember(x => x.Name, y => y.MapFrom(z => z.Name));
    }
}