using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Music.Base;
using Music.Base.ExpressionHelper;
using Music.DbMigrator;
using Music.DbMigrator.Domain;
using Music.IService.Music;
using Music.Model.DTO;
using Music.Model.DTO.ExceptionDto;
using Music.Model.DTO.Music;
using Music.Model.VO.UpLoad;
using TagLib;

namespace MusicService.Music;

public class MusicInfoService : BaseService<MusicInfo,UpLoadVo>,IMusicInfoService
{
    private readonly ILogger<MusicInfoService> _logger;
    public MusicInfoService
    (
        MusicDbContext _dbContext,
        IMapper mapper,
        ILogger<MusicInfoService> _logger
        ) : base(_dbContext,mapper)
    {
        this._logger = _logger;
    }

    /// <summary>
    /// 上传更新文件
    /// </summary>
    /// <param name="vo">上传vo</param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async ValueTask<bool> upLoadMusicAsync(UpLoadVo vo)
    {
        try
        {
            var musicObj = await GetFirstOrDefultAsync(x => x.isDel == "0" && x.id == vo.id);
            if (musicObj is not null)
            {
                var mapToEntity = MapToEntity(vo);
                await AddAsync(mapToEntity);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                //获取媒体信息
                if (vo.file == null || vo.file.Length == 0)
                {
                    throw new UserFriendlyException(400, "上传文件不能为null");
                }
                var filePath = Path.Combine("D://", "upload", vo.file.FileName);
                var mapToEntity = MapToEntity(vo);
                mapToEntity.id = SnowflakeIdGenerator.getSnowId();
                mapToEntity.musicUrl = filePath;
                await AddAsync(mapToEntity);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    vo.file.CopyTo(stream);
                }
                return true;
            }
           
        }
        catch (UserFriendlyException ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// 获取文件详细信息
    /// </summary>
    /// <param name="audioFile">文件流</param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task<AudioFileInfo> getMusicInfoAsync(IFormFile audioFile)
    {
        try
        {
            AudioFileInfo audioInfo = new AudioFileInfo();
            if (audioFile == null || audioFile.Length == 0)
            {
                throw new UserFriendlyException(400,"Invalid file");
            }
            // 检查文件类型是否是 mp3 或 flac
            string fileExtension = Path.GetExtension(audioFile.FileName)?.ToLower();
            if (fileExtension != ".mp3" && fileExtension != ".flac")
            {
                throw new UserFriendlyException(400,"Unsupported file format. Please upload mp3 or flac files.");
            }
            // 在这里实现解析音频文件的逻辑，获取更多详细信息
            using (var stream = audioFile.OpenReadStream())
            {
                // 使用 TagLib# 库解析音频文件
                var file = TagLib.File.Create(new StreamFileAbstraction(audioFile.FileName, stream, stream));
                // 获取元数据信息
                audioInfo.MusicDuration = file.Properties.Duration.TotalSeconds.ToString();
                audioInfo.MusicAlbum = file.Tag.Album;
                audioInfo.MusicComposer = file.Tag.Composers.FirstOrDefault() ?? "";
                audioInfo.MusicName = audioFile.FileName;
                audioInfo.MusicStar = file.Tag.AlbumArtists.FirstOrDefault() ?? "";
                audioInfo.MusicSingerName = file.Tag.AlbumArtists.FirstOrDefault() ?? "";
                audioInfo.MusicStar = "五星";
            }
            return audioInfo;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
       
    }
    
    /// <summary>
    /// 获取音乐列表
    /// </summary>
    /// <param name="vo">获取列表vo</param>
    /// <returns></returns>
    public async Task<ResultDto<MusicInfo>> getMusicListAsync(GetMusicDto vo)
    {
        try
        {
            Expression<Func<MusicInfo, bool>> query = x => x.isDel == "0";
            if (!string.IsNullOrEmpty(vo.musicName))
            {
                query.And(x => x.musicName.Contains(vo.musicName));
            }

            var list = await GetListByPageOrderBy(query, vo.pageSize, vo.pageIndex, x => x.createTime);
            var count = await GetCountAsync(query);
            return new ResultDto<MusicInfo>()
            {
                ResultData = list,
                ResultSum = count
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
       
    }
   
    /// <summary>
    /// 删除音频信息
    /// </summary>
    /// <param name="musicId">音频主键</param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async ValueTask<bool> DeleteMusicAsync(long musicId)
    {
        try
        {
            var musicObj = await GetFirstOrDefultAsync(x => x.isDel == "0") 
                           ?? throw new UserFriendlyException(400,"音频信息不存在！");
            musicObj.isDel = "1";
            _dbContext.Update(musicObj);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
    }
    
    /// <summary>
    /// 删除音频信息
    /// </summary>
    /// <param name="musicId">音频主键</param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task<ResultDto<MusicInfo>> GetMusicAsync(long musicId)
    {
        try
        {
            var musicObj = await GetFirstOrDefultAsync(x => x.isDel == "0" && x.id == musicId) 
                           ?? throw new UserFriendlyException(400,"音频信息不存在！");
            return new ResultDto<MusicInfo>()
            {
                ResultObj = musicObj
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
    }
}