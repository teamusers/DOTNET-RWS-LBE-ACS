namespace RWS_LBE_ACS.Common
{
    public class Codes
    {
        // SignUpType enums
        public const string SignUpTypeNew = "NEW";
        public const string SignUpTypeGR = "GR";
        public const string SignUpTypeGRCMS = "GR_CMS";
        public const string SignUpTypeTM = "TM";

        // Legacy numeric codes
        public const int CODE_SUCCESS = 0;
        public const int CODE_ERR_METHOD_UNSUPPORT = 1;
        public const int CODE_ERR_REQFORMAT = 2;
        public const int CODE_ERR_APPID_INVALID = 3;
        public const int CODE_ERR_SIGMETHOD_UNSUPP = 4;
        public const int CODE_ERR_AUTHTOKEN_FAIL = 5;

        // Higher-level result codes
        public const int SUCCESSFUL = 1000;
        public const int UNSUCCESSFUL = 1001;
        public const int FOUND = 1002;
        public const int NOT_FOUND = 1003;

        // Error codes (4xxx)
        public const int INTERNAL_ERROR = 4000;
        public const int INVALID_REQUEST_BODY = 4001;
        public const int INVALID_AUTH_TOKEN = 4002;
        public const int MISSING_AUTH_TOKEN = 4003;
        public const int INVALID_SIGNATURE = 4004;
        public const int MISSING_SIGNATURE = 4005;
        public const int INVALID_APP_ID = 4006;
        public const int MISSING_APP_ID = 4007;
        public const int INVALID_QUERY_PARAMETERS = 4008;
        public const int EXISTING_USER_NOT_FOUND = 4009;
        public const int EXISTING_USER_FOUND = 4010;
        public const int CACHED_PROFILE_NOT_FOUND = 4011;
        public const int GR_MEMBER_LINKED = 4012;
        public const int GR_MEMBER_NOT_FOUND = 4013;
        public const int INVALID_GR_MEMBER_CLASS = 4014;

        /// <summary>
        /// Validates whether the provided type is one of the known SignUpType constants.
        /// </summary>
        public static bool IsValidSignUpType(string t)
        {
            return t == SignUpTypeNew
                || t == SignUpTypeGR
                || t == SignUpTypeGRCMS
                || t == SignUpTypeTM;
        }
    }
}
