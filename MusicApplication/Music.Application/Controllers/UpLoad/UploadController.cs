using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Music.DbMigrator.Domain;
using Music.IService.Music;
using Music.Model.DTO;
using Music.Model.DTO.Music;
using Music.Model.VO.UpLoad;

namespace MusicApplication.Controllers.UpLoad;
/// <summary>
/// 文件控制器
/// </summary>
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
    /// 解析音频 返回音频详细信息
    /// </summary>
    /// <param name="file">文件流</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<AudioFileInfo> getMusicInfoAsync([FromForm] IFormFile file)
    {
        return await _service.getMusicInfoAsync(file);
    }
}   