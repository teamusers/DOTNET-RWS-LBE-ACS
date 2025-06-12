
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RWS_LBE_ACS.Common;
using RWS_LBE_ACS.DTOs.Requests;
using RWS_LBE_ACS.Services;
using static RWS_LBE_ACS.DTOs.Requests.Send_email; 

namespace RWS_LBE_ACS.Controllers
{
    [ApiController]
    [Route("api/v1/send")]
    public class SendController : ControllerBase
    {
        private readonly IEmailService _email;

        public SendController(IEmailService email) => _email = email;

        [HttpPost("plain-text")]
        public async Task<IActionResult> PlainText([FromBody] SendPlainTextEmailRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.InvalidRequestBodyErrorResponse());

            await _email.SendPlainTextAsync(req.Email, req.Subject, req.PlainText, req.Attachments);
            return Ok(new ApiResponse<object>
            {
                Code = Codes.SUCCESSFUL,
                Message = "email sent",
                Data = null
            });
        }

        [HttpPost("template/{name}")]
        public async Task<IActionResult> Template([FromRoute] string name,[FromBody] SendTemplateEmailRequest req)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(ApiResponse.InvalidQueryParametersErrorResponse());
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.InvalidRequestBodyErrorResponse());

            // pass the *actual* email address (req.Email) first, then subject, then template name:
            await _email.SendTemplateAsync(
                req.Email!,       // ← the real “to” address
                req.Subject!,     // ← subject
                name,             // ← template key (e.g. "request_email_otp")
                req.Data!,
                req.Attachments
            );
            return Ok(new ApiResponse<object>
            {
                Code = Codes.SUCCESSFUL,
                Message = "email sent",
                Data = null
            });
        }
    }
}

