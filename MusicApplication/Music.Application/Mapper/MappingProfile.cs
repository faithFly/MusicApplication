using AutoMapper;
using Music.DbMigrator.Domain;
using Music.Model.VO.UpLoad;

namespace MusicApplication.Mapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<UpLoadVo, MusicInfo>();
        // Add other mappings as needed
    }
}