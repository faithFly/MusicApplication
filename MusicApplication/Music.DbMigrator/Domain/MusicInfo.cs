using Music.Model.DTO;

namespace Music.DbMigrator.Domain;

/// <summary>
/// 音乐信息类
/// </summary>
public class MusicInfo : BaseEntity
{
    /// <summary>
    /// 歌曲名称
    /// </summary>
    public string musicName { get; set; }
    /// <summary>
    /// 歌曲专辑
    /// </summary>
    public string musicAlbum { get; set; }
    /// <summary>
    /// 星级
    /// </summary> 
    public string musicStar { get; set; }
    /// <summary>
    /// 时长
    /// </summary>
    public string musicDuration { get; set; }
    /// <summary>
    /// 歌手名称
    /// </summary>
    public string musicSingerName { get; set; }
    /// <summary>
    /// 作曲
    /// </summary>
    public string musicComposer { get; set; }
    /// <summary>
    /// 音乐地址
    /// </summary>
    public string musicUrl { get; set; }
}