namespace Music.Model.DTO.ExceptionDto;

public class UserFriendlyException: Exception
{
    public UserFriendlyException()
    {
    }
    public int Code { get; set; }
    public UserFriendlyException(int code, string message)
        : base(message)
    {
        Code = code;
    }
}