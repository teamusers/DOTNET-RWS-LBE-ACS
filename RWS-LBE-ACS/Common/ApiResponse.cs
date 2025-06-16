using System.Text.Json;
using System.Text.Json.Serialization; 

namespace RWS_LBE_ACS.Common
{
    /// <summary>
    /// Standard envelope for API responses.
    /// The Data field contains the payload, which varies by endpoint.
    /// </summary>
    public class ApiResponse
    {
        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public object? Data { get; set; }
    }

    /// <summary>
    /// Non-generic helpers for producing common ApiResponse&lt;object&gt; instances.
    /// </summary>
    public static class ResponseTemplate
    {
        public static ApiResponse DefaultResponse(long code, string message) =>
            new ApiResponse { Code = code, Message = message, Data = null };

        public static ApiResponse? GenericSuccessResponse(object? data) =>
            new ApiResponse { Code = Codes.SUCCESSFUL, Message = "successful", Data = data };

        public static ApiResponse InternalErrorResponse() =>
            DefaultResponse(Codes.INTERNAL_ERROR, "internal error");

        public static ApiResponse InvalidRequestBodyErrorResponse() =>
            DefaultResponse(Codes.INVALID_REQUEST_BODY, "invalid json request body");

        public static ApiResponse InvalidRequestBodySpecificErrorResponse(string errString) =>
            DefaultResponse(Codes.INVALID_REQUEST_BODY, $"invalid json request body:{errString}");

        public static ApiResponse InvalidQueryParametersErrorResponse() =>
            DefaultResponse(Codes.INVALID_QUERY_PARAMETERS, "invalid query parameters");

        public static ApiResponse MissingAppIdErrorResponse() =>
            DefaultResponse(Codes.MISSING_APP_ID, "missing appId header");

        public static ApiResponse InvalidAppIdErrorResponse() =>
            DefaultResponse(Codes.INVALID_APP_ID, "invalid appId header");

        public static ApiResponse MissingAuthTokenErrorResponse() =>
            DefaultResponse(Codes.MISSING_AUTH_TOKEN, "missing authorization token");

        public static ApiResponse InvalidAuthTokenErrorResponse() =>
            DefaultResponse(Codes.INVALID_AUTH_TOKEN, "invalid authorization token");

        public static ApiResponse InvalidSignatureErrorResponse() =>
            DefaultResponse(Codes.INVALID_SIGNATURE, "invalid signature");

        public static ApiResponse ExistingUserFoundErrorResponse() =>
            DefaultResponse(Codes.EXISTING_USER_FOUND, "existing user found");

        public static ApiResponse ExistingUserNotFoundErrorResponse() =>
            DefaultResponse(Codes.EXISTING_USER_NOT_FOUND, "existing user not found");

        public static ApiResponse GrMemberIdLinkedErrorResponse() =>
            DefaultResponse(Codes.GR_MEMBER_LINKED, "gr profile already linked to another email");

        public static ApiResponse InvalidGrMemberClassErrorResponse() =>
            DefaultResponse(Codes.INVALID_GR_MEMBER_CLASS, "invalid gr member class provided");

        public static ApiResponse CachedProfileNotFoundErrorResponse() =>
            DefaultResponse(Codes.CACHED_PROFILE_NOT_FOUND, "cached profile not found");
    }

    public class ApiException
    {
        public static bool TryParseJson<T>(string raw, out T? result)
        {
            try
            {
                result = JsonSerializer.Deserialize<T>(raw);
                return true;
            }
            catch (JsonException)
            {
                result = default;
                return false;
            }
        }
    }
}
