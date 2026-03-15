namespace ShortnerUrl.Api.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }

    public static ApiResponse<T> Ok(T data) => new() { IsSuccess = true, Data = data };
}
