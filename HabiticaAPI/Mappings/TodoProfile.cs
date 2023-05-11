using AutoMapper;
using HabiticaAPI.Constants;
using HabiticaAPI.Models.Responses;
using HabiticaAPI.Models.Tags;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Mappings;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        CreateMap<string, Guid>()
            .ConvertUsing<StringToGuidConverter>();
        
        CreateMap<string, Tag>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

        CreateMap<TodoData, Todo>()
            .ForMember(x => x.TaskId, y => y.MapFrom(z => z.Id))
            .ForMember(x => x.Tags, y => y.MapFrom(z => z.Tags))
            .ForMember(x => x.TaskName, y => y.MapFrom(z => z.Text))
            .ForMember(x => x.CreationTime, y => y.MapFrom(z => z.CreatedAt));
    }
}

public class StringToGuidConverter : ITypeConverter<string, Guid>
{
    public Guid Convert(string source, Guid destination, ResolutionContext context)
    {
        return Guid.TryParse(source, out var guid) ? guid : Guid.Empty;
    }
}
