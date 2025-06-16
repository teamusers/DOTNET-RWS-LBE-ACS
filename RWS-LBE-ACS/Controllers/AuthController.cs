using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWS_LBE_ACS.Common;
using RWS_LBE_ACS.Data;
using RWS_LBE_ACS.DTOs.Requests;
using RWS_LBE_ACS.DTOs.Responses;
using RWS_LBE_ACS.Services.Authentication;
using RWS_LBE_ACS.Services; 

namespace RWS_LBE_Transaction.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _authService;

        public AuthController(AppDbContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        /// <summary>
        /// Generate authentication token
        /// </summary>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> Post(
            [FromHeader(Name = "AppID")] string appId,
            [FromBody] AuthRequest req)
        {
            // 1) Require AppID header
            if (string.IsNullOrWhiteSpace(appId))
                return Unauthorized(ResponseTemplate.MissingAppIdErrorResponse());

            // 2) Validate JSON body
            if (!ModelState.IsValid)
                return BadRequest(ResponseTemplate.InvalidRequestBodyErrorResponse());

            // 3) Look up secret key
            var channel = await _db.SysChannel.SingleOrDefaultAsync(c => c.AppId == appId);
            if (channel == null)
                return Unauthorized(ResponseTemplate.InvalidAppIdErrorResponse());

            var secretKey = channel.AppKey;

            if (string.IsNullOrWhiteSpace(req.Nonce) || string.IsNullOrWhiteSpace(req.Timestamp))
                return BadRequest(ResponseTemplate.InvalidRequestBodyErrorResponse());

            // 4) Compute our own signature
            var computed = _authService.GenerateSignatureWithParams(appId, req.Nonce, req.Timestamp, secretKey!);

            // 5) Compare signatures
            if (computed.Signature != req.Signature)
                return Unauthorized(ResponseTemplate.InvalidSignatureErrorResponse());

            // 6) Generate JWT
            var token = TokenInterceptor.GenerateToken(appId);

            // 7) Return wrapped response
            var responseData = new AuthResponseData { AccessToken = token };
            var resp = ResponseTemplate.GenericSuccessResponse(responseData);

            return Ok(resp);
        }

        /// <summary>
        /// Fallback for missing query parameters
        /// </summary>
        [HttpGet("invalid-query")]
        public IActionResult InvalidQuery() => BadRequest(ResponseTemplate.InvalidQueryParametersErrorResponse());
    }
}
