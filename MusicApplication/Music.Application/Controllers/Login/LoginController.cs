using Microsoft.AspNetCore.Mvc;
using Music.DbMigrator.Domain;
using Music.IService;
using Music.IService.Login;
using Music.Model.DTO.Captcha;
using Music.Model.DTO.Login;
using Music.Model.VO.Login;

namespace MusicApplication.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController
{
    private readonly ICaptchaCodeService _captchaCodeService;
    private readonly ILoginService _loginService;
    public LoginController(
        ICaptchaCodeService captchaCodeService,
        ILoginService loginService
    )
    {
        _captchaCodeService = captchaCodeService;
        _loginService = loginService;
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<CaptchaDto> getCaptchaCodeAsync()
    {
        return await _captchaCodeService.getCaptchaCodeAsync();
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<LoginDto> userLoginAsync(UserLoginVo vo)
    {
        return await _loginService.UserLoginTaskAsync(vo);
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<FaithUser> GetUserAsync(long userId)
    {
        return await _loginService.GetUserAsync(userId);
    }
}