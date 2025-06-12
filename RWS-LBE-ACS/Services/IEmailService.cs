 
namespace RWS_LBE_ACS.Services
{
    public interface IEmailService
    {
        Task SendPlainTextAsync(string to, string subject, string body,
                                List<Dictionary<string, string>>? attachments = null);

        Task SendTemplateAsync(string to, string subject, string templateName,
                               Dictionary<string, object> model,
                               List<Dictionary<string, string>>? attachments = null);
    }
}
