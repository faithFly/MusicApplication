using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Music.DbMigrator.Domain;
using Music.IService.Music;
using Music.Model.DTO;
using Music.Model.DTO.Music;
using Music.Model.VO.UpLoad;

namespace MusicApplication.Controllers.UpLoad;

[Route("api/[controller]")]
[ApiController]
public class UploadController
{
    private readonly IMusicInfoService _service; 
    private readonly IMapper _mapper;
    public UploadController(
        IMusicInfoService _service
        )
    {
        this._service = _service;
    }
    /// <summary>
    /// 上传文件到服务器 信息入库
    /// </summary>
    /// <param name="vo">上传vo</param>
    /// <returns></returns>
    [HttpPut]
    public async ValueTask<bool> upLoadMusicAsync([FromForm]UpLoadVo vo)
    {
        return await _service.upLoadMusicAsync(vo);
    }
    /// <summary>
    /// 解析音频 返回音频详细信息
    /// </summary>
    /// <param name="file">文件流</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<object> getMusicInfoAsync([FromForm] IFormFile file)
    {
        return await _service.getMusicInfoAsync(file);
    }

    /// <summary>
    /// 获取音频列表
    /// </summary>
    /// <param name="vo">获取列表vo</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ResultDto<MusicInfo>> getMusicListAsync([FromBody]GetMusicDto vo)
    {
        return await _service.getMusicListAsync(vo);
    }
    
    /// <summary>
    /// 删除音频
    /// </summary>
    /// <param name="musicId">音频主键</param>
    /// <returns></returns>
    [HttpDelete("{musicId}")]
    public async ValueTask<bool> DeleteMusicAsync([FromRoute]long musicId)
    {
        return await _service.DeleteMusicAsync(musicId);
    }
    
    /// <summary>
    /// 获取音频详情
    /// </summary>
    /// <param name="musicId">音频主键</param>
    /// <returns></returns>
    [HttpGet("{musicId}")]
    public async Task<ResultDto<MusicInfo>> GetMusicAsync([FromRoute]long musicId)
    {
        return await _service.GetMusicAsync(musicId);
    }
}