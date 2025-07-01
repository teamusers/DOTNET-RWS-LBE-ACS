using Microsoft.AspNetCore.Mvc;
using RWS_LBE_ACS.Common; 
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
                return BadRequest(ResponseTemplate.InvalidRequestBodyErrorResponse());

            await _email.SendPlainTextAsync(req.Email, req.Subject, req.PlainText, req.Attachments);
            return Ok(ResponseTemplate.GenericSuccessResponse(null));
        }

        [HttpPost("html")]
        public async Task<IActionResult> Html([FromBody] SendHtmlEmailRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseTemplate.InvalidRequestBodyErrorResponse());

            await _email.SendHtmlAsync(req.Email, req.Subject, req.Html, req.Attachments);
            return Ok(ResponseTemplate.GenericSuccessResponse(null));
        }

        [HttpPost("template/{name}")]
        public async Task<IActionResult> Template([FromRoute] string name, [FromBody] SendTemplateEmailRequest req)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(ResponseTemplate.InvalidQueryParametersErrorResponse());

            if (!ModelState.IsValid)
                return BadRequest(ResponseTemplate.InvalidRequestBodyErrorResponse());

            await _email.SendTemplateAsync(
                req.Email!,       // recipient email
                req.Subject!,     // email subject
                name,             // template name
                req.Data!,
                req.Attachments
            );
            return Ok(ResponseTemplate.GenericSuccessResponse(null));
        }
    }
}
