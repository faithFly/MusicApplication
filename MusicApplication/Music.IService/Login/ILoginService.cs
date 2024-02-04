using Music.DbMigrator.Domain;
using Music.Model.DTO.Login;
using Music.Model.VO.Login;

namespace Music.IService.Login;

public interface ILoginService
{
    /// <summary>
    /// 验证登录
    /// </summary>
    /// <param name="vo">验证vo</param>
    /// <returns></returns>
    Task<LoginDto> UserLoginTaskAsync(UserLoginVo vo);

    /// <summary>
    /// 获取用户
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <returns></returns>
    Task<FaithUser> GetUserAsync(long userId);
}