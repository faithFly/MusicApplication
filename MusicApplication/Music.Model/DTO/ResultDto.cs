namespace Music.Model.DTO;

public class ResultDto<T> where T : class
{
    public T ResultObj { get; set; }
    public IList<T> ResultData { get; set; }
    public int ResultCode { get; set; }
    public string ResultMsg { get; set; }
    public int ResultSum { get; set; }
}