using Music.Model.DTO.Captcha;
using Music.Model.VO;

namespace Music.IService;

public interface ICaptchaCodeService
{
     /// <summary>
     /// 获取验证码 存redis 
     /// </summary>
     /// <returns></returns>
     Task<CaptchaDto> getCaptchaCodeAsync();
     /// <summary>
     /// 获取验证码 验证
     /// </summary>
     /// <param name="vo"></param>
     /// <returns></returns>
     ValueTask<bool> GetCapchValueAsync(CaptchaVo vo);
}