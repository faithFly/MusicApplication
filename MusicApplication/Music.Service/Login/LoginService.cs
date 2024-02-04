using System.Linq.Expressions;
using AutoMapper;
using Music.Base;
using Music.Base.JWT;
using Music.DbMigrator;
using Music.DbMigrator.Domain;
using Music.IService;
using Music.IService.Login;
using Music.Model.DTO.ExceptionDto;
using Music.Model.DTO.Login;
using Music.Model.VO;
using Music.Model.VO.Login;
namespace MusicService.Login;

public class LoginService : BaseService<FaithUser,string>,ILoginService
{
    private readonly ICaptchaCodeService _captchaCodeService;
    private readonly JWTHelper _jwtHelper;
    public LoginService(
        ICaptchaCodeService _captchaCodeService,
        JWTHelper jwtHelper
        ,MusicDbContext _dbContext,
        IMapper mapper) : base(_dbContext,mapper)
    {
        this._captchaCodeService = _captchaCodeService;
        this._jwtHelper = jwtHelper;
    }

    public async Task<LoginDto> UserLoginTaskAsync(UserLoginVo vo)
    {
        try
        {
            if (string.IsNullOrEmpty(vo.passWord))throw new UserFriendlyException(400, "密码选项不能为空");
            //判断验证码
            CaptchaVo captchaVo = new CaptchaVo()
            {
                guid = vo.captCodeGuid,
                code = vo.captCodeValue
            };
            if (!await _captchaCodeService.GetCapchValueAsync(captchaVo)){throw new UserFriendlyException(400, "验证码验证失败");}
            //密码md5加密
            var passMd5 = MD5Helper.GetMD5Hash(vo.passWord);
            //验证成功判断账号密码
            Expression<Func<FaithUser, bool>> query = x => 
                (string.IsNullOrEmpty(vo.userName)||x.userName == vo.userName) && 
                x.passWord == passMd5 && x.isdel == 0;
            var user = await GetFirstOrDefultAsync(query) ?? throw new UserFriendlyException(404,"用户不存在！");
            //生成token
            var accessObj = _jwtHelper.CreatToken(user.id.ToString(), user.userName);
            return new LoginDto()
            {
                accessToken = accessObj.token,
                expireDateTime = accessObj.expireDateTime,
                userId = user.id.ToString()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// 获取用户
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<FaithUser> GetUserAsync(long userId)
    {
        try
        {
            Expression<Func<FaithUser, bool>> query = x =>
                x.isdel == 0 && x.id == userId;
            return await GetFirstOrDefultAsync(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}