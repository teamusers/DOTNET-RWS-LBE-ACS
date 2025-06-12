using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Email; 
using Microsoft.Extensions.Configuration;
using RazorLight;

namespace RWS_LBE_ACS.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailClient _client;
        private readonly RazorLightEngine _razor;
        private readonly string _senderAddress;

        public EmailService(IConfiguration config, IWebHostEnvironment env)
        {
            // 1) Initialize EmailClient
            var connectionString = config.GetConnectionString("EmailService");
            _client = new EmailClient(connectionString);                      // :contentReference[oaicite:2]{index=2}

            // 2) Sender must be a verified domain/email in ACS
            _senderAddress = "DoNotReply@0efe76d7-4c16-4d97-9fd1-adc32956d527.azurecomm.net";

            // this points at the folder that contains your .csproj
            var root = Path.Combine(env.ContentRootPath, "templates");

            _razor = new RazorLightEngineBuilder()
                .UseFileSystemProject(root)
                .EnableDebugMode(true)
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderTemplateAsync(string templateName, object model)
        {
            // RazorLight looks for EXACT file names – no leading slash
            // and matching extension.
            var fileName = $"{templateName}.cshtml";
            return await _razor.CompileRenderAsync(fileName, model);
        }

        public async Task SendPlainTextAsync(string to, string subject, string plainText,
                                             List<Dictionary<string, string>>? attachments = null)
        {
            // Build content object
            var emailContent = new EmailContent(subject)
            {
                PlainText = plainText
            };

            // 2) Construct the message and add attachments
            var message = new EmailMessage(
                senderAddress: _senderAddress,      // your verified sender
                recipients: new EmailRecipients(
                    to: new[] { new EmailAddress(to) }
                ),
                content: emailContent               // content WITHOUT attachments
            );


            // Add attachments if any
            if (attachments != null)
            {
                foreach (var att in attachments)
                {
                    // Read file bytes, wrap in BinaryData
                    var bytes = File.ReadAllBytes(att["path"]);
                    var attachment = new EmailAttachment(
                        name: att["name"],
                        contentType: att["contentType"],
                        content: BinaryData.FromBytes(bytes)
                    );                              // EmailAttachment ctor :contentReference[oaicite:1]{index=1}

                    message.Attachments.Add(attachment);  // <-- works here :contentReference[oaicite:2]{index=2}
                }
            }

            // 3) Send the message
            var response = await _client.SendAsync(
                WaitUntil.Completed,
                message                             // pass the EmailMessage, not separate content
            );                                      // SendAsync overload :contentReference[oaicite:3]{index=3}


            // Optionally inspect status or operation ID
            Console.WriteLine($"Email send status: {response.Value.Status}"); // :contentReference[oaicite:5]{index=5}
        }

        public async Task SendTemplateAsync(string to, string subject, string templateName,
                                    Dictionary<string, object> model,
                                    List<Dictionary<string, string>>? attachments = null)
        {
            // 1) Render template
            var htmlBody = await RenderTemplateAsync(templateName, model);

            // 2) Build content
            var content = new EmailContent(subject) { Html = htmlBody };

            // 3) Build recipients
            var recipients = new EmailRecipients(
                to: new[] { new EmailAddress(to) }
            );

            // 4) Construct EmailMessage (attachments omitted for brevity)
            var message = new EmailMessage(_senderAddress, recipients, content);

            // 5) Send & poll
            var operation = await _client.SendAsync(WaitUntil.Started, message);
            await operation.WaitForCompletionAsync();

            Console.WriteLine($"Email queued for delivery. Status = {operation.Value.Status}");
        }
    }
}
