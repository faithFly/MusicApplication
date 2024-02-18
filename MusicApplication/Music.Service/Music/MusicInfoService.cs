using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
    private readonly IConfiguration _config;
    public MusicInfoService
    (
        MusicDbContext _dbContext,
        IMapper mapper,
        ILogger<MusicInfoService> _logger,
        IConfiguration _config
        ) : base(_dbContext,mapper)
    {
        this._config = _config;
        this._logger = _logger;
    }

    /// <summary>
    /// 上传更新文件
    /// </summary>
    /// <param name="vo">上传vo</param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async ValueTask<bool> upDateMusicAsync(UpLoadVo vo)
    {
        try
        {
            var musicObj = await GetFirstOrDefultAsync(x => x.isDel == "0" && x.id == vo.id);
            if (musicObj is not null)
            {
                //存在更新
                var mapToEntity = MapToEntity(vo);
                return await UpdateAsync(mapToEntity);
            }
            else
            {
                //不存在新增
                var mapToEntity = MapToEntity(vo);
                mapToEntity.id = SnowflakeIdGenerator.getSnowId();
                return await AddAsync(mapToEntity);
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
            //将音频文件存储到本地 返回url地址
            var uploadUrl = _config["UploadUrl"].ToString();
            var filePath = Path.Combine(uploadUrl, audioFile.FileName);
            // 确保目录存在，如果不存在则创建
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                audioFile.CopyTo(fs);
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
                audioInfo.MusicStar = "5";
                audioInfo.MusicUrl = filePath;
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
            Expression<Func<MusicInfo, bool>> query = x => x.isDel == "0"
            && (string.IsNullOrEmpty(vo.musicName) || x.musicName.Contains(vo.musicName))
            && (string.IsNullOrEmpty(vo.singerName) || x.musicSingerName.Contains(vo.singerName));

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
            throw new UserFriendlyException(400,e.ToString());
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