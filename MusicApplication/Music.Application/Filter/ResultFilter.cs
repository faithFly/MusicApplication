using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Music.Model.DTO;
using Newtonsoft.Json;

namespace MusicApplication.Filter;

/// <summary>
/// 结果过滤器
/// </summary>
public class ResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        var result = new ApiResult<IActionResult>
        {
            Code = 200,
            Message = "",
            Data = context.Result
        };
        //返回结果之前
        context.Result = new ContentResult
        {
            // 返回状态码设置为200，表示成功
            StatusCode = (int)HttpStatusCode.OK,
            // 设置返回格式
            ContentType = "application/json;charset=utf-8",
            Content = JsonConvert.SerializeObject(result)
        };
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        
    }
}