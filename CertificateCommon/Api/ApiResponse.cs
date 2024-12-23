namespace CertificateCommon.Api
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } // Indicates if the request was successful

        public T? Data { get; set; } // Holds the data if successful

        public string? ErrorMessage { get; set; } // Holds the error message if any
        public string? Message { get; set; } // Holds the success message if any

        public ApiErrorType? ErrorType { get; set; } // Holds the error type (optional, can be used for debugging)

        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
        {
            return new ApiResponse<T> { Success = true, Data = data, Message = message };
        }

        public static ApiResponse<T> ErrorResponse(string errorMessage, ApiErrorType errorType)
        {
            return new ApiResponse<T> { Success = false, ErrorMessage = errorMessage, ErrorType = errorType };
        }
    }

    public enum ApiErrorType
    {
        None,
        ValidationError,
        NotFound,
        AlreadyExists,
        ServerError,
        Unauthorized,
        Forbidden,
        InternalServerError
    }
}
