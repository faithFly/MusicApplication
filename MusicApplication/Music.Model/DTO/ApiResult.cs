﻿namespace Music.Model.DTO;

public class ApiResult<T>
{
    public int Code { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}