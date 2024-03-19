public class GlobalResponse<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string[] Errors { get; set; }

    public GlobalResponse()
    {
        IsSuccess = false;
        Errors = new string[0];
        Data = default!;
    }

    public static GlobalResponse<T> Success(T data)
    {
        return new GlobalResponse<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static GlobalResponse<T> Success()
    {
        return new GlobalResponse<T>
        {
            IsSuccess = true
        };
    }

    public static GlobalResponse<T> Failure(string[] errors)
    {
        return new GlobalResponse<T>
        {
            Errors = errors
        };
    }
}