using Microsoft.AspNetCore.Http;
using Music.DbMigrator.Domain;
using Music.Model.DTO;
using Music.Model.DTO.Music;
using Music.Model.VO.UpLoad;

namespace Music.IService.Music;

public interface IMusicInfoService : IBaseService<MusicInfo,UpLoadVo>
{
    ValueTask<bool> upLoadMusicAsync(UpLoadVo vo);
    Task<AudioFileInfo> getMusicInfoAsync(IFormFile file);
    Task<ResultDto<MusicInfo>> getMusicListAsync(GetMusicDto vo);
    ValueTask<bool> DeleteMusicAsync(long musicId);
    Task<ResultDto<MusicInfo>> GetMusicAsync(long musicId);
}