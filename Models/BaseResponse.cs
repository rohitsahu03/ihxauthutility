namespace AuthUtility.Models {
    public class BaseResponse {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class BaseResponseAuth
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}