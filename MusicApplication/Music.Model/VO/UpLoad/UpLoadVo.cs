using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Music.Model.VO.UpLoad;

public class UpLoadVo
{
    public long id { get; set; }
    /// <summary>
    /// 歌曲名称
    /// </summary>
    [Required(ErrorMessage = "musicName is required.")]
    public string musicName { get; set; }
    /// <summary>
    /// 歌曲专辑
    /// </summary>
    [Required(ErrorMessage = "musicAlbum is required.")]
    public string musicAlbum { get; set; }
    /// <summary>
    /// 星级
    /// </summary>
    public string musicStar { get; set; }
    /// <summary>
    /// 时长
    /// </summary>
    [Required(ErrorMessage = "musicDuration is required.")]
    public string musicDuration { get; set; }
    /// <summary>
    /// 歌手名称
    /// </summary>
    [Required(ErrorMessage = "musicSingerName is required.")]
    public string musicSingerName { get; set; }
    /// <summary>
    /// 作曲
    /// </summary>
    public string musicComposer { get; set; }
    /// <summary>
    /// 音频地址
    /// </summary>
    [Required(ErrorMessage = "musicUrl is required.")]
    public string musicUrl { get; set; }
}