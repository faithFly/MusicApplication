using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Music.Model.DTO;
using Music.Model.DTO.ExceptionDto;
using Newtonsoft.Json;

namespace MusicApplication.Filter;

/// <summary>
/// 异常过滤器
/// </summary>
public class ExceptionFilter: ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is UserFriendlyException ex)
        {
            ResultDto<object> obj = new ResultDto<object>
            {
                ResultCode = ex.Code,
                ResultMsg = ex.Message
            };
            context.Result = new OkObjectResult(obj);
            context.ExceptionHandled = true;
        }
        //如果异常没有被处理
        if (context.ExceptionHandled == false)
        {
            //定义返回信息
            ResultDto<object> res = new ResultDto<object>();
            res.ResultMsg = "发生错误请联系管理员"+context.Exception.Message;
            res.ResultCode = 500;
            _logger.LogError(context.HttpContext.Request.Path, context.Exception);
            context.Result = new ContentResult
            {
                StatusCode = 500,
                ContentType = "application/json;charset=utf-8",
                Content = JsonConvert.SerializeObject(res)
            };
        }
        context.ExceptionHandled = true;
    }
}