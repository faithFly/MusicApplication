namespace Music.Model.DTO.Music;

public class GetMusicDto
{
    public int pageIndex { get; set; }
    public int pageSize { get; set; }
    public string musicName { get; set; }
    public string singerName { get; set; }
}