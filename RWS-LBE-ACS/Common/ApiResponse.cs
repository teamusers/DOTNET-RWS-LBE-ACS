 
using System.Text.Json.Serialization;

namespace RWS_LBE_ACS.Common
{
    /// <summary>
    /// Standard envelope for API responses.
    /// The Data field contains the payload, which varies by endpoint.
    /// </summary>
    public class ApiResponse<T>
    {
        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

    /// <summary>
    /// Non-generic helpers for producing common ApiResponse&lt;object&gt; instances.
    /// </summary>
    public static class ApiResponse
    {
        public static ApiResponse<object> DefaultResponse(long code, string message) =>
            new ApiResponse<object> { Code = code, Message = message, Data = null };

        public static ApiResponse<object> InternalErrorResponse() =>
            DefaultResponse(Codes.INTERNAL_ERROR, "internal error");

        public static ApiResponse<object> InvalidRequestBodyErrorResponse() =>
            DefaultResponse(Codes.INVALID_REQUEST_BODY, "invalid json request body");

        public static ApiResponse<object> InvalidRequestBodySpecificErrorResponse(string errString) =>
            DefaultResponse(Codes.INVALID_REQUEST_BODY, $"invalid json request body:{errString}");

        public static ApiResponse<object> InvalidQueryParametersErrorResponse() =>
            DefaultResponse(Codes.INVALID_QUERY_PARAMETERS, "invalid query parameters");

        public static ApiResponse<object> MissingAppIdErrorResponse() =>
            DefaultResponse(Codes.MISSING_APP_ID, "missing appId header");

        public static ApiResponse<object> InvalidAppIdErrorResponse() =>
            DefaultResponse(Codes.INVALID_APP_ID, "invalid appId header");

        public static ApiResponse<object> MissingAuthTokenErrorResponse() =>
            DefaultResponse(Codes.MISSING_AUTH_TOKEN, "missing authorization token");

        public static ApiResponse<object> InvalidAuthTokenErrorResponse() =>
            DefaultResponse(Codes.INVALID_AUTH_TOKEN, "invalid authorization token");

        public static ApiResponse<object> InvalidSignatureErrorResponse() =>
            DefaultResponse(Codes.INVALID_SIGNATURE, "invalid signature");

        public static ApiResponse<object> ExistingUserFoundErrorResponse() =>
            DefaultResponse(Codes.EXISTING_USER_FOUND, "existing user found");

        public static ApiResponse<object> ExistingUserNotFoundErrorResponse() =>
            DefaultResponse(Codes.EXISTING_USER_NOT_FOUND, "existing user not found");

        public static ApiResponse<object> GrMemberIdLinkedErrorResponse() =>
            DefaultResponse(Codes.GR_MEMBER_LINKED, "gr profile already linked to another email");

        public static ApiResponse<object> InvalidGrMemberClassErrorResponse() =>
            DefaultResponse(Codes.INVALID_GR_MEMBER_CLASS, "invalid gr member class provided");

        public static ApiResponse<object> CachedProfileNotFoundErrorResponse() =>
            DefaultResponse(Codes.CACHED_PROFILE_NOT_FOUND, "cached profile not found");
    }
}
