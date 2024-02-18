namespace Music.Model.DTO.Music;

public class GetMusicDto
{
    public int pageIndex { get; set; }
    public int pageSize { get; set; }
    /// <summary>
    /// 音乐名称
    /// </summary>
    public string musicName { get; set; }
    /// <summary>
    /// 音乐作者
    /// </summary>
    public string singerName { get; set; }
}