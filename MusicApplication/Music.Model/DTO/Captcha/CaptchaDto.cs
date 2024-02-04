namespace Music.Model.DTO.Captcha;

/// <summary>
/// 图片验证码
/// </summary>
public class CaptchaDto
{
    /// <summary>
    /// guid
    /// </summary>
    public string guid { get; set; }

    /// <summary>
    /// 图片字节
    /// </summary>
    public string img { get; set; }
}