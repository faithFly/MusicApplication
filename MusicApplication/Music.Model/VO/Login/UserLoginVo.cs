namespace Music.Model.VO.Login;

/// <summary>
/// 用户登录Vo
/// </summary>
public class UserLoginVo
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string userName { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string passWord { get; set; }
    /// <summary>
    /// 验证码id
    /// </summary>
    public string captCodeGuid { get; set; }
    /// <summary>
    /// 验证码
    /// </summary>
    public string captCodeValue { get; set; }
}