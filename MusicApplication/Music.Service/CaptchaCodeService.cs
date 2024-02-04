using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Music.IService;
using Music.Model.DTO.Captcha;
using Music.Model.DTO.ExceptionDto;
using Music.Model.VO;
using Music.Redis;
using StackExchange.Redis;

namespace MusicService;

public class CaptchaCodeService:ICaptchaCodeService
{
    private readonly string CAPTCHAKEY = "captcha:";
    private IDatabase _redis;
    public CaptchaCodeService(RedisService redisService)
    {
        _redis = redisService.GetDatabase();
    }
    /// <summary>
    /// 获取图片验证码
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<CaptchaDto> getCaptchaCodeAsync()
    {
        CaptchaDto captcha = new CaptchaDto();
        string verificationCode = GenerateVerificationCode();
        captcha.guid =await SaveInRedisCode(verificationCode);
        captcha.img = Convert.ToBase64String(CreateValidateGraphic(verificationCode));
        return captcha;
    }

    #region 生成验证码通用方法
    /// <summary>
    /// 生成随机的验证码
    /// </summary>
    /// <param name="len"></param>
    /// <returns></returns>
    private string GenerateVerificationCode(int len = 4)
    {
        return CreateRandomCode(len);
    }
    /// <summary>
    /// 生成随机的字符串
    /// </summary>
    /// <param name="codeCount"></param>
    /// <returns></returns>
    public string CreateRandomCode(int codeCount)
    {
        string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,a,b,c,d,e,f,g,h,i,g,k,l,m,n,o,p,q,r,F,G,H,I,G,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,s,t,u,v,w,x,y,z";
        string[] allCharArray = allChar.Split(',');
        string randomCode = "";
        int temp = -1;
        Random rand = new Random();
        for (int i = 0; i < codeCount; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            }
            int t = rand.Next(35);
            if (temp == t)
            {
                return CreateRandomCode(codeCount);
            }
            temp = t;
            randomCode += allCharArray[t];
        }
        return randomCode;
    }

    /// <summary>
    /// 将验证码存储到redis中 (5分钟过期时间)
    /// </summary>
    /// <param name="verificationCode"></param>
    /// <returns></returns>
    public async Task<string> SaveInRedisCode(string verificationCode)
    {
        string key = CAPTCHAKEY + Guid.NewGuid().ToString();
        var stringSetAsync = await _redis.StringSetAsync(key, verificationCode,TimeSpan.FromMinutes(5));
        return key;
    }

    /// <summary>
    /// 创建验证码图片
    /// </summary>
    /// <param name="validateCode"></param>
    /// <returns></returns>
    public byte[] CreateValidateGraphic(string validateCode)
    {
        Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 16.0), 27);
        Graphics g = Graphics.FromImage(image);
        try
        {
            //生成随机生成器
            Random random = new Random();
            //清空图片背景色
            g.Clear(Color.White);
            //画图片的干扰线
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
            }
            Font font = new Font("Arial", 13, (FontStyle.Bold | FontStyle.Italic));
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
            g.DrawString(validateCode, font, brush, 3, 2);

            //画图片的前景干扰线
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

            //保存图片数据
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);

            //输出图片流
            return stream.ToArray();
        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
    }
    #endregion

    /// <summary>
    /// 验证验证码
    /// </summary>
    /// <param name="vo"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>

    public async ValueTask<bool> GetCapchValueAsync(CaptchaVo vo)
    {
        try
        {
            var value = await _redis.StringGetAsync(vo.guid);
            if (string.IsNullOrEmpty(value))
            {
                throw new UserFriendlyException(404, "验证码不存在或已过期");
            }
            return vo.code.ToUpper() == value.ToString().ToUpper();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }
}