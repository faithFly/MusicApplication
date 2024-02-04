namespace Music.Model.DTO.Login;

/// <summary>
/// 登录验证返回类型
/// </summary>
public class LoginDto
{
    /// <summary>
    /// 验证token
    /// </summary>
    public string accessToken { get; set; }
    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime expireDateTime { get; set; }
    /// <summary>
    /// 用户id
    /// </summary>
    public string userId { get; set; }
}