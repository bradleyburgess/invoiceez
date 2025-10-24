using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs;

public class ApiResponse<T>
{
    public ApiResponseCode Code { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, string[]>? FormErrors { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data, string? message = "Success")
        => new ApiResponse<T>
        {
            Code = ApiResponseCode.Ok,
            Message = message,
            Data = data
        };

    public static ApiResponse<object> Ok(string message = "Success")
        => new ApiResponse<object>
        {
            Code = ApiResponseCode.Ok,
            Message = message,
            Data = null
        };

    public static ApiResponse<T> Fail(
        ApiResponseCode code,
        string? message,
        Dictionary<string, string[]>? formErrors = null
    )
        => new ApiResponse<T>
        {
            Code = code,
            Message = message,
            FormErrors = formErrors
        };

}

public class EmptyDto { }